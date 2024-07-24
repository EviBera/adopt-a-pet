import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IApplication, filterValues } from '../models/application.model';
import { AppService } from './app.service';
import { ApplicationDetailsComponent } from '../application-details/application-details.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'aap-applications',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ApplicationDetailsComponent
  ],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit{
  private applications: IApplication[] = [];
  filterValues = filterValues;
  filter: string = filterValues[0].value;

  constructor(private appSvc: AppService){}

  ngOnInit(): void {
    this.appSvc.getApplications().subscribe({
      next: (applications) => (this.applications = this.sortApplications(applications))
    })
  }

  sortApplications(applications: IApplication[]): IApplication[] {
    return applications.sort((a, b) => {
        if (a.isAccepted === null && b.isAccepted !== null) return -1;
        if (a.isAccepted !== null && b.isAccepted === null) return 1;

        return b.id - a.id;
    });
  }

  get applicationList(){
    switch(this.filter){
      case 'pending':
        return this.applications.filter(a => a.isAccepted === null);
      case 'accepted':
        return this.applications.filter(a => a.isAccepted === true);
      case 'refused':
        return this.applications.filter(a => a.isAccepted === false);
      default:
        return this.applications;
    }
  }

  withdrawApplication( app: IApplication){
    if(app.isAccepted === null){
      this.appSvc.withdraw(app.id);
    }
    else {
      alert("You can not withdraw this application.");
    }
  }

}
