import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IAdvertisement } from '../models/advertisement.model';
import { CommonModule } from '@angular/common';
import { IUser } from '../models/user.model';
import { UserService } from '../user/user.service';

@Component({
  selector: 'aap-advertisement-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './advertisement-details.component.html',
  styleUrl: './advertisement-details.component.css'
})
export class AdvertisementDetailsComponent {
  @Input() ad!: IAdvertisement;
  @Output() adopt = new EventEmitter();

  private user:IUser | null = null;

  constructor(private userSvc: UserService){
    this.userSvc.getUser().subscribe({
    next: (user) => { this.user = user}
  })}

  adoptButtonClicked(ad: IAdvertisement){
    if(!this.hasApplication(ad))
      this.adopt.emit();
  }

  hasApplication(ad: IAdvertisement){
    if(!this.user)
      return false;
    else {
      let app = ad.applications.filter(app => app.userId === this.user?.id);
    return app.length > 0;
    }
  }
}
