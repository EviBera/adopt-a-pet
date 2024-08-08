import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { IUserRegistrationCredentials } from '../../models/user.model';
import { UserService } from '../user.service';

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
  message: string | null = null;
  firstPasswordFieldType: string = "password";
  secondPasswordFieldType: string = "password";

  constructor(private router: Router, private userSvc: UserService){}

  register(form: NgForm){
    console.log("register button clicked");
    console.log(form.value);

    if(this.identicalPasswords){
      this.registrationCredentials.password = this.passwords.firstPassword;

      this.registrationError = false;

      this.userSvc.register(this.registrationCredentials).subscribe({
        next: () => {
          this.message = "Successful registration.";
          setTimeout(() => this.router.navigate(['/login']), 2500);
        },
        error: err => {
          this.message = `Registration failed: ${err}`
          this.registrationError = true;
        }
      })
    }
  }

  cancel(){
    console.log("cancel button clicked");
    this.router.navigate(["/home"]);
  }

  checkPasswords(){
    this.identicalPasswords = this.passwords.firstPassword === this.passwords.secondPassword;
    console.log("Passwords are idetical? " + this.identicalPasswords);
  }

  changeVisibility(field: string){
    switch(field){
      case 'first':
        this.firstPasswordFieldType === "password" ? this.firstPasswordFieldType = "text" : this.firstPasswordFieldType = "password";
        break;
      case 'second':
        this.secondPasswordFieldType === "password" ? this.secondPasswordFieldType = "text" : this.secondPasswordFieldType = "password";
        break;
      default:
        return;
    }
  }
}
 