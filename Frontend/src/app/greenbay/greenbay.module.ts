import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { MaterialModule } from '../material/material.module';
import { FormlyModule } from '@ngx-formly/core';
import { FormlyBootstrapModule } from '@ngx-formly/bootstrap';
import { DialogPopupComponent } from './dialog-popup/dialog-popup.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { RegisterComponent } from './register/register.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AccountService } from '../services/account.service';
import { MessagePopupComponent } from './message-popup/message-popup.component';
import { ItemsComponent } from './items/items.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { CreateItemComponent } from './create-item/create-item.component';
import { DetailedViewComponent } from './detailed-view/detailed-view.component';
import { SearchComponent } from './search/search.component';
import { GenericBannerComponent } from './generic-banner/generic-banner.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    LandingPageComponent,
    DialogPopupComponent,
    MessagePopupComponent,
    MyProfileComponent,
    CreateItemComponent,
    GenericBannerComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormlyModule.forChild(),
    FormlyBootstrapModule,
    AppRoutingModule,
    BrowserModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ItemsComponent,
    DetailedViewComponent,
    SearchComponent,
  ],
  providers: [AccountService],
})
export class GreenbayModule {}
