import { IPet } from "./pet.model"
import { IApplication } from "./application.model"

export interface IAdvertisement {
    id: number,
    petId: number,
    pet: IPet,
    createdAt: Date,
    expiresAt: Date,
    applications: IApplication[]
}