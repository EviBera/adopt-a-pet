import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IUserCredentials } from '../user.model';

@Component({
  selector: 'aap-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials: IUserCredentials = { email: '', password: '' };
  loginError: boolean = false;

  login(form: NgForm){
    console.log("Login button cliked.");
    console.log(form.value);
  }
}
