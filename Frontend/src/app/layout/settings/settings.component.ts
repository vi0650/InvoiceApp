import { Location } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FeatureAccessService } from '../../core/services/feature-access.service';
import { HotToastService } from '@ngxpert/hot-toast';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {

  constructor(private location: Location, private featureAccess: FeatureAccessService, private toast:HotToastService) { }

  readonly panelOpenState = signal(false);
  checked = false;

  ngOnInit(): void {
    this.featureAccess.productAccessChange$
      .subscribe(value => {
        this.checked = value;
      });
      this.toast.info("this feature is currently not working",{dismissible:true})
  }

  back() {
    this.location.back();
  }

  accessToggle(event: boolean) {
    // this.featureAccess.setProductAccess(event);
    this.toast.info("this feature is currently not working")
  }

}
