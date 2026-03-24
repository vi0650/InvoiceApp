import { Injectable } from '@angular/core';
import { Invoice } from '../models/invoice.model';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {

  private readonly baseUri = `${environment.apiUrl}/Invoice`
  private loaded = false;
  private invoices$ = new BehaviorSubject<Invoice[]>([]);

  constructor(private http: HttpClient) { }

  loadInvoices(){
    if (this.loaded) return;
    this.http.get<any>(this.baseUri).subscribe(res =>{
      this.invoices$.next(res.value?.data ?? []);
      this.loaded = true;
    })
  }

  getAllInvoices() {
    return this.http.get<Invoice[]>(this.baseUri)
  }

  getInvoiceById(id: string) {
    return this.http.get<Invoice>(`${this.baseUri}/${id}`);
  }

  addInvoice(invoice: Invoice): Observable<Invoice> {
    return this.http.post<Invoice>(this.baseUri, invoice);
  }

  updateInvoice(id: string, invoice: Invoice): Observable<void> {
    return this.http.put<void>(`${this.baseUri}/${id}`, invoice);
  }

  deleteInvoice(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUri}/${id}`);
  }
}