import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'aap-logout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {
  message: string | null = null;

  constructor(private router: Router, private userSvc: UserService) { }

  logout() {
    console.log("logout button clicked");
    this.userSvc.logout().subscribe({
      next: () => {
        this.message = 'You have logged out successfully.';
        setTimeout(() => {
          this.router.navigate(["/home"]);
        }, 2000);
      },
      error: err => {
        this.message = `Logout failed: ${err}`;
      }
    });
  }

  cancel() {
    console.log("cancel button clicked");
    this.router.navigate(["/adopt"]);
  }
}
