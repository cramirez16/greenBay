import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserLoginComponent } from './user-login/user-login.component';
import { MaterialModule } from '../material/material.module';
import { FormlyModule } from '@ngx-formly/core';
import { FormlyBootstrapModule } from '@ngx-formly/bootstrap';
import { DialogPopupComponent } from './dialog-popup/dialog-popup.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { RegisterComponent } from './user-register/register.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AccountService } from '../services/account.service';
import { MessagePopupComponent } from './message-popup/message-popup.component';
import { ItemListComponent } from './item-list/item-list.component';
import { ItemCreateComponent } from './item-create/item-create.component';
import { ItemDetailedViewComponent } from './item-detailed-view/item-detailed-view.component';
import { ItemSearchComponent } from './item-search/item-search.component';
import { GenericBannerComponent } from './generic-banner/generic-banner.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import { UserUpdateComponent } from './user-update/user-update.component';

@NgModule({
  declarations: [
    UserLoginComponent,
    RegisterComponent,
    LandingPageComponent,
    DialogPopupComponent,
    MessagePopupComponent,
    ItemCreateComponent,
    GenericBannerComponent,
    UserDeleteComponent,
    UserUpdateComponent,
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
    ItemListComponent,
    ItemDetailedViewComponent,
    ItemSearchComponent,
  ],
  providers: [AccountService],
})
export class GreenbayModule {}
