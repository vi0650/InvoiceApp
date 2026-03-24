import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HotToastService } from '@ngxpert/hot-toast';
import { hotToastObserve } from '../core/utils/toast-observer';
import { Role } from '../core/models/user.model';
import { AuthService } from '../core/services/auth.service';
import { login } from '../core/models/login.model';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm: FormGroup;
  hidePassword = false;
  login: login[] = [];
  role = Object.values(Role);

  private buildForm(): FormGroup {
    return this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    })
  }

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router, private toast: HotToastService) {
    this.loginForm = this.buildForm();
    const preventUrl = auth.getUserData();
    if (preventUrl?.role === Role.Employee || preventUrl?.role === Role.User || preventUrl?.role === Role.SuperAdmin) {
      router.navigate(['/layout'], { replaceUrl: true });
    } else {
      router.navigate(['/login'], { replaceUrl: true });
    }
    console.log(this.auth.getUserData()?.isActive);
  }

  showHidePassword() {
    this.hidePassword = !this.hidePassword;
  }

  loginUser() {
    const user: login = this.loginForm.value as login;
    console.log("Form Value:", user);
    this.auth.loginUser(user)
      .pipe(
        hotToastObserve(this.toast, {
          loading: "Please Wait...",
          success: "",
          error: (err) => {
            if (err.status === 0) return "Server is Offline!";
            if (err.status === 400) return `${err.error.message}`;
            if (err.status === 401) return "Invalid credentials!";
            return "Something went wrong!";
          },
        })).subscribe({
          next: (res: any) => {
            const userdata = this.auth.getUserData();
            console.log(res);
            if (res && res.success) {
              this.toast.success(res.message || 'Login successful');
              console.log('User Role is:', userdata?.role);
              if (userdata?.role === Role.Employee || userdata?.role === Role.User || userdata?.role === Role.SuperAdmin) {
                this.router.navigate(['/layout'], { replaceUrl: true });
              }
              else {
                this.toast.warning(res.message || 'unauthorized')
              }
            }
            return this.router.parseUrl('/login');
          }
        })
  }

}
