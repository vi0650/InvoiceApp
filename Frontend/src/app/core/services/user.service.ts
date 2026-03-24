import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from './../models/user.model';
import { ApiResponse } from '../models/api-response.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  private readonly baseUri = `${environment.apiUrl}/User`;

  getAllUsers() {
    return this.http.get<User[]>(this.baseUri)
  }

  getUserById(id: string) {
    return this.http.get<ApiResponse<User>>(`${this.baseUri}/${id}`);
  }

  addUser(user: User) :Observable<User>{
    return this.http.post<User>(this.baseUri, user);
  }

  updateUser(id: string, user: User):Observable<void> {
    return this.http.put<void>(`${this.baseUri}/${id}`, user);
  }

  deleteUser(id: string):Observable<void> {
    return this.http.delete<void>(`${this.baseUri}/${id}`);
  }
}
