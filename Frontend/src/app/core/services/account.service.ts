import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { environment } from '../../../environments/environment';
import { IUserRegisterRequestDto } from '../../core/models/IUserRegisterRequestDto';
import { IUserRegisterResponseDto } from '../../core/models/IUserRegisterResponseDto';
import { Observable } from 'rxjs';
import { IUserLoginRequestDto } from '../../core/models/IUserLoginRequestDto';
import { IUserLoginResponseDto } from '../../core/models/IUserLoginResponseDto';
import { IUserResponseDto } from '../../core/models/IUserResponseDto';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseURL = `${environment.SERVER_URL}/api/user/`;
  constructor(private http: HttpClient, private storage: LocalStorageService) {}

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

  deleteUser(userId: number): Observable<object> {
    return this.http.delete(this.baseURL + userId);
  }
}
