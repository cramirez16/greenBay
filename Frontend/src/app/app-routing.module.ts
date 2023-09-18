import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
import { ItemListComponent } from './features/item/item-list/item-list.component';
import { ItemCreateComponent } from './features/item/item-create/item-create.component';
import { ItemDetailedViewComponent } from './features/item/item-detailed-view/item-detailed-view.component';
import { ItemSearchComponent } from './features/item/item-search/item-search.component';
import { UserLoginComponent } from './features/user/user-login/user-login.component';
import { UserRegisterComponent } from './features/user/user-register/user-register.component';
import { UserUpdateComponent } from './features/user/user-update/user-update.component';
import { UserDeleteComponent } from './features/user/user-delete/user-delete.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: UserLoginComponent },
  { path: 'register', component: UserRegisterComponent },
  { path: 'items', component: ItemListComponent },
  { path: 'user-delete', component: UserDeleteComponent },
  { path: 'user-update', component: UserUpdateComponent },
  { path: 'search', component: ItemSearchComponent },
  {
    path: 'create-item',
    component: ItemCreateComponent,
  },
  {
    path: 'detailed-view',
    component: ItemDetailedViewComponent,
  },
  { path: '**', component: LandingPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
