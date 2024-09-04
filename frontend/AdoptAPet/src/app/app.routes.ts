import { Routes } from '@angular/router';
import { inject } from '@angular/core';

import { UserService } from './user/user.service';
import { HomeComponent } from './home/home.component';
import { AdvertisementListComponent } from './advertisements/advertisement-list.component';
import { ApplicationsComponent } from './applications/applications.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LogoutComponent } from './user/logout/logout.component';
import { AdvertisementHandlerComponent } from './management/advertisement-handler/advertisement-handler.component';
import { ApplicationHandlerComponent } from './management/application-handler/application-handler.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';

export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
        title: 'Home'
    },
    {
        path: 'adopt',
        component: AdvertisementHandlerComponent,
        title: 'Advertisements',
        canMatch: [() => inject(UserService).canAdvertise()]
    },
    {
        path: 'adopt',
        component: AdminPanelComponent,
        title: 'Advertisements',
        canMatch: [() => inject(UserService).canSupervise()]
    },
    {
        path: 'adopt',
        component: AdvertisementListComponent,
        title: 'Adoptable pets'
    },
    {
        path: 'applications/:advertisementId',
        component: ApplicationHandlerComponent,
        title: 'Applications',
        canMatch: [() => inject(UserService).canAdvertise()]
    },
    {
        path: 'applications',
        component: ApplicationHandlerComponent,
        title: 'Applications',
        canMatch: [() => inject(UserService).canAdvertise()]
    },
    {
        path: ' ',
        component: AdminPanelComponent,
        title: 'Applications',
        canMatch: [() => inject(UserService).canSupervise()]
    },
    {
        path: 'applications',
        component: ApplicationsComponent,
        title: 'My applications'
    },
    {
        path: 'login',
        component: LoginComponent,
        title: "Login"
    },
    {
        path: 'registration',
        component: RegistrationComponent,
        title: "Registration"
    },
    {
        path: 'logout',
        component: LogoutComponent,
        title: "Logout"
    },
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    }
];
