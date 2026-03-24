import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly baseUrl = `${environment.apiUrl}/Auth/Login`;
  private token = 'token';

  constructor(private http: HttpClient) { }

  loginUser(data: any) {
    return this.http.post<any>(this.baseUrl, data)
      .pipe(
        tap(res => {
          const token = res.data.token;
          if (!token) {
            return;
          } else {
            localStorage.setItem(this.token, token)
          }
        })
      );
  }

  getToken() {
    return localStorage.getItem(this.token);
  }

  signOut() {
    return localStorage.removeItem(this.token);
  }

  getDecodeToken() {
    const token = this.getToken();
    if (!token || token === 'undefined') return null;
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error("decode failed", e);
      return null;
    }
  }

  getUserData() {
    const decoded = this.getDecodeToken();
    if (!decoded) return;
    return {
      userId: decoded['nameid'],
      userName: decoded['unique_name'],
      email: decoded['email'],
      role: decoded['role'],
      shopName: decoded['ShopName'],
      isActive: decoded['IsActive']
    }
  }

  isLoggedIn(): boolean {
    const decode = this.getDecodeToken();
    if (!decode) return false;
    const now = Math.floor(Date.now() / 1000);
    return decode.exp > now;
  }

  hasRole(roles: string[]): boolean {
    const role = this.getUserData()?.role;
    return role ? roles.includes(role) : false;
  }
}