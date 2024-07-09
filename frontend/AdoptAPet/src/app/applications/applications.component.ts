import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IApplication } from '../models/application.model';
import { AppService } from './app.service';

@Component({
  selector: 'aap-applications',
  standalone: true,
  imports: [CommonModule],
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

}
