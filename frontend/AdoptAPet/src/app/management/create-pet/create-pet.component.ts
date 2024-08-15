import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ICreatePet, petSpecies } from '../../models/pet.model';
import { ManagementService } from '../management.service';

@Component({
  selector: 'aap-create-pet',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-pet.component.html',
  styleUrl: './create-pet.component.css'
})
export class CreatePetComponent {
  @Output() cancel = new EventEmitter();

  petSpecies = petSpecies;  
  hasError: boolean = false;
  message: string = '';
  createPetModel: ICreatePet = {
    name: '',
    species: '',
    birth: '',
    gender: '',
    isNeutered: false,
    description: '',
    pictureLink: ''
  }

  constructor(private managementSvc: ManagementService){}
  
  savePet(form: NgForm){
    console.log(form.value);
    this.managementSvc.createPet(this.createPetModel).subscribe({
      next: () => {
        alert("New pet has been saved");
        setTimeout(() => this.clearFields(), 1500);
      },
      error: (err) => {
        this.hasError = true;
        this.message = err;
      }
    })
  }

  cancelButtonClicked(){
    this.cancel.emit();
  }

  clearFields(){
    this.createPetModel = {
      name: '',
      species: '',
      birth: '',
      gender: '',
      isNeutered: false,
      description: '',
      pictureLink: ''
    }
    this.hasError = false;
  }
}
