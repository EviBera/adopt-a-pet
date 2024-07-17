export interface IApplication {
    id: number,
    userId: string,
    advertisementId: number,
    isAccepted: boolean
    advertisementExpiresAt: Date,
    petName: string,
    petSpecies: string,
    petBirth: Date,
    petGender: string,
    petIsNeutered: boolean,
    petDescription: string,
    petPictureLink: string
}