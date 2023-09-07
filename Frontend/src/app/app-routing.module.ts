import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './greenbay/landing-page/landing-page.component';
import { LoginComponent } from './greenbay/login/login.component';
import { RegisterComponent } from './greenbay/register/register.component';
import { ItemsComponent } from './greenbay/items/items.component';
import { MyProfileComponent } from './greenbay/my-profile/my-profile.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'items', component: ItemsComponent },
  { path: 'my_profile', component: MyProfileComponent },
  { path: '**', component: LandingPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
