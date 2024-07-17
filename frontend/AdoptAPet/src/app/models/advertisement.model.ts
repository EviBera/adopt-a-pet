import { IPet } from "./pet.model"
import { IApplication } from "./application.model"

export interface IAdvertisement {
    id: number,
    petDto: IPet,
    createdAt: Date,
    expiresAt: Date,
    applications: IApplication[]
}