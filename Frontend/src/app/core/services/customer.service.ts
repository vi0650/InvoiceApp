import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Customer } from '../models/customer.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private readonly baseUri = `${environment.apiUrl}/Customer`

  constructor(private http: HttpClient) { }

  getAllCustomers() {
    return this.http.get<Customer[]>(this.baseUri)
  }

  getCustomerById(id: string) {
    return this.http.get<Customer>(`${this.baseUri}/${id}`);
  }

  addCustomer(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>(this.baseUri, customer);
  }

  updateCustomer(id: string, customer: Customer): Observable<void> {
    return this.http.put<void>(`${this.baseUri}/${id}`, customer);
  }

  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUri}/${id}`);
  }
}
