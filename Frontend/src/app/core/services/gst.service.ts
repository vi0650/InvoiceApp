import { Injectable } from '@angular/core';
import { Gst } from '../models/gst.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class GstService {

  private readonly baseUri = `${environment.apiUrl}/Gst`;

  constructor(private http: HttpClient) { }

  getAllGst() {
    return this.http.get<Gst[]>(this.baseUri)
  }

  getGstByUserId(id: string) {
    return this.http.get<Gst>(`${this.baseUri}/${id}`);
  }

  getGstById(id: string) {
    return this.http.get<Gst>(`${this.baseUri}/${id}`);
  }

  addGst(gst: Gst): Observable<Gst> {
    return this.http.post<Gst>(this.baseUri, gst);
  }

  updateGst(id: string, gst: Gst): Observable<void> {
    return this.http.put<void>(`${this.baseUri}/${id}`, gst);
  }

  deleteGst(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUri}/${id}`);
  }
}
