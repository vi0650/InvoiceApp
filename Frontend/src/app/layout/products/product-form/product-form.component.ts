import { ChangeDetectionStrategy, Component, inject, Inject, OnInit } from '@angular/core';
import { NgUIModule } from '../../../shared/ng-ui.module';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { HotToastService } from '@ngxpert/hot-toast';
import { hotToastObserve } from '../../../core/utils/toast-observer';
import { AuthService } from '../../../core/services/auth.service';
import { Product } from '../../../core/models/product.model';
import { User } from '../../../core/models/user.model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-product-form',
  standalone: false,
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {

  readonly dialog = inject(MatDialog);
  products: Product[] = [];
  productForm: FormGroup;
  productId!: string;
  isUpdate = false;

  private buildForm(): FormGroup {
    return this.fb.group({
      productName: ['', Validators.required],
      userId: [this.loadUserId()],
      rate: ['', [Validators.required]],
      isActive: [true]
    })
  }

  private loadUserId() {
    const user = this.auth.getUserData()?.userId;
    return user;
  }

  private createProduct(product: Product) {
    this.productService.addProduct(product).pipe(
      hotToastObserve(this.toast, {
        loading: "Just a Second....",
        success: () => `Successfully Saved!`,
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          if(err.status === 409) return "Product Already Exists!";
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      // this.existingIds.push(product.productId);
      this.productForm.reset();
      this.router.navigate(['/layout/products']);
    });
  }

  private updateProductData(product: Product) {
    this.productService.updateProduct(this.productId, product).pipe(
      hotToastObserve(this.toast, {
        loading: "Updating data....",
        success: (res:any) => `${res.data.productName} data Updated successfully`,
        error: err => {
          if (err.status === 0) return "Server Sleeping!";
          if (err.status === 401) return "Invalid Data!";
          return "Something Crashed!";
        }
      }),
    ).subscribe(() => {
      console.log(product.productId, product);
      this.router.navigate(['/layout/products'])
    })
  }

  private loadProductData(id: string) {
    this.productService.getProductById(id).subscribe({
      next: (res: any) => {
        const product = res?.data || res;
        console.log(product);
        if (!product) return;
        this.productForm.patchValue(product);
      }
    })
  }

  private fixDate(date: any) {
    const d = new Date(date);
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
    return d.toISOString().split('T')[0];
  }

  constructor(private route: ActivatedRoute, private location:Location, private auth: AuthService, private fb: FormBuilder, private toast: HotToastService, private productService: ProductService, private router: Router) {
    this.productForm = this.buildForm();
    console.log(this.loadUserId());
    this.loadUserId();
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productId = id;
      this.isUpdate = true;
      this.loadProductData(id)
    } else {
      this.isUpdate = false;
    }

  }

  saveProduct() {
    const product: Product = this.productForm.value as Product;
    if (this.productForm.invalid) {
      this.toast.error('Form is Empty')
      return
    };
    const dialogRef = this.dialog.open(productDialog, {
      width: '400px',
      data: product.productName,
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`dialog result : ${result}`);
      if (result !== 'confirm') return;
      const newProduct = { ...product }
      console.log(newProduct)
      if (this.isUpdate) {
        newProduct.productId = this.productId
        this.updateProductData(newProduct);
      } else {
        this.createProduct(newProduct)
      }
    }
    )
  }

  cancel() {
    this.router.navigate(['/layout/products']);
  }

  back() {
    this.location.back();
  }
}


@Component({
  selector: 'product-dialog',
  templateUrl: './dialogs/product-dialog.html',
  standalone: true,
  imports: [NgUIModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class productDialog {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<productDialog>
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
