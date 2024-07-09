import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IApplication } from '../models/application.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private applications: BehaviorSubject<IApplication[]> = new BehaviorSubject<IApplication[]>([]);

  constructor(private http: HttpClient) {
    this.http.get<IApplication[]>('api/application/711704b1-035f-4f76-99d9-a1dece06c153')
      .subscribe({
        next: (applications) => (this.applications.next(applications))
      })
   }

   getApplications():Observable<IApplication[]>{
    return this.applications.asObservable();
   }
}
