import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { IUserRegistrationCredentials } from '../../models/user.model';

@Component({
  selector: 'aap-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  registrationCredentials: IUserRegistrationCredentials = { firstName: '', lastname: '', email: '', password: ''};
  passwords = {
    firstPassword: '',
    secondPassword: ''
  }
  identicalPasswords: boolean = true;
  
  registrationError: boolean = false;

  constructor(private router: Router){}

  register(form: NgForm){
    console.log("register button clicked");
    console.log(form.value);
  }

  cancel(){
    console.log("cancel button clicked");
    this.router.navigate(["/home"]);
  }

  checkPasswords(){
    this.identicalPasswords = this.passwords.firstPassword === this.passwords.secondPassword;
    console.log("Passwords are idetical? " + this.identicalPasswords);
  }
}
 