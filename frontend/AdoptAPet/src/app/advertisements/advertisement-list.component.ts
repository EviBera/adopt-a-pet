import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AdvertisementDetailsComponent } from '../advertisement-details/advertisement-details.component';
import { AdvertisementService } from './advertisement.service';
import { AppService } from '../applications/app.service';
import { IAdvertisement } from '../models/advertisement.model';
import { speciesValues } from '../models/pet.model';
import { UserService } from '../user/user.service';
import { IUser } from '../models/user.model';

@Component({
  selector: 'aap-advertisement-list',
  standalone: true,
  imports: [CommonModule, AdvertisementDetailsComponent, FormsModule],
  templateUrl: './advertisement-list.component.html',
  styleUrl: './advertisement-list.component.css'
})
export class AdvertisementListComponent {
  private advertisements: IAdvertisement[] = [];
  speciesValues = speciesValues;
  filter: string = '';
  private user: IUser | null = null;

  constructor(
    private adSvc: AdvertisementService,
    private appSvc: AppService,
    private router: Router,
    private userSvc: UserService
  ){}

  ngOnInit(){
    this.adSvc.getAdvertisementsOfAdoptablePets().subscribe((ads) => {
      this.advertisements = ads;
    });
    this.userSvc.getUser().subscribe({
      next: (user) => { this.user = user}
    });
  }

  handInApplication(ad: IAdvertisement){
    console.log("I'd like to adopt button is clicked, ad: " + ad.id)
    if(this.user){
      this.appSvc.handIn(ad);
          alert("Your wish for adoption has been registered.");
          this.router.navigate(['/applications']);
    } 
    else {
      alert("You have to login if you wish to apply for a pet.")
    }
    
  }

  get advertisementList(){
    switch(this.filter){
      case 'dogs': 
        return this.advertisements.filter(a => a.petDto.species.toLowerCase() === 'dog');
      case 'cats':
        return this.advertisements.filter(a => a.petDto.species.toLowerCase() === 'cat');
      case 'rabbits':
        return this.advertisements.filter(a => a.petDto.species.toLowerCase() === 'rabbit');
      case 'hamsters':
        return this.advertisements.filter(a => a.petDto.species.toLowerCase() === 'hamster');
      case 'elephants':
        return this.advertisements.filter(a => a.petDto.species.toLowerCase() === 'elephant');
      default:
        return this.advertisements;
    }
  }
}
