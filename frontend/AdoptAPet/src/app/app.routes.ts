import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdvertisementListComponent } from './advertisements/advertisement-list.component';
import { ApplicationsComponent } from './applications/applications.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { LogoutComponent } from './user/logout/logout.component';

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
        path: 'login',
        component: LoginComponent,
        title: "Login"
    },
    {
        path: 'register',
        component: RegisterComponent,
        title: "Register"
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
