import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AdListComponent } from '../ad-list/ad-list.component';
import { PetListComponent } from '../pet-list/pet-list.component';

@Component({
  selector: 'aap-advertisement-handler',
  standalone: true,
  imports: [CommonModule, AdListComponent, PetListComponent],
  templateUrl: './advertisement-handler.component.html',
  styleUrl: './advertisement-handler.component.css'
})
export class AdvertisementHandlerComponent {
  showPets: boolean = false;
  showAds: boolean = false;

  constructor(){}

  setPetVisibility(){
    this.showPets = !this.showPets;
  }

  setAdvertisementVisibiliy(){
    this.showAds = !this.showAds;
  }

}
