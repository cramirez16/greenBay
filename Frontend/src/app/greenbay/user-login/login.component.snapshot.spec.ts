import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from '@angular/core/testing';
import { AccountService } from '../../services/account.service';
import { IUserLoginResponseDto } from '../models/IUserLoginResponseDto';
import { LocalStorageService } from '../../services/local-storage.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserLoginRequestDto } from '../models/IUserLoginRequestDto';
import { Observable, of, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from './login.component';
import { UserValidationService } from '../../services/user-validation.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MessagePopupComponent } from '../message-popup/message-popup.component';
import { MaterialModule } from '../../material/material.module';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../services/jwt-decoder.service';

export class MockRouter {
  navigate() {
    // Do nothing, this is a mock implementation
  }
}

class MockAccountService {
  private loginResponse: Observable<any>;

  constructor(loginResponse: Observable<any>) {
    this.loginResponse = loginResponse;
  }

  login(user: IUserLoginRequestDto): Observable<any> {
    return this.loginResponse;
  }
}

class MockLocalStorageService {
  private data: { [key: string]: any } = {};

  set(key: string, value: any) {
    this.data[key] = value;
  }

  get(key: string) {
    return this.data[key];
  }

  remove(key: string) {
    return this.data[key];
  }

  clear() {
    this.data = {};
  }
}

type tokenContent = {
  userId: string;
  name: string;
  isAdmin: boolean;
  email: string;
  money: number;
};

class MockJwtDecoderService {
  public tokenContent!: tokenContent;

  constructor(tokenContent: tokenContent) {
    this.tokenContent = tokenContent;
  }

  public mockLocalStorageService = new MockLocalStorageService();
  decode(token: string) {
    this.mockLocalStorageService.set('userId', this.tokenContent.userId);
    this.mockLocalStorageService.set('name', this.tokenContent.name);
    this.mockLocalStorageService.set('email', this.tokenContent.email);
    this.mockLocalStorageService.set('tokenKey', token);
    this.mockLocalStorageService.set('isAdmin', this.tokenContent.isAdmin);
    this.mockLocalStorageService.set('money', this.tokenContent.money);
  }
}

describe('[snapshot] LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  //let accountService: AccountService;
  //const mockAccountService = new MockAccountService();
  //let httpMock: HttpTestingController;
  let mockMatDialog: MatDialog;
  const storageAfterResponse: any = {
    userId: 1,
    name: 'Admin',
    isAdmin: true,
    email: 'admin@fox.hu',
    money: 100,
  };
  const mockJwtDecoderService = new MockJwtDecoderService(storageAfterResponse);

  beforeEach(() => {
    const accountServiceMock = {
      login: jest.fn(),
    };

    mockMatDialog = {
      open: jest.fn(),
    } as unknown as MatDialog;

    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [
        ReactiveFormsModule,
        MaterialModule,
        BrowserAnimationsModule,
        HttpClientTestingModule,
      ],
      providers: [
        { provide: MatDialog, useValue: mockMatDialog },
        { provide: AccountService, useValue: accountServiceMock },
        {
          provide: LocalStorageService,
          useValue: mockJwtDecoderService.mockLocalStorageService,
        },
        { provide: Router, useClass: MockRouter },
        { provide: JwtDecoderService, useValue: mockJwtDecoderService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('[snapshot] should renders markup to snapshot', () => {
    expect(fixture).toMatchSnapshot();
  });
});
