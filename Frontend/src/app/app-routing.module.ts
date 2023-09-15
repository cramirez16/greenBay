import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './greenbay/landing-page/landing-page.component';
import { LoginComponent } from './greenbay/user-login/login.component';
import { RegisterComponent } from './greenbay/user-register/register.component';
import { ItemListComponent } from './greenbay/item-list/item-list.component';
import { CreateItemComponent } from './greenbay/item-create/create-item.component';
import { DetailedViewComponent } from './greenbay/item-detailed-view/detailed-view.component';
import { ItemSearchComponent } from './greenbay/item-search/item-search.component';
import { UserDeleteComponent } from './greenbay/user-delete/user-delete.component';
import { UserUpdateComponent } from './greenbay/user-update/user-update.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'items', component: ItemListComponent },
  { path: 'user-delete', component: UserDeleteComponent },
  { path: 'user-update', component: UserUpdateComponent },
  { path: 'search', component: ItemSearchComponent },
  {
    path: 'create-item',
    component: CreateItemComponent,
  },
  {
    path: 'detailed-view',
    component: DetailedViewComponent,
  },
  { path: '**', component: LandingPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
