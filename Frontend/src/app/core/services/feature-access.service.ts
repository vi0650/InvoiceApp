import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class FeatureAccessService {

  constructor(private auth :AuthService){}

  private readonly STORAGE_KEY = 'access';

  private access$ = new BehaviorSubject<boolean>(
    this.loadFromStorage()
  );

  productAccessChange$ = this.access$.asObservable();

  setProductAccess(value: boolean): void {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(value));
    this.access$.next(value);
  }

  getProductAccess(): boolean {
    localStorage.getItem(this.STORAGE_KEY);
    return this.access$.value;
  }

  private loadFromStorage(): boolean {
    const stored = localStorage.getItem(this.STORAGE_KEY);
    return stored ? JSON.parse(stored) : false;
  }
}