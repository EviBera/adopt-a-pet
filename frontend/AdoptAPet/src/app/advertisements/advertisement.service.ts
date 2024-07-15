import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAdvertisement } from '../models/advertisement.model';

@Injectable({
  providedIn: 'root'
})
export class AdvertisementService {

  constructor(private http: HttpClient) { }

  getAdvertisementsOfAdoptablePets(): Observable<IAdvertisement[]> {
    return this.http.get<IAdvertisement[]>('api/advertisement/current')
  }
}
