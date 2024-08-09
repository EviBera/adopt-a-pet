import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IApplication } from '../models/application.model';
import { HttpClient } from '@angular/common/http';
import { IAdvertisement } from '../models/advertisement.model';
import { IUser } from '../models/user.model';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private applications: BehaviorSubject<IApplication[]> = new BehaviorSubject<IApplication[]>([]);
  private user: IUser | null = null;

  constructor(private http: HttpClient, private userSvc: UserService) {
    this.userSvc.getUser().subscribe({
      next: (user) => {
        this.user = user;
        this.fetchApplications();
      }
    });
  }

  private fetchApplications() {
    if(this.user === null)
      return;    

    if (!this.user?.id) {
      console.error('User ID is not available.');
      return;
    }

    const url = 'api/application/' + this.user?.id;
    this.http.get<IApplication[]>(url, {withCredentials: true})
      .subscribe({
        next: (applications) => (this.applications.next(applications)),
        error: (err) => console.error("Error fetching applications: ", err)
      });
  }

  getApplications(): Observable<IApplication[]> {
    return this.applications.asObservable();
  }

  handIn(ad: IAdvertisement) {
    if (!this.user?.id) {
      console.error('User ID is not available.');
      return;
    }
    
    const appRequest = {
      "userId": this.user?.id,
      "advertisementId": ad.id
    }

    this.http.post('/api/application', appRequest, {withCredentials: true}).subscribe(() => {
      console.log("new application was handed in");
      this.fetchApplications();
    });
  }

  withdraw(applicationId: number) {
    const url = '/api/application/' + applicationId;
    this.http.delete(url, {withCredentials: true}).subscribe({
      next: data => {
        this.fetchApplications();
      },
      error: err => {
        console.error('Observable emitted an error: ' + err);
      }
    })
  }

}
