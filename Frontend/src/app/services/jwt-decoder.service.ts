import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class JwtDecoderService {
  constructor(private storage: LocalStorageService) {}

  decode(token: string) {
    const tokenContent = new JwtHelperService().decodeToken(token);
    const isAdmin =
      tokenContent[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ] === 'Admin'
        ? true
        : false;
    this.storage.set('userId', tokenContent.userId);
    this.storage.set('name', tokenContent.email);
    this.storage.set('tokenKey', token);
    this.storage.set('isAdmin', isAdmin);
    this.storage.set('money', tokenContent.money);
  }
}
