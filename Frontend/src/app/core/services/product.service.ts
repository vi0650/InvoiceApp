import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly baseUri = `${environment.apiUrl}/Product`;

  constructor(private http: HttpClient) { }

  getAllProducts() {
    return this.http.get<Product[]>(this.baseUri)
  }

  getProductByUserId(id: string) {
    return this.http.get<Product>(`${this.baseUri}/${id}`);
  }

  getProductById(id: string) {
    return this.http.get<Product>(`${this.baseUri}/${id}`);
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.baseUri, product);
  }

  updateProduct(id: string, product: Product): Observable<void> {
    return this.http.put<void>(`${this.baseUri}/${id}`, product);
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUri}/${id}`);
  }

}
