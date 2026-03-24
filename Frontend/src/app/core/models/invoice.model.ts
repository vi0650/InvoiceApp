import { DecimalPipe } from "@angular/common";

export interface Invoice {
  invoiceId:string;
  userId: string;
  customerId: string;
  customerName: string;
  email: string;
  mobileNo: string;
  address: string;
  invoiceDate: string;

  invoiceItems: InvoiceItem[];

  description?: string | null;
  invoiceStatus: string;
  subTotal: number;
  gstAmount: number;
  grandTotal: number;
}

export interface InvoiceItem {
  productId: string;
  productName: string;
  rate: number;
  qty: number;
  gst: number;
  amount: number;
}