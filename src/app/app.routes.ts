import { Routes } from '@angular/router';
import { LoginComponent } from './Pages/login/login.component';
import { RegisterComponent } from './Pages/register/register.component';
import { VerifyCodeComponent } from './Pages/verify-code/verify-code.component';

export const routes: Routes = [
  { path: 'register', component: RegisterComponent } ,
  { path: 'validate-code', component: VerifyCodeComponent } ,
  { path: 'login', component: LoginComponent } ,
  { path: '', component: LoginComponent }, // route racine = page de démarrage
  { path: '**', redirectTo: '' } // redirection pour les routes inconnues
];
