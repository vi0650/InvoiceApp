import { ChangeDetectionStrategy, Component, Inject, inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { HotToastService } from '@ngxpert/hot-toast';
import { Router } from '@angular/router';
import { hotToastObserve } from '../../core/utils/toast-observer';
import { InvoiceService } from '../../core/services/invoice.service';
import { Invoice } from '../../core/models/invoice.model';
import { INVOICE_COL } from '../../core/data/tabledata/invoiceColumns';
import { NgUIModule } from '../../shared/ng-ui.module';
import { take } from 'rxjs';

@Component({
  selector: 'app-invoice',
  standalone: false,
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.scss'
})
export class InvoiceComponent {

  readonly path = '/layout/invoices/invoices-form';
  readonly dialog = inject(MatDialog);

  constructor(private invoiceService: InvoiceService, private toast: HotToastService, private router: Router) { }

  dataSource = new MatTableDataSource<Invoice | any>();
  displayColumns = INVOICE_COL

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  ngOnInit(): void {
    this.loadInvoice();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data, filter) =>
      data.invoiceStatus.toLowerCase().includes(filter) ||
      data.invoiceId.toString().includes(filter) ||
      data.customer.customerName.toLowerCase().includes(filter) ||
      data.customer.email.toLowerCase().includes(filter) ||
      data.grandTotal.toString().includes(filter) ||
      data.createdAt.toString().includes(filter) ||
      data.updatedAt.toString().includes(filter);
  }

  loadInvoice() {
    this.invoiceService.getAllInvoices().pipe(
      take(1),
      hotToastObserve(this.toast, {
        loading: "Loading...",
        success: () => "",
        error: (err) => {
          if (err.status === 0) { return 'server is offline!' };
          if (err.status === 401) { return 'Unauthorized access' };
          if (err.status === 403) { return 'Unauthorized access' };
          return "Internal server error !!!";
        }
      })
    ).subscribe({
      next: (res: any) => {
        console.log(res);
        this.dataSource.data = res.value.data ?? [];
      },
    })
  }

  applyFilter(event: any) {
    const value = event.target.value.trim().toLowerCase();
    this.dataSource.filter = value;
  }

  editInvoice(row: Invoice) {
    this.router.navigate([this.path, row.invoiceId]);
  }

  deleteInvoice(row: Invoice) {
    const dialogRef = this.dialog.open(invoiceDeleteDialog, {
      width: '400px',
      data: row.invoiceId,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if(result !== 'confirm') return;
      this.invoiceService.deleteInvoice(row.invoiceId)
        .pipe(
          hotToastObserve(this.toast, {
            loading: "Deleting Invoice...",
            success: () => `Invoice Deleted!`,
            error: (err) => {
              if (err.status === 0) return "server is offline!";
              if (err.status === 401) return "Unauthorized access";
              if (err.status === 403) { return 'You don’t have permission to perform this action' };
              return "Internal server error !!!";
            }
          }),
        ).subscribe(() => this.loadInvoice());
    })
  }

  addInvoice() {
    this.router.navigate([this.path]);
  }

  viewInvoice(id: string) {
    this.router.navigate(['/layout/invoices/invoices-show', id])
  }

  printInvoice(invoice: Invoice) {
    this.router.navigate(
      ['/layout/invoices/invoices-show', invoice.invoiceId],
      { queryParams: { print: 'true' } }
    );
  }
}

@Component({
  selector: 'invoice-delete-dialog',
  templateUrl: './dialogs/invoice-delete-dialog.html',
  imports: [NgUIModule],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class invoiceDeleteDialog {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<invoiceDeleteDialog>,
  ) {
    // console.log(this.data);
  }

  confirm() {
    this.dialogRef.close('confirm');
  }

  close() {
    this.dialogRef.close('cancel');
  }
}
