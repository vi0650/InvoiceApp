import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { NgUIModule } from '../../../shared/ng-ui.module';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CITIES } from '../../../core/data/city';
import { STATES } from '../../../core/data/state';
import { HotToastService } from '@ngxpert/hot-toast';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../../core/services/customer.service';
import { hotToastObserve } from '../../../core/utils/toast-observer';
import { Customer } from '../../../core/models/customer.model';
import { AuthService } from '../../../core/services/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-customer-form',
  standalone: false,
  templateUrl: './customer-form.component.html',
  styleUrl: './customer-form.component.scss'
})
export class CustomerFormComponent {

  readonly path = '/layout/customers'
  readonly dialog = inject(MatDialog);
  cities = CITIES;
  states = STATES;
  customer: Customer[] = [];
  existingIds: string[] = [];
  customerForm: FormGroup;
  customerId!: string;
  isUpdate = false;

  private buildForm(): FormGroup {
    return this.fb.group({
      customerName: ['', Validators.required],
      userId: [this.loadUserId()],
      email: ['', [Validators.required, Validators.email]],
      mobileNo: ['', [Validators.required, Validators.pattern("^[0-9]{10}$")]],
      address: [''],
      isActive: [true]
    });
  }

  private loadUserId() {
    const user = this.auth.getUserData()?.userId;
    return user;
  }

  private createCustomer(customer: Customer) {
    this.CustomerService.addCustomer(customer).pipe(
      hotToastObserve(this.toast, {
        loading: "Just a Second....",
        success: () => `Successfully Saved!`,
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      // this.existingIds.push(customer.customerId);
      this.customerForm.reset();
      this.router.navigate([this.path])
    })
  }

  private updateCustomer(customer: Customer) {
    this.CustomerService.updateCustomer(this.customerId, customer).pipe(
      hotToastObserve(this.toast, {
        loading: "Updating data....",
        success: (res: any) => `${res.data.customerName} data Updated successfully`,
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      this.router.navigate([this.path]);
    })
  }

  private loadCustomerData(id: string) {
    this.CustomerService.getCustomerById(id).subscribe({
      next: (res: any) => {
        const customer = res.data || res;
        console.log(customer);
        if (!customer) {
          return;
        }
        this.customerForm.patchValue(customer);
      }
    })
  }

  private fixDate(date: any) {
    const d = new Date(date);
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
    return d.toISOString().split('T')[0];
  }

  constructor(private CustomerService: CustomerService, private location: Location, private auth: AuthService, private route: ActivatedRoute, private fb: FormBuilder, private toast: HotToastService, private router: Router) {
    this.customerForm = this.buildForm()
    this.loadUserId();
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.customerId = id;
      this.isUpdate = true;
      this.loadCustomerData(id);
    } else {
      this.isUpdate = false;
    }
  }

  saveCustomer() {
    const customer: Customer = this.customerForm.value as Customer;

    if (!this.customerForm.valid) {
      this.toast.error('fill the all fields')
      return
    };
    const dialogRef = this.dialog.open(customerDialog, {
      width: '400px',
      data: customer.customerName,
    });
    dialogRef.afterClosed().subscribe((result => {
      if (result !== 'confirm') return;
      const newCustomer = { ...customer }
      if (this.isUpdate) {
        newCustomer.customerId = this.customerId;
        this.updateCustomer(newCustomer);
      } else {
        this.createCustomer(newCustomer);
      }
    }))
  }

  cancel() {
    this.router.navigate([this.path]);
  }

  back() {
    this.location.back();
  }
}


@Component({
  selector: 'customer-dialog',
  templateUrl: './dialogs/customer-dialog.html',
  imports: [NgUIModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class customerDialog {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<customerDialog>
  ) {
    console.log(this.data);
  }

  user: Customer[] = []

  confirm() {
    this.dialogRef.close('confirm');
  }

  close() {
    this.dialogRef.close('cancel');
  }
}