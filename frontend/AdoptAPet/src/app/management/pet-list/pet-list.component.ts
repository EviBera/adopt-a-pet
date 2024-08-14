import { Component } from '@angular/core';
import { IPet, IUpdatePet } from '../../models/pet.model';
import { ManagementService } from '../management.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'aap-pet-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pet-list.component.html',
  styleUrl: './pet-list.component.css'
})
export class PetListComponent {
  pets: IPet[] = [];
  showUpdateForm: boolean = false;
  idToUpdate: number | null = null;
  updatePetModel: IUpdatePet = {
    id: 0,
    name: '',
    isNeutered: false,
    description: '',
    pictureLink: ''
  }

  constructor(private managementSvc: ManagementService){}

  ngOnInit(){
    this.managementSvc.getPets().subscribe((pets) => {
      this.pets = pets;
    })
  }

  sortPets(pets: IPet[]): IPet[]{
    return pets.sort((a,b) => {return a.id - b.id});
  }

  get petList(): IPet[] {
    return this.sortPets(this.pets);
  }

  updatePet(id: number){
    console.log("Update: " + id);
    this.showUpdateForm = true;
    this.idToUpdate = id;
    this.managementSvc.updatePet(id);
  }

  deletePet(id: number){
    console.log("Delete: " + id);
    this.managementSvc.deletePet(id);
  }
}
