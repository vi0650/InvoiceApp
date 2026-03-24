import { MediaMatcher } from '@angular/cdk/layout';
import { Component, inject, signal, ViewChild, OnDestroy, viewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatAccordion } from '@angular/material/expansion';
import { AuthService } from '../core/services/auth.service';
import { Role } from '../core/models/user.model';
import { FeatureAccessService } from '../core/services/feature-access.service';

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnDestroy {

  private media = inject(MediaMatcher);
  private router = inject(Router);
  accordion = viewChild.required(MatAccordion);

  isMobile = signal(false);
  userName: string = "";
  shopName: string = "";
  canAccessPage?: boolean;

  private mobileQuery: MediaQueryList = this.media.matchMedia('(max-width: 900px)');
  private mobileListener = () => this.isMobile.set(this.mobileQuery.matches);

  @ViewChild('snav') snav: any;

  constructor(private auth: AuthService, private featureAccess: FeatureAccessService) {
    console.log('Layout module loaded');
    this.isMobile.set(this.mobileQuery.matches);
    this.mobileQuery.addEventListener('change', this.mobileListener);
    const userdata = auth.getUserData();
    this.userName = (userdata ? userdata.userName : null);
    this.shopName = (userdata ? userdata.shopName : null);
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeEventListener('change', this.mobileListener);
  }

  ngOnInit(): void {
    const role = this.auth.getUserData();
    if (role?.role !== Role.SuperAdmin && Role.User) {
      this.featureAccess.productAccessChange$
        .subscribe((access: any) => {
          this.canAccessPage = access;
        });
    }
  }

  closeSidebar() {
    if (this.isMobile() && this.snav) {
      this.snav.close();
    }
  }

  canAccess(): boolean {
    return this.auth.hasRole([Role.SuperAdmin]);
  }

  settings() {
    this.router.navigate(['/layout/settings'])
  }

  logout() {
    this.auth.signOut()
    this.router.navigate(['/login'], { replaceUrl: true });
  }
}