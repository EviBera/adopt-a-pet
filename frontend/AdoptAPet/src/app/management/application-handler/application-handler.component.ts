import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IAdvertisement } from '../../models/advertisement.model';
import { ManagementService } from '../management.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'aap-application-handler',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './application-handler.component.html',
  styleUrl: './application-handler.component.css'
})
export class ApplicationHandlerComponent {
  advertisementId: string | null = '';
  advertisement: IAdvertisement | null = null;

  constructor(private route: ActivatedRoute, private managementSvc: ManagementService) {}

  ngOnInit() {
    this.advertisementId = this.route.snapshot.paramMap.get('advertisementId');
    console.log("param: " + this.advertisementId);

    if(this.advertisementId !== null){
      this.fetchAd(this.advertisementId);
    }
  }

  fetchAd(advertisementId: string){
    this.managementSvc.getAdById(advertisementId).subscribe((advertisement) => {
      this.advertisement = advertisement;
    })
  }
}
