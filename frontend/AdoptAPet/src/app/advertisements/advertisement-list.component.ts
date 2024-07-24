import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AdvertisementDetailsComponent } from '../advertisement-details/advertisement-details.component';
import { AdvertisementService } from './advertisement.service';
import { AppService } from '../applications/app.service';
import { IAdvertisement } from '../models/advertisement.model';
import { speciesValues } from '../models/pet.model';

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

  constructor(
    private adSvc: AdvertisementService,
    private appSvc: AppService,
    private router: Router
  ){}

  ngOnInit(){
    this.adSvc.getAdvertisementsOfAdoptablePets().subscribe((ads) => {
      this.advertisements = ads;
    })
  }

  handInApplication(ad: IAdvertisement){
    console.log("I'd like to adopt button is clicked, ad: " + ad.id)
    this.appSvc.handIn(ad);
    alert("Your wish for adoption has been registered.");
    this.router.navigate(['/applications']);
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
