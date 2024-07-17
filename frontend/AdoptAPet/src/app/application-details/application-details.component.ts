import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IApplication } from '../models/application.model';

@Component({
  selector: 'aap-application-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './application-details.component.html',
  styleUrl: './application-details.component.css'
})
export class ApplicationDetailsComponent {
  @Input() app!: IApplication;
  @Output() withdraw = new EventEmitter();

  withdrawButtonClicked(app: IApplication){
    this.withdraw.emit();
  }
}
