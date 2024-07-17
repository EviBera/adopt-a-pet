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
    this.adopt.emit();
  }
}
