import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { IAdvertisement } from '../../models/advertisement.model';
import { ManagementService } from '../management.service';
import { Router } from '@angular/router';

@Component({
  selector: 'aap-ad-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ad-list.component.html',
  styleUrl: './ad-list.component.css'
})
export class AdListComponent {
advetisements: IAdvertisement[] = [];

constructor(private managementSvc: ManagementService, private router: Router){}

  ngOnInit(){
    this.fetchAds();
  }

  fetchAds(){
    this.managementSvc.getAds().subscribe((advetisements) => {
      this.advetisements = advetisements;
    })
  }

  get adList(): IAdvertisement[] {
    return this.advetisements;
  }

  hasExpired(advertisement: IAdvertisement) {
    const currentDate = new Date();
    const adExpiration = new Date(advertisement.expiresAt);
    return adExpiration < currentDate;
  }

  handleApplications(advertisement: IAdvertisement){
    console.log("Handle application of ad " + advertisement.id);
    this.router.navigate(['/applications']);
  }

  createApplications(advertisement: IAdvertisement){
    console.log("Create new ad " + advertisement.id);
  }
  
}
