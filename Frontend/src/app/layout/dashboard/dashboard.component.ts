import { Component } from '@angular/core';
import { take } from 'rxjs';
import { Invoice } from '../../core/models/invoice.model';
import { InvoiceService } from '../../core/services/invoice.service';
import { DashboardState } from '../../core/models/dashboardState.model';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  stats: DashboardState[] = [];
  invoices: Invoice[] = [];
  loading = true;
  private readonly today = new Date();

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  private loadDashboard() {
    this.invoiceService.refreshInvoices().pipe(take(1)).subscribe({
      next: (invoices) => {
        this.invoices = invoices;
        this.stats = this.buildStats(invoices);
        this.loading = false;
        console.log(invoices);
        
      },
      error: () => {
        this.stats = this.buildStats([]);
        this.loading = false;
      }
    });
  }

  private buildStats(invoices: Invoice[]): DashboardState[] {
    const paidInvoices = invoices.filter((invoice) => this.getStatus(invoice) === 'paid');
    const pendingInvoices = invoices.filter((invoice) => this.getStatus(invoice) === 'sent');
    const overdueInvoices = invoices.filter((invoice) =>
      this.getStatus(invoice) !== 'paid' && this.isOverdue(invoice.invoiceDate)
    );

    const totalRevenue = paidInvoices.reduce((sum, invoice) => sum + (invoice.grandTotal ?? 0), 0);
    const outstandingAmount = invoices
      .filter((invoice) => this.getStatus(invoice) !== 'paid')
      .reduce((sum, invoice) => sum + (invoice.grandTotal ?? 0), 0);

    return [
      { label: 'Total Revenue', value: totalRevenue, type: 'currency' },
      { label: 'Paid Invoices', value: paidInvoices.length, type: 'count' },
      { label: 'Pending', value: pendingInvoices.length, type: 'count' },
      { label: 'Overdue', value: overdueInvoices.length, type: 'count', cardClass: 'overdue' },
      { label: 'Outstanding Amount', value: outstandingAmount, type: 'currency' }
    ];
  }

  private getStatus(invoice: Invoice) {
    return (invoice.invoiceStatus ?? '').toLowerCase();
  }

  private isOverdue(invoiceDate: string) {
    const dueDate = new Date(invoiceDate);
    return !Number.isNaN(dueDate.getTime()) && dueDate < this.today;
  }
}
