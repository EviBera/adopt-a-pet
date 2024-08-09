import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { IUser } from '../models/user.model';
import { UserService } from '../user/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'aap-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  user: IUser | null = null;

  constructor(private userSvc: UserService){}

  ngOnInit(): void {
    this.userSvc.getUser().subscribe({
      next: (user) => { this.user = user}
    })
  }


}
