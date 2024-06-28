import { Component } from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '../../material.module';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { doctorRegister } from '../../_model/doctor.model';
import { UserService } from '../../_service/user.service';
import { ToastrService } from 'ngx-toastr';
//import { Router } from 'express';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, MaterialModule, CommonModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  specialties: string[] = ['Cardiology', 'Dermatology', 'Neurology', 'Pediatrics', 'Radiology'];
  constructor(private builder: FormBuilder, private service: UserService, private toastr: ToastrService, private router: Router) {

  }

  _response: any;

    _regform=this.builder.group({
      Username: this.builder.control('', Validators.compose([Validators.required, Validators.minLength(5)])),
      Email: this.builder.control('', Validators.compose([Validators.required, Validators.email])),
      Password: this.builder.control('', Validators.required),
      ConfirmPassword: this.builder.control('', Validators.required),
      Specialty:this.builder.control('', Validators.required),
    })

  proceedregister() {
    if (this._regform.valid) {
      let _obj: doctorRegister = {
        Username: this._regform.value.Username as string,
        Password: this._regform.value.Password as string,
        Email: this._regform.value.Email as string,
        Specialty: this._regform.value.Specialty as string
      }
      console.log('Data being sent to backend:', _obj);

      this.service.Doctorregistration(_obj).subscribe(item => {
        this._response = item;
        console.log('Response from backend:', this._response);
        if (this._response.result === 'Ok') {
          this.toastr.success('Registration successful!', 'Success');
          this.router.navigateByUrl('/login');
        } else {
          this.toastr.error('Registration failed: ' + this._response.message, 'Error');
        }
      }
      );
    }
  }
}
