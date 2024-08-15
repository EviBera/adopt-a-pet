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

export interface IUpdatePet {
    id: number,
    name: string,
    isNeutered: boolean,
    description: string,
    pictureLink: string
}

export interface ICreatePet {
    name: string,
    species: string,
    birth: string,
    gender: string,
    isNeutered: boolean,
    description: string,
    pictureLink: string
}

export const speciesValues = [
    {title: 'All', value: 'all'},
    {title: 'Dogs', value: 'dogs'},
    {title: 'Cats', value: 'cats'},
    {title: 'Rabbits', value: 'rabbits'},
    {title: 'Hamsters', value: 'hamsters'},
    {title: 'Elephants', value: 'elephants'},
]

export const petSpecies = [
    {title: 'Dog', value: 'Dog'},
    {title: 'Cat', value: 'Cat'},
    {title: 'Rabbit', value: 'Rabbit'},
    {title: 'Hamster', value: 'Hamster'},
    {title: 'Elephant', value: 'Elephant'},
]
