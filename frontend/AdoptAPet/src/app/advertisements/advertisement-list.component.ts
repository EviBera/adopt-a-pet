import { Component } from '@angular/core';
import { IAdvertisement } from '../models/advertisement.model';
import { AdvertisementService } from './advertisement.service';
import { CommonModule } from '@angular/common';
import { AdvertisementDetailsComponent } from '../advertisement-details/advertisement-details.component';
import { AppService } from '../applications/app.service';
import { Router } from '@angular/router';

@Component({
  selector: 'aap-advertisement-list',
  standalone: true,
  imports: [CommonModule, AdvertisementDetailsComponent],
  templateUrl: './advertisement-list.component.html',
  styleUrl: './advertisement-list.component.css'
})
export class AdvertisementListComponent {
  advertisements: any

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
}
