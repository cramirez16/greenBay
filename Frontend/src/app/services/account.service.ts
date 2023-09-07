import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../environments/environment';
import { IUserRegisterRequestDto } from '../greenbay/models/IUserRegisterRequestDto';
import { IUserRegisterResponseDto } from '../greenbay/models/IUserRegisterResponseDto';
import { Observable } from 'rxjs';
import { IUserLoginRequestDto } from '../greenbay/models/IUserLoginRequestDto';
import { IUserLoginResponseDto } from '../greenbay/models/IUserLoginResponseDto';
import { IUserResponseDto } from '../greenbay/models/IUserResponseDto';
import {
  DialogContent,
  DialogPopupComponent,
} from '../greenbay/dialog-popup/dialog-popup.component';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseURL = `${environment.SERVER_URL}/api/user/`;
  constructor(
    private http: HttpClient,
    private storage: LocalStorageService,
    private dialog: MatDialog,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  register(
    user: IUserRegisterRequestDto
  ): Observable<IUserRegisterResponseDto> {
    return this.http.post<IUserRegisterResponseDto>(
      this.baseURL + 'register',
      user
    );
  }

  login(user: IUserLoginRequestDto): Observable<IUserLoginResponseDto> {
    // HTTP requests made using Angular's HttpClient, manual unsubscription is
    // not needed because Angular's HttpClient manages subscriptions internally
    // and automatically unsubscribes when necessary.
    return this.http.post<IUserLoginResponseDto>(this.baseURL + 'login', user);
  }

  getUserInfo(userId: string): Observable<IUserResponseDto> {
    return this.http.get<IUserResponseDto>(this.baseURL + userId);
  }

  logout(): void {
    this.storage.clear();
  }

  updateUser(
    userId: string,
    user: IUserRegisterRequestDto
  ): Observable<HttpResponse<void>> {
    return this.http.put<HttpResponse<void>>(this.baseURL + userId, user);
  }

  checkPassword(userId: string, password: string) {
    return this.http.get<HttpResponse<void>>(
      `${this.baseURL}check?id=${userId}&password=${password}`
    );
  }

  get name(): string {
    return this.storage.get('name');
  }

  get isAuthenticated(): boolean {
    return this.storage.get('tokenKey');
  }

  get isAdmin(): boolean {
    return this.storage.get('isAdmin') === true;
  }

  deleteUser(userId: number) {
    const deleteUrl = `${this.baseURL}${userId}`;
    const deleteDialog: DialogContent = {
      title: 'Remove account',
      description:
        'Please confirm that you want to remove your account and all your personal data.',
      buttons: [
        {
          buttonText: 'Remove',
          buttonColor: 'warn',
          func: () => {
            this.http.delete(deleteUrl).subscribe({
              next: () => {
                this.deleteUser(userId);
                this.logout();
                this.router.navigateByUrl('/items');
                this.dialog.closeAll();
              },
              error: (error: Error) => {
                this.dialog.closeAll();
                this.snackBar.open(
                  `Something went wrong. Try again later.`,
                  '',
                  {
                    duration: 10000,
                  }
                );
              },
            });
          },
        },
        {
          buttonText: 'Cancel',
          buttonColor: 'primary',
          func: () => {
            this.dialog.closeAll();
          },
        },
      ],
    };
    this.dialog.open(DialogPopupComponent, {
      data: deleteDialog,
    });
  }
}
