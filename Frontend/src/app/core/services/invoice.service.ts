import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { Invoice } from '../models/invoice.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {
  private readonly baseUri = `${environment.apiUrl}/Invoice`;
  private loaded = false;
  private readonly invoicesSubject = new BehaviorSubject<Invoice[]>([]);

  readonly invoices$ = this.invoicesSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadInvoices() {
    if (this.loaded) {
      return;
    }

    this.refreshInvoices().subscribe();
  }

  getAllInvoices() {
    return this.http.get<any>(this.baseUri).pipe(
      map((res) => res?.value?.data ?? []),
      tap((invoices: Invoice[]) => {
        this.invoicesSubject.next(invoices);
        this.loaded = true;
      })
    );
  }

  refreshInvoices() {
    this.loaded = false;
    return this.getAllInvoices();
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
