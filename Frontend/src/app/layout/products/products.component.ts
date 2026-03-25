import { Component, inject, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProductService } from '../../core/services/product.service';
import { HotToastService } from '@ngxpert/hot-toast';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { PRODUCT_COL } from '../../core/data/tabledata/productColumns';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { hotToastObserve } from '../../core/utils/toast-observer';
import { Product } from '../../core/models/product.model';
import { AuthService } from '../../core/services/auth.service';
import { Role } from '../../core/models/user.model';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent {
  readonly path = '/layout/products/products-form';
  readonly dialog = inject(MatDialog);

  constructor(
    private productService: ProductService,
    private toast: HotToastService,
    private router: Router,
    private auth: AuthService
  ) { }

  dataSource = new MatTableDataSource<Product>();
  displayColumns = PRODUCT_COL;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadProduct();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data, filter) => {
      const searchStr = filter.toLowerCase();
      const nameMatch = data.productName.toLowerCase().includes(searchStr);
      const rateMatch = data.rate.toString().includes(searchStr);
      let statusMatch = false;
      if (searchStr === 'active') {
        statusMatch = data.isActive === true;
      } else if (searchStr === 'inactive') {
        statusMatch = data.isActive === false;
      } else {
        statusMatch = data.isActive.toString().includes(searchStr);
      }
      return nameMatch || rateMatch || statusMatch;
    }
  }

  canAddProduct(): boolean {
    return this.auth.hasRole([Role.SuperAdmin, Role.User]);
  }

  loadProduct() {
    this.productService.getAllProducts().pipe(
      hotToastObserve(this.toast, {
        loading: 'Loading...',
        success: () => '',
        error: (err) => {
          if (err.status === 0) {
            return 'server is offline!';
          }
          if (err.status === 401) {
            return 'Unauthorized access';
          }
          if (err.status === 403) {
            return 'Unauthorized access';
          }
          return 'Internal server error !!!';
        },
      }),
    )
      .subscribe({
        next: (res: any) => {
          console.log(res.value.data);
          this.dataSource.data = res.value.data || [];
        },
      });
  }

  applyFilter(event: any) {
    const value = event.target.value.trim().toLowerCase();
    this.dataSource.filter = value;
  }

  editProduct(row: Product) {
    this.router.navigate([this.path, row.productId]);
  }

  deleteProduct(row: Product) {
    this.productService
      .deleteProduct(row.productId)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Deleting Product...',
          success: () => `Product ${row.productName} Deleted!`,
          error: (err) => {
            if (err.status === 0) return 'server offline!';
            if (err.status === 401) return 'Unauthorized access';
            if (err.status === 403) return 'You don’t have permission to perform this action';
            if (err.status === 409) return `${err.error.message}`;
            if (err.status === 500) return 'You have invoices of this product please delete first'
            return 'Something Crashed!';
          },
        }),
      )
      .subscribe(() => this.loadProduct());
  }

  addProduct() {
    this.router.navigate([this.path]);
  }
}
