import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  inject,
} from '@angular/core';
import { NgUIModule } from '../../../shared/ng-ui.module';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { Location } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HotToastService } from '@ngxpert/hot-toast';
import { hotToastObserve } from '../../../core/utils/toast-observer';
import { ActivatedRoute, Router } from '@angular/router';
import { Invoice } from '../../../core/models/invoice.model';
import { InvoiceService } from '../../../core/services/invoice.service';
import { ProductService } from '../../../core/services/product.service';
import { CustomerService } from '../../../core/services/customer.service';
import { AuthService } from '../../../core/services/auth.service';
import { GstService } from '../../../core/services/gst.service';

@Component({
  selector: 'app-invoice-form',
  standalone: false,
  templateUrl: './invoice-form.component.html',
  styleUrl: './invoice-form.component.scss',
})
export class InvoiceFormComponent {
  readonly path = '/layout/invoices';
  readonly dialog = inject(MatDialog);
  private invoice_Service = inject(InvoiceService);
  machine_status: Invoice[] = [];
  existingIds: number[] = [];
  invoiceForm!: FormGroup;
  invoiceId!: string;
  isUpdate = false;
  mId: number[] = [];
  cId: number[] = [];
  customers: any[] = [];
  products: any[] = [];
  gstRates: any[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private invoiceService: InvoiceService,
    private productService: ProductService,
    private customerService: CustomerService,
    private auth: AuthService,
    private toast: HotToastService,
    private location: Location,
    private GstService: GstService
  ) { }

  ngOnInit(): void {
    this.buildForm();
    this.loadCustomers();
    this.loadProduct();
    this.loadGst();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isUpdate = true;
      this.invoiceId = id;
      this.loadInvoice(id);
    }
  }

  private buildForm() {
    this.invoiceForm = this.fb.group({
      invoiceId: [this.invoiceId],
      userId: [this.auth.getUserData()?.userId],
      customerId: ['', Validators.required],
      customerName: [''],
      email: [''],
      mobileNo: [''],
      address: [''],
      invoiceDate: [new Date()],
      invoiceItems: this.fb.array([this.invoiceItem()]),
      description: [''],
      invoiceStatus: [],
      subTotal: [0],
      gstAmount: [0],
      grandTotal: [0],
    });
  }

  get invoiceItems(): FormArray {
    return this.invoiceForm.get('invoiceItems') as FormArray;
  }

  invoiceItem(): FormGroup {
    return this.fb.group({
      productId: ['', Validators.required],
      productName: [''],
      rate: [0,Validators.required],
      qty: [1,Validators.min(1)],
      rateId:['',Validators.required],
      gst: [0],
      amount: [0],
    });
  }

  private loadCustomers() {
    this.customerService.getAllCustomers().subscribe((res: any) => {
      if(res.value.data === null){
        this.toast.info('please add customers first')
      }
      this.customers = res.value.data || [];
      console.log(res.value.data);
      
      // const customerId = this.invoiceForm.get('customerId')?.value;
      // if (customerId) {
      //   this.invoiceForm.get('customerId')?.setValue(customerId);
      // }
    });
  }

  private loadProduct() {
    this.productService.getAllProducts().subscribe((res: any) => {
      if(res.value.data === null){
        this.toast.info('please add Products first')
      }
      this.products = res.value.data || [];
      console.log(res.value.data);
    });
  }

  private loadGst() {
    this.GstService.getAllGst().subscribe((res: any) => {
      if(res.value.data === null){
        this.toast.info('please add GST first')
      }
      this.gstRates = res.value.data || [];
      console.log(res.value.data);
    });
  }

  private loadInvoice(id: string) {
    this.invoice_Service.getInvoiceById(id).subscribe((res: any) => {
      this.patchInvoice(res.data);
    });
  }

  // private loadIds() {
  //   this.invoice_Service.getIds().subscribe(res =>{
  //     this.mId = res.mid;
  //     this.cId = res.cid
  //   })
  // }

  private patchInvoice(invoice: any) {
    this.invoiceForm.patchValue({
      invoiceId: invoice.invoiceId,
      userId: this.auth.getUserData()?.userId,
      customerId: invoice.customer.customerId,
      customerName: invoice.customer.customerName,
      email: invoice.customer.email,
      mobileNo: invoice.customer.mobileNo,
      address: invoice.customer.address,
      invoiceDate: invoice.invoiceDate,
      invoiceStatus: invoice.invoiceStatus,
      description: invoice.description,
      subTotal: invoice.subTotal,
      gstAmount: invoice.gstAmount,
      grandTotal: invoice.grandTotal
    });
    this.invoiceItems.clear();
    invoice.invoiceItems.forEach((item: any) => {
      this.invoiceItems.push(this.fb.group({
        productId: item.productId,
        productName: item.productName,
        rate: item.rate,
        qty: item.qty,
        rateId: item.rateId,
        gst: item.gst,
        amount: item.amount,
      }));
      // console.log(item);
    });
    // console.log(invoice.invoiceId);
    // console.log(invoice.customer.address);
    this.grandTotal();
  }

  onCustomerChange(customerId: string) {
    const c = this.customers.find((x) => x.customerId === customerId);
    if (!c) return;

    this.invoiceForm.patchValue({
      customerName: c.customerName,
      email: c.email,
      mobileNo: c.mobileNo,
      address: c.address,
    });
  }

  onProductChange(index: number) {
    const item = this.invoiceItems.at(index);
    const product = this.products.find(
      (p) => p.productId === item.get('productId')?.value,
    );
    if (!product) return;
    item.patchValue({
      productName: product.productName,
      rate: product.rate,
    });
    this.calculateItemAmount(index);
  }

  onGstChange(index: number) {
    const item = this.invoiceItems.at(index);
    const gst = this.gstRates.find(
      (p) => p.rateId === item.get('rateId')?.value,
    );
    if (!gst) return;
    item.patchValue({
      gst: gst.gst,
    });
    this.calculateItemAmount(index);
  }

  calculateItemAmount(index: number) {
    const item = this.invoiceItems.at(index);
    const rate = Number(String(item.get('rate')?.value) || 0);
    const qty = Number(String(item.get('qty')?.value) || 0);
    const gst = Number(item.get('gst')?.value) || 0;
    console.log(gst);
    
    const taxableAmount = rate * qty;
    const gstAmount = taxableAmount * (gst / 100);
    const finalAmount = gstAmount + taxableAmount;

    item.patchValue({ amount: Number(String(finalAmount.toFixed(2))) });
    this.grandTotal();
  }

  grandTotal() {
    let totalTaxableAmount = 0;
    let totalAmount = 0;
    let gstTotal = 0;

    for (const item of this.invoiceItems.controls) {
      const itemTotal = Number(item.get('amount')?.value || 0);
      const gst = Number(item.get('gst')?.value || 0);

      const taxable = itemTotal / (1 + gst / 100);
      const gstValue = itemTotal - taxable;

      totalTaxableAmount += taxable;
      gstTotal += gstValue;
      totalAmount += itemTotal;
    }

    this.invoiceForm.patchValue({
      subTotal: Number(totalTaxableAmount.toFixed(2)),
      gstAmount: Number(gstTotal.toFixed(2)),
      grandTotal: Number(totalAmount.toFixed(2)),
    });
  }

  addItem() {
    this.invoiceItems.push(this.invoiceItem());
  }

  removeItem(i: number) {
    this.invoiceItems.removeAt(i);
    this.grandTotal();
  }

  private createInvoice(invoice: Invoice) {
    console.log(invoice);
    
    this.invoice_Service
      .addInvoice(invoice)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Just a Second....',
          success: () => 'Successfully Saved!',
          error: (err) => {
            if (err.status === 0) return 'Server offline!';
            if (err.status === 401) return 'Invalid Data!';
            return 'Something Crashed!';
          },
        }),
      ).subscribe(() => {
        this.invoiceForm.reset();
        this.router.navigate([this.path]);
      });
  }

  private editInvoice(invoice: Invoice) {
    console.log(invoice);
    
    this.invoice_Service
      .updateInvoice(invoice.invoiceId, invoice)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Updating data....',
          success: () => `data Updated successfully`,
          error: (err) => {
            if (err.status === 0) return 'Server Sleeping!';
            if (err.status === 401) return 'Invalid Data!';
            return 'Something Crashed!';
          },
        }),
      )
      .subscribe(() => {
        // console.log(invoice.invoiceId, invoice);
        this.router.navigate([this.path]);
      });
  }

  saveInvoice() {
    if (!this.invoiceForm.valid) {
      this.toast.error('fill the all fields');
      return;
    }
    const invoice: Invoice = this.invoiceForm.value as Invoice;
    const dialogRef = this.dialog.open(invoiceDialog, {
      width: '400px',
      data: invoice.invoiceId,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result !== 'confirm') return;
      const newInvoice = { ...invoice };
      if (this.isUpdate) {
        // newInvoice.invoiceId = invoice.invoiceId;
        this.editInvoice(newInvoice);
      } else {
        this.createInvoice(newInvoice);
      }
    });
  }

  cancel() {
    this.router.navigate([this.path]);
  }

  back() {
    this.location.back();
  }

}

@Component({
  selector: 'invoice-dialog',
  templateUrl: './dialogs/invoice-dialog.html',
  imports: [NgUIModule],
standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class invoiceDialog {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<invoiceDialog>,
  ) {
    // console.log(this.data);
  }

  confirm() {
    this.dialogRef.close('confirm');
  }

  close() {
    this.dialogRef.close('cancel');
  }
}
