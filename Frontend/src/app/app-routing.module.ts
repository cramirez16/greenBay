import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './greenbay/landing-page/landing-page.component';
import { LoginComponent } from './greenbay/login/login.component';
import { RegisterComponent } from './greenbay/register/register.component';
import { LoginewComponent } from './greenbay/loginew/loginew.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'loginew', component: LoginewComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', component: LandingPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
