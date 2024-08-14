import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IPet } from '../models/pet.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {

  constructor(private http: HttpClient) { }

  getPets(): Observable<IPet[]> {
    return this.http.get<IPet[]>('api/pet', {withCredentials: true});
  }

  updatePet(petId: number){
    console.log("UPDATE");
  }

  deletePet(petId: number){
    console.log("DELETE");
  }
}
