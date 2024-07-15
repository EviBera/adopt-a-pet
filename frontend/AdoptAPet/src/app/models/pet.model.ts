export interface IPet {
    id: number,
    name: string,
    species: string,
    birth: Date,
    gender: string,
    isNeutered: boolean,
    description: string,
    ownerId: string,
    pictureLink: string
}