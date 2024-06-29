import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '../../material.module';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../_service/user.service';
import { ToastrService } from 'ngx-toastr';
import { LoginForm } from '../../_model/doctor.model';
//import { Router } from 'express';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MaterialModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  constructor(private builder: FormBuilder, private service: UserService, private toastr: ToastrService, private router: Router) {

  }

  _loginform = this.builder.group({
    username: this.builder.control('', Validators.required),
    password: this.builder.control('', Validators.required),
    role: ['admin', Validators.required]
  })

  proceedlogin(): void {

    if (this._loginform.valid) {
      let _login: LoginForm = {
        Username: this._loginform.value.username as string,
        Password: this._loginform.value.password as string,
        role: this._loginform.value.role as 'admin' | 'doctor',

      };
      this.service.proceedlogin(_login).subscribe(
        (response: any) => {
          this.handleLoginSuccess(response.token);
        },
        (error: any) => {
          this.handleLoginError(error);
        }
      );
    }
  }
  handleLoginSuccess(token: string): void {
    const decodedToken = this.service.decodeToken(token);
    console.log('Decoded token:', decodedToken);

    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    console.log('Role extracted from token:', role);

    localStorage.setItem('token', token);
    console.log('Token stored in local storage.');

    this.toastr.success(`Logged in as ${role}`);
    if (role === 'Admin') {
      this.router.navigateByUrl('/');
    } else if (role === 'Doctor') {
      this.router.navigateByUrl('/');
    } else {
      this.toastr.error('Invalid role.');
    }
  }

  handleLoginError(error: any): void {
    if (error.status === 500 && error.error.message === 'Invalid credentials') {
      this.toastr.error('You are not authorized.');
    } else {
      this.toastr.error('Login failed. Please try again.');
    }
  }

}

 
