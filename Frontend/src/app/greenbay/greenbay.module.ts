import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { LandingPageComponent } from './landing-page/landing-page.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    LandingPageComponent
  ],
  imports: [
    CommonModule
  ]
})
export class GreenbayModule { }
