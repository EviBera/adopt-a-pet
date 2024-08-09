import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IUserCredentials } from '../../models/user.model';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'aap-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials: IUserCredentials = { email: '', password: '', rememberMe: false };
  loginError: boolean = false;

  constructor(private userSvc: UserService, private router: Router){}

  login(form: NgForm){  
    //login, then navigate to advertisements
    this.loginError = false;
    this.userSvc.login(this.credentials).subscribe({
      next: () => this.router.navigate(['/adopt']),
      error: () => (this.loginError = true)
    });
  }
}
