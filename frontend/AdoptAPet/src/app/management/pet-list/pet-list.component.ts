import { Component } from '@angular/core';
import { IPet, IUpdatePet } from '../../models/pet.model';
import { ManagementService } from '../management.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'aap-pet-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pet-list.component.html',
  styleUrl: './pet-list.component.css'
})
export class PetListComponent {
  pets: IPet[] = [];
  showUpdateForm: boolean = false;
  idToUpdate: number | null = null;
  idToDelete: number | null = null;
  updateError: boolean = false;
  deleteError: boolean = false;
  message: string = '';
  updatePetModel: IUpdatePet = {
    id: 0,
    name: '',
    isNeutered: false,
    description: '',
    pictureLink: ''
  }

  constructor(private managementSvc: ManagementService){}

  ngOnInit(){
    this.fetchPets();
  }

  fetchPets(){
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

  showForm(pet: IPet){
    this.showUpdateForm = true;
    this.idToUpdate = pet.id;

    this.updatePetModel.id = pet.id;
    this.updatePetModel.name = pet.name;
    this.updatePetModel.isNeutered = pet.isNeutered;
    this.updatePetModel.description = pet.description;
    this.updatePetModel.pictureLink = pet.pictureLink;
  }
  
  updatePet(form: NgForm){
    this.managementSvc.updatePet(this.updatePetModel).subscribe({
      next: () => {
        alert("Pet has been updated successfully.");
        this.clearFields();
        this.fetchPets();
      },
      error: (err) => {
        this.updateError = true;
        this.message = err;
      }
    });
  }

  showConfirm(petId: number){
    this.idToDelete = petId;
  }
  
  deletePet(){
    console.log("Delete: " + this.idToDelete);
    if(this.idToDelete == null)
      return;

    this.managementSvc.deletePet(this.idToDelete).subscribe({
      next: () => {
        alert("Pet has been deleted successfully.");
        this.clearFields();
        this.fetchPets();
      },
      error: (err) => {
        this.deleteError = true;
        this.message = err;
      }
    })
  }
  
  cancel(){
    this.clearFields();
  }
  
  clearFields(){
    this.showUpdateForm = false;
    this.idToDelete = null;
    this.idToUpdate = null;
    this.updatePetModel = {
      id: 0,
      name: '',
      isNeutered: false,
      description: '',
      pictureLink: ''
    }
  }
}
