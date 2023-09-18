import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../shared//material/material.module';
import { FormlyModule } from '@ngx-formly/core';
import { FormlyBootstrapModule } from '@ngx-formly/bootstrap';
import { DialogPopupComponent } from './dialog-popup/dialog-popup.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AccountService } from '../core/services/account.service';
import { ItemListComponent } from './item/item-list/item-list.component';
import { ItemCreateComponent } from './item/item-create/item-create.component';
import { ItemDetailedViewComponent } from './item/item-detailed-view/item-detailed-view.component';
import { ItemSearchComponent } from './item/item-search/item-search.component';
import { GenericBannerComponent } from './generic-banner/generic-banner.component';
import { UserLoginComponent } from './user/user-login/user-login.component';
import { UserRegisterComponent } from './user/user-register/user-register.component';
import { UserUpdateComponent } from './user/user-update/user-update.component';
import { UserDeleteComponent } from './user/user-delete/user-delete.component';

@NgModule({
  declarations: [
    UserLoginComponent,
    UserRegisterComponent,
    LandingPageComponent,
    DialogPopupComponent,
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
export class FeaturesModule {}
