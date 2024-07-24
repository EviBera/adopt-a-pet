import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IAdvertisement } from '../models/advertisement.model';
import { CommonModule } from '@angular/common';

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

  adoptButtonClicked(ad: IAdvertisement){
    if(!this.hasApplication(ad))
      this.adopt.emit();
  }

  hasApplication(ad: IAdvertisement){
    let app = ad.applications.filter(app => app.userId === '711704b1-035f-4f76-99d9-a1dece06c153');
    return app.length > 0;
  }
}
