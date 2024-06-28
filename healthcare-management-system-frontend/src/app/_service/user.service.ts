import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { doctorRegister } from '../_model/doctor.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  baseUrl = environment.apiUrl;

  Doctorregistration(_data: doctorRegister) {
    return this.http.post(`${this.baseUrl}doctor/register`, _data);
  }
}
