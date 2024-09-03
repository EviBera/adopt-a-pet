import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { IAdvertisement } from '../../models/advertisement.model';
import { ManagementService } from '../management.service';

@Component({
  selector: 'aap-ad-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ad-list.component.html',
  styleUrl: './ad-list.component.css'
})
export class AdListComponent {
advetisements: IAdvertisement[] = [];

constructor(private managementSvc: ManagementService){}

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
  
}
