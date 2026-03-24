import { Location } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FeatureAccessService } from '../../core/services/feature-access.service';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {

  constructor(private location: Location, private featureAccess: FeatureAccessService) { }

  readonly panelOpenState = signal(false);
  checked = false;

  ngOnInit(): void {
    this.featureAccess.productAccessChange$
      .subscribe(value => {
        this.checked = value;
      });
  }

  back() {
    this.location.back();
  }

  accessToggle(event: boolean) {
    this.featureAccess.setProductAccess(event);
  }

}
