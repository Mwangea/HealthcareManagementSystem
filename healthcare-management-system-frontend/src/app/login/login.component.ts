import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  username: string = '';
  password: string = '';
  role: 'admin' | 'doctor' = 'doctor';

  constructor(private authService: AuthService, private router: Router) { }

  login(role: 'admin' | 'doctor'): void {
    this.authService.login(this.username, this.password, this.role);
  }

}
