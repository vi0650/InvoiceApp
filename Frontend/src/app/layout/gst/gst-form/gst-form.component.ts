import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Gst } from '../../../core/models/gst.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { hotToastObserve } from '../../../core/utils/toast-observer';
import { NgUIModule } from '../../../shared/ng-ui.module';
import { ActivatedRoute, Router } from '@angular/router';
import { GstService } from '../../../core/services/gst.service';
import { HotToastService } from '@ngxpert/hot-toast';
import { AuthService } from '../../../core/services/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-gst-form',
  standalone: false,
  templateUrl: './gst-form.component.html',
  styleUrl: './gst-form.component.scss'
})
export class GstFormComponent {
  readonly dialog = inject(MatDialog);
  gst: Gst[] = [];
  gstForm: FormGroup;
  rateId!: string;
  isUpdate = false;

  private buildForm(): FormGroup {
    return this.fb.group({
      userId: [this.loadUserId()],
      gst: ['', Validators.required],
      isActive: [false]
    })
  }

  private loadUserId() {
    const user = this.auth.getUserData()?.userId;
    return user;
  }

  private createGst(gst: Gst) {
    this.gstService.addGst(gst).pipe(
      hotToastObserve(this.toast, {
        loading: "Just a Second....",
        success: () => `Successfully Saved!`,
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          if (err.status === 409) return `${err.error.message}`;
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      // this.existingIds.push(product.productId);
      this.gstForm.reset();
      this.router.navigate(['/layout/gst']);
    });
  }

  private updateGstData(gst: Gst) {
    this.gstService.updateGst(this.rateId, gst).pipe(
      hotToastObserve(this.toast, {
        loading: "Updating data....",
        success: () => 'gst data Updated successfully',
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          if (err.status === 409) return `${err.error.message}`;
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      console.log(gst.rateId, gst);
      this.router.navigate(['/layout/gst']);
    })
  }

  private loadGstData(id: string) {
    this.gstService.getGstById(id).subscribe({
      next: (res: any) => {
        const gst = res?.data || res;
        console.log(gst);
        if (!gst) return;
        this.gstForm.patchValue(gst);
      }
    })
  }

  constructor(private route: ActivatedRoute, private location: Location, private auth: AuthService, private fb: FormBuilder, private toast: HotToastService, private gstService: GstService, private router: Router) {
    this.gstForm = this.buildForm();
    console.log(this.loadUserId());
    this.loadUserId();
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.rateId = id;
      this.isUpdate = true;
      this.loadGstData(id)
    } else {
      this.isUpdate = false;
    }

  }

  saveGst() {
    const gst: Gst = this.gstForm.value as Gst;
    if (this.gstForm.invalid) {
      this.toast.error('Form is Empty')
      return
    };
    const dialogRef = this.dialog.open(gstDialog, {
      width: '400px',
      data: gst.gstRate,
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`dialog result : ${result}`);
      if (result !== 'confirm') return;
      const newGst = { ...gst }
      console.log(newGst)
      if (this.isUpdate) {
        newGst.rateId = this.rateId
        this.updateGstData(newGst);
      } else {
        this.createGst(newGst)
      }
    }
    )
  }

  cancel() {
    this.router.navigate(['/layout/gst']);
  }

  back() {
    this.location.back();
  }
}


@Component({
  selector: 'gst-dialog',
  templateUrl: './dialogs/gst-dialog.html',
  standalone: true,
  imports: [NgUIModule],
changeDetection: ChangeDetectionStrategy.OnPush,
})
export class gstDialog {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<gstDialog>
  ) {
    console.log(this.data);
  }

  confirm() {
    this.dialogRef.close('confirm');
  }

  close() {
    this.dialogRef.close('cancel');
  }
}
