import { Component } from '@angular/core';
import { IAdvertisement } from '../models/advertisement.model';
import { AdvertisementService } from './advertisement.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'aap-advertisement-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './advertisement-list.component.html',
  styleUrl: './advertisement-list.component.css'
})
export class AdvertisementListComponent {
  advertisements: any

  constructor(
    private adSvc: AdvertisementService
  ){}

  ngOnInit(){
    this.adSvc.getAdvertisementsOfAdoptablePets().subscribe((ads) => {
      this.advertisements = ads;
    })
  }
}
