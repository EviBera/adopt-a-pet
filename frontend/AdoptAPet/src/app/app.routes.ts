import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdvertisementListComponent } from './advertisements/advertisement-list.component';
import { ApplicationsComponent } from './applications/applications.component';

export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
        title: 'Home'
    },
    {
        path: 'adopt',
        component: AdvertisementListComponent,
        title: 'Adoptable pets'
    },
    {
        path: 'applications',
        component: ApplicationsComponent,
        title: 'My applications'
    },
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    }
];
