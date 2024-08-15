import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IPet, IUpdatePet } from '../models/pet.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {

  constructor(private http: HttpClient) { }

  getPets(): Observable<IPet[]> {
    return this.http.get<IPet[]>('api/pet', {withCredentials: true});
  }

  updatePet(petModel: IUpdatePet){
    console.log("UPDATE");
    let url = 'api/pet/' + petModel.id;
    let body = {
      "name": petModel.name,
      "isNeutered": petModel.isNeutered,
      "description": petModel.description,
      "pictureLink": petModel.pictureLink
    }

    return this.http.patch<IPet>(url, body, {withCredentials: true});
  }

  deletePet(petId: number){
    console.log("DELETE");
  }
}
