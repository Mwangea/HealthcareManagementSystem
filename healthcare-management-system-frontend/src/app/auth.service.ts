import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5032/api';
  private currentUser: any;

  constructor(private http: HttpClient, private router: Router) { }

  login(username: string, password: string, role: 'admin' | 'doctor'): void {
    const loginUrl = role === 'admin' ? `${this.apiUrl}/admin/login` : `${this.apiUrl}/doctor/login`;
    this.http.post<any>(loginUrl, { username, password }).subscribe(
      (response) => {
        this.currentUser = {
          username: response.usernamem,
          role: role
        };
        localStorage.setItem('currentUser', JSON.stringify(this.currentUser));
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        console.error('Login error:', error)
      }
    );
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.router.navigate(['/login'])
  }

  getCurrentUser(): any {
    const currentUser = localStorage.getItem('currentUser');
    return currentUser ? JSON.parse(currentUser) : null;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('currentUser');
  }

  isAdmin(): boolean {
    return this.getCurrentUser()?.role === 'admin';
  }

  isDoctor(): boolean{
    return this.getCurrentUser()?.role === 'doctor';
  }
}
