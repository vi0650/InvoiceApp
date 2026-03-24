import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {


  displayedColumns = ['number', 'client', 'amount', 'status'];

  invoices = [
    { number: 'INV-0012', client: 'ABC Pvt Ltd', amount: 12000, status: 'Paid' },
    { number: 'INV-0013', client: 'XYZ Corp', amount: 18000, status: 'Pending' }
  ];


}
