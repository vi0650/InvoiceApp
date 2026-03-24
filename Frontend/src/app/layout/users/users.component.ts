import { Component, inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserService } from '../../core/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { HotToastService } from '@ngxpert/hot-toast';
import { Router } from '@angular/router';
import { hotToastObserve } from '../../core/utils/toast-observer';
import { USER_COL } from '../../core/data/tabledata/userColumns';
import { Role, User } from '../../core/models/user.model';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  standalone: false,
  styleUrl: './users.component.scss',
})
export class UsersComponent {
  readonly path = 'layout/users/users-form';
  readonly dialog = inject(MatDialog);
  isLoading = false;
  disable = false;

  constructor(
    private userService: UserService,
    private toast: HotToastService,
    private router: Router,
    private auth: AuthService
  ) { }
  
  dataSource = new MatTableDataSource<User>();
  displayColumns = USER_COL;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadUsers();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data, filter) =>
      data.userName.toLowerCase().includes(filter) ||
      data.userId.toString().includes(filter) ||
      data.role.toString().includes(filter);
  }

  canAddUser(): boolean {
    return this.auth.hasRole([Role.SuperAdmin]);
  }

  loadUsers() {
    this.userService.getAllUsers().pipe(
      hotToastObserve(this.toast, {
        loading: 'Loading...',
        success: () => '',
        error: (err) => {
          if (err.status === 0) { return 'server offline!' };
          if (err.status === 401 || err.status === 403) { return 'Unauthorized access' };
          return 'Something Crashed!';
        },
      }),
    ).subscribe({
      next: (res: any) => {
        console.log(res);
        this.dataSource.data = res.value.data || [];
      },
      error: () => {
        this.toast.error('Failed to load users');
      },
    });
  }

  applyFilter(event: any) {
    const value = event.target.value.trim().toLowerCase();
    this.dataSource.filter = value;
  }

  editUser(row: User) {
    this.router.navigate([this.path, row.userId]);
  }

  deleteUser(row: User) {
    this.userService
      .deleteUser(row.userId)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Deleting User...',
          success: () => `${row.userName} Deleted!`,
          error: (err) => {
            if (err.status === 0) return 'server offline!';
            if (err.status === 401) return 'invalid request';
            return 'Something Crashed!';
          },
        }),
      ).subscribe(() => this.loadUsers());
  }

  addUser() {
    this.router.navigate([this.path]);
  }
}
