import { Component, inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { HotToastService } from '@ngxpert/hot-toast';
import { Router } from '@angular/router';
import { hotToastObserve } from '../../core/utils/toast-observer';
import { CustomerService } from '../../core/services/customer.service';
import { CUSTOMER_COL } from '../../core/data/tabledata/customerColumns';
import { Customer } from '../../core/models/customer.model';
import { Role } from '../../core/models/user.model';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-customer',
  standalone: false,
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss',
})
export class CustomerComponent {
  readonly path = '/layout/customers/customer-form';
  readonly dialog = inject(MatDialog);

  constructor(
    private customerService: CustomerService,
    private toast: HotToastService,
    private router: Router,
    private auth : AuthService
  ) { }

  dataSource = new MatTableDataSource<Customer>();
  displayColumns = CUSTOMER_COL;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadCustomers();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data, filter) =>
      data.customerName.toLowerCase().includes(filter) ||
      data.customerId.toString().includes(filter) ||
      data.mobileNo.toString().includes(filter);
  }

  canAddUser(): boolean {
    return this.auth.hasRole([Role.SuperAdmin,Role.User]);
  }

  loadCustomers() {
    this.customerService
      .getAllCustomers()
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Loading...',
          success: () => '',
          error: (err) => {
            if (err.status === 0) return 'server is offline!';
            if (err.status === 401) { return 'Unauthorized access' };
            if (err.status === 403) { return 'Unauthorized access' };
            return 'Internal server error !!!';
          },
        }),
      ).subscribe({
        next: (res: any) => {
          console.log(res);
          this.dataSource.data = res.value.data || [];
        },
      });
  }

  applyFilter(event: any) {
    const value = event.target.value.trim().toLowerCase();
    this.dataSource.filter = value;
  }

  editCustomer(row: Customer) {
    this.router.navigate([this.path, row.customerId]);
  }

  deleteCustomer(row: Customer) {
    this.customerService.deleteCustomer(row.customerId).pipe(
      hotToastObserve(this.toast, {
        loading: 'deleting customer...',
        success: () => `${row.customerName} Deleted!`,
        error: (err) => {
          if (err.status === 0) return 'server is offline!';
          if (err.status === 401) return 'Unauthorized access';
          if (err.status === 403) { return 'You don’t have permission to perform this action' };
          if (err.status === 409) { return `${err.error.message}` };
          if (err.status === 500) return 'You have invoices of this customer please delete first'
          return 'Internal server error !!!';
        },
      }),
    ).subscribe(() => this.loadCustomers());
  }

  addCustomer() {
    this.router.navigate([this.path]);
  }
}
