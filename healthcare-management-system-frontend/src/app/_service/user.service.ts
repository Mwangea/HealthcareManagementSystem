import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginForm, doctorRegister } from '../_model/doctor.model';
import { environment } from '../../environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  baseUrl = environment.apiUrl;

  Doctorregistration(_data: doctorRegister) {
    return this.http.post(`${this.baseUrl}doctor/register`, _data);
  }

  proceedlogin(_data: LoginForm) {
    let loginEndpoint: string = '';

    if (_data.role === 'admin') {
      loginEndpoint = `${this.baseUrl}admin/login`;
    } else if (_data.role === 'doctor') {
      loginEndpoint = `${this.baseUrl}doctor/login`;
    } else {
      // Handle other roles or throw an error if needed
      throw new Error(`Role '${_data.role}' is not supported.`);
    }

    return this.http.post(loginEndpoint, _data);
  }

  decodeToken(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }
}
