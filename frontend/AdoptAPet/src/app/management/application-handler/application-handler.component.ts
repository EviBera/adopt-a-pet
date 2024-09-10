import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IAdvertisement } from '../../models/advertisement.model';
import { ManagementService } from '../management.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { IApplication } from '../../models/application.model';

@Component({
  selector: 'aap-application-handler',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './application-handler.component.html',
  styleUrl: './application-handler.component.css'
})
export class ApplicationHandlerComponent {
  advertisementId: string | null = '';
  advertisement: IAdvertisement | null = null;
  applicationId!: number;
  selectedUserId: string | null = '';
  usersMap: { [key: string]: string } = {}; // Map userId to userName

  constructor(private route: ActivatedRoute, private managementSvc: ManagementService) {}

  ngOnInit() {
    this.advertisementId = this.route.snapshot.paramMap.get('advertisementId');

    if(this.advertisementId !== null){
      this.fetchAd(this.advertisementId);
    }
  }

  fetchAd(advertisementId: string){
    this.managementSvc.getAdById(advertisementId).subscribe((advertisement) => {
      this.advertisement = advertisement;
      // For each application, fetch the user name
      this.advertisement?.applications.forEach(application => {
        this.fetchUserName(application.userId);
      });
    });
  }

  fetchUserName(userId: string) {
    this.managementSvc.getUserById(userId).subscribe((applicant) => {
      this.usersMap[userId] = applicant.firstName + " " + applicant.lastName; // Store the user name in the map
    });
  }

  changeSelectedoption(application: IApplication){
    this.applicationId = application.id;
  }

  selectOwner(form: NgForm){
    console.log(this.applicationId);
  }
}
