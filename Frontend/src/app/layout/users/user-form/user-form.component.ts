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
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HotToastService } from '@ngxpert/hot-toast';
import { UserService } from '../../../core/services/user.service';
import { hotToastObserve } from '../../../core/utils/toast-observer';
import { ActivatedRoute, Router } from '@angular/router';
import { Role, User } from '../../../core/models/user.model';
import { Location } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-user-form',
  standalone: false,
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.scss',
})
export class UserFormComponent {
  readonly dialog = inject(MatDialog);
  readonly path = '/layout/users';
  users: User[] = [];
  userForm: FormGroup;
  userId!: string;
  Role = Role;
  roles: string[] = Object.values(Role);
  tokenData: any;
  hidePassword = false;
  isUpdate = false;

  private buildForm(): FormGroup {
    return this.fb.group({
      // userId: ['', Validators.required],
      shopName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      mobileNo: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      password: ['', [Validators.required]],
      address: ['', [Validators.required]],
      role: ['', [Validators.required]],
      isActive: [true],
    });
  }

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private fb: FormBuilder,
    private toast: HotToastService,
    private userService: UserService,
    private router: Router,
    private auth: AuthService
  ) { this.userForm = this.buildForm(); }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userId = id;
      this.isUpdate = true;
      this.loadUserData(id);
    } else {
      this.isUpdate = false;
    }

    this.tokenData = this.auth.getUserData();
    this.userForm.patchValue({
      shopName: this.tokenData.shopName
    })
  }

  private createUser(user: User) {
    this.userService
      .addUser(user)
      .pipe(
        hotToastObserve(this.toast, {
          loading: 'Just a Second....',
          success: (res: any) => `${res.data.userName} Successfully Saved!`,
          error: (err) => {
            if (err.status === 0) return 'Server Sleeping!';
            if (err.status === 401) return 'Invalid Data!';
            return 'Something Crashed!';
          },
        }),
      )
      .subscribe(() => {
        // this.existingIds.push(user.userId);
        this.userForm.reset();
        this.router.navigate([this.path]);
      });
  }

  private updateUserData(user: User) {
    this.userService
      .updateUser(user.userId, user)
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
        console.log(user.userId, user);
        this.router.navigate([this.path]);
      });
  }

  private loadUserData(id: string) {
    this.userService.getUserById(id).subscribe({
      next: (res: any) => {
        const user = res?.data || res;
        console.log(user);
        if (!user) return;
        this.userForm.patchValue(user);
        // this.userForm.get('password')?.disable();
      },
    });
  }

  showHidePassword() {
    this.hidePassword = !this.hidePassword;
  }

  saveUser() {
    const user: User = this.userForm.value as User;
    if (this.userForm.invalid) {
      this.toast.error('Form is Empty');
      return;
    }
    const dialogRef = this.dialog.open(userDialog, {
      width: '400px',
      data: user.userName,
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log(`dialog result : ${result}`);
      if (result !== 'confirm') return;
      const newUser = { ...user };
      console.log(newUser);
      if (this.isUpdate) {
        newUser.userId = this.userId;
        this.updateUserData(newUser);
      } else {
        this.createUser(newUser);
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
  selector: 'user-dialog',
  templateUrl: './dialogs/user-dialog.html',
  standalone: true,
  imports: [NgUIModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class userDialog {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<userDialog>,
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
