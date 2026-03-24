import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InvoiceService } from '../../../core/services/invoice.service';
import { Invoice } from '../../../core/models/invoice.model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-show-invoice',
  standalone: false,
  templateUrl: './show-invoice.component.html',
  styleUrl: './show-invoice.component.scss'
})
export class ShowInvoiceComponent implements OnInit, OnDestroy  {

  invoice!: Invoice | any;
  shouldAutoPrint = false;

  constructor(private route:ActivatedRoute, private location:Location, private invoiceService:InvoiceService){}

  ngOnInit(): void {
    document.body.classList.add('invoice-print-view');

    const id = this.route.snapshot.paramMap.get('id');
    this.shouldAutoPrint = this.route.snapshot.queryParamMap.get('print') === 'true';

    if (id) {
      this.loadInvoice(id);
    }
  }

  loadInvoice(id:string){
    this.invoiceService.getInvoiceById(id).subscribe((res :any)=>{
      this.invoice = res.data;
      if (this.shouldAutoPrint) {
        setTimeout(() => this.printInvoice(), 350);
      }
    });
  }

  goBack() {
    this.location.back();
  }

  printInvoice() {
    window.print();
  }

  ngOnDestroy(): void {
    document.body.classList.remove('invoice-print-view');
  }
}
