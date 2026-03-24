import { Component, inject, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GstService } from '../../core/services/gst.service';
import { HotToastService } from '@ngxpert/hot-toast';
import { Route, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { Gst } from '../../core/models/gst.model';
import { MatTableDataSource } from '@angular/material/table';
import { GST_COL } from '../../core/data/tabledata/gstColumns';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Role } from '../../core/models/user.model';
import { hotToastObserve } from '../../core/utils/toast-observer';

@Component({
  selector: 'app-gst',
  standalone: false,
  templateUrl: './gst.component.html',
  styleUrl: './gst.component.scss'
})
export class GstComponent {

  readonly path = '/layout/gst/gst-form';
  readonly dialog = inject(MatDialog);

  constructor(
    private gstService: GstService,
    private toast: HotToastService,
    private router: Router,
    private auth: AuthService
  ) { }

  dataSource = new MatTableDataSource<Gst>();
  displayColumns = GST_COL;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadGst();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data, filter) =>
      data.rateId.toString().includes(filter) ||
      data.gstRate.toString().includes(filter);
  }

  canAddGst(): boolean {
    return this.auth.hasRole([Role.SuperAdmin, Role.User]);
  }

  loadGst() {
    this.gstService.getAllGst().pipe(
      hotToastObserve(this.toast, {
        loading: 'Loading...',
        success: () => '',
        error: (err) => {
          if (err.status === 0) {
            return 'server is offline!';
          }
          if (err.status === 401) {
            return 'Unauthorized access';
          }
          if (err.status === 403) {
            return 'Unauthorized access';
          }
          return 'Internal server error !!!';
        },
      }),
    ).subscribe({
        next: (res: any) => {
          console.log(res);
          this.dataSource.data = res.value.data || [];
          console.log(res.value.data);
        },
      });
  }

  applyFilter(event: any) {
    const value = event.target.value.trim().toLowerCase();
    this.dataSource.filter = value;
  }

  editGst(row: Gst) {
    console.log(row.rateId);
    this.router.navigate([this.path, row.rateId]);
  }

  deleteGst(row: Gst) {
    this.gstService
      .deleteGst(row.rateId)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Deleting GST...',
          success: () => 'Gst Deleted',
          error: (err) => {
            if (err.status === 0) return 'server offline!';
            if (err.status === 401) return 'Unauthorized access';
            if (err.status === 403) return 'You don’t have permission to perform this action';
            if (err.status === 409) return `${err.error.message}`;
            if (err.status === 500) return 'You have invoices of this product please delete first'
            return 'Something Crashed!';
          },
        }),
      )
      .subscribe(() => this.loadGst());
  }

  addGst() {
    this.router.navigate([this.path]);
  }

}
