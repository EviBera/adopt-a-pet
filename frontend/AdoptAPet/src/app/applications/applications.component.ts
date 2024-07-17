import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IApplication } from '../models/application.model';
import { AppService } from './app.service';
import { ApplicationDetailsComponent } from '../application-details/application-details.component';

@Component({
  selector: 'aap-applications',
  standalone: true,
  imports: [CommonModule,
    ApplicationDetailsComponent
  ],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit{
  private applications: IApplication[] = [];

  constructor(private appSvc: AppService){}

  ngOnInit(): void {
    this.appSvc.getApplications().subscribe({
      next: (applications) => (this.applications = applications)
    })
  }

  get applicationList(){
    return this.applications;
  }

  withdrawApplication( app: IApplication){
    console.log("withdraw btn clicked on application " + app.id);

    if(app.isAccepted === null){
      this.appSvc.withdraw(app.id);
    }
    else {
      alert("You can not withdraw this application.");
    }
    
  }

}
