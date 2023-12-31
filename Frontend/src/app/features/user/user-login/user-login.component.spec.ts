import { ComponentFixture, TestBed, fakeAsync } from '@angular/core/testing';
import { AccountService } from '../../../core/services/account.service';
import { IUserLoginResponseDto } from '../../../core/models/IUserLoginResponseDto';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserLoginRequestDto } from '../../../core/models/IUserLoginRequestDto';
import { Observable, of, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UserLoginComponent } from './user-login.component';
import { UserValidationService } from '../../../core/services/user-validation.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MaterialModule } from '../../../shared/material/material.module';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../../core/services/jwt-decoder.service';
import { GenericBannerComponent } from '../../../features/generic-banner/generic-banner.component';

export class MockRouter {
  navigate() {
    // Do nothing, this is a mock implementation
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

  constructor(tokenContent?: tokenContent) {
    if (tokenContent) {
      this.tokenContent = tokenContent;
    }
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

describe('UserLoginComponent Suscefull Login', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
  let accountService: AccountService;
  //const mockAccountService = new MockAccountService();
  let httpMock: HttpTestingController;
  let mockMatDialog: MatDialog;
  const mockJwtDecoderService = new MockJwtDecoderService();

  // Create a mock LocalStorageService instance
  //const mockLocalStorageService = new MockLocalStorageService();

  beforeEach(async () => {
    const accountServiceMock = {
      login: jest.fn(),
    };

    mockMatDialog = {
      open: jest.fn(),
    } as unknown as MatDialog;

    await TestBed.configureTestingModule({
      declarations: [UserLoginComponent],
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

    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(UserLoginComponent);
    component = fixture.componentInstance;

    accountService = TestBed.inject(AccountService);

    // Set initial model values
    component.email.setValue('admin@fox.hu');
    component.password.setValue('Admin123');

    // Mark the form controls as pristine and untouched
    component.email.markAsTouched();
    component.email.markAsPristine();
    component.password.markAsTouched();
    component.password.markAsPristine();

    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should set local storage on successful login', fakeAsync(async () => {
    const mockStorageAfterResponse: any = {
      userId: 1,
      name: 'Admin',
      tokenKey: 'fakeToken',
      isAdmin: true,
      email: 'admin@fox.hu',
      money: 100,
    };
    mockJwtDecoderService.tokenContent = mockStorageAfterResponse;
    const mockDecodeToken: IUserLoginResponseDto = {
      tokenKey: 'fakeToken',
    };

    // Mock the accountService.login to return the mockResponse
    accountService.login = jest.fn(() => of(mockDecodeToken));

    // Call the component.submit method ---> will store the response data
    // in the mockedLocalStorage
    component.submit();

    // Reading data from LocalStorage
    const userIdInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('userId');
    const nameInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('name');
    const tokenInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('tokenKey');
    const isAdminInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('isAdmin');
    const emailInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('email');
    const moneyInLocalStorage =
      mockJwtDecoderService.mockLocalStorageService.get('money');

    expect(userIdInLocalStorage).toBe(mockStorageAfterResponse.userId);
    expect(nameInLocalStorage).toBe(mockStorageAfterResponse.name);
    expect(tokenInLocalStorage).toBe(mockStorageAfterResponse.tokenKey);
    expect(isAdminInLocalStorage).toBe(mockStorageAfterResponse.isAdmin);
    expect(emailInLocalStorage).toBe(mockStorageAfterResponse.email);
    expect(moneyInLocalStorage).toBe(mockStorageAfterResponse.money);
  }));
});

class MockAccountService {
  private loginResponse: Observable<any>;

  constructor(loginResponse: Observable<any>) {
    this.loginResponse = loginResponse;
  }

  login(user: IUserLoginRequestDto): Observable<any> {
    return this.loginResponse;
  }
}

//------------------------------------------------------
//                       Test 2
//------------------------------------------------------

describe('LoginComponent with Network Error', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
  let mockAccountService: MockAccountService;
  let mockMatDialog: MatDialog;

  beforeEach(async () => {
    // intializing mockAccountService for the specific case
    const errorResponse = new HttpErrorResponse({
      error: 'Unauthorized',
      status: 0,
      statusText: 'Unauthorized',
    });
    mockAccountService = new MockAccountService(throwError(errorResponse));

    // Moking MatDialog
    mockMatDialog = {
      open: jest.fn(),
    } as unknown as MatDialog;

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, MaterialModule, RouterTestingModule],
      declarations: [UserLoginComponent],
      providers: [
        LocalStorageService,
        UserValidationService,
        { provide: AccountService, useValue: mockAccountService },
        { provide: MatDialog, useValue: mockMatDialog },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserLoginComponent);
    component = fixture.componentInstance;
  });

  it('should handle Network Error', () => {
    // Set initial model values
    component.email.setValue('admin@fox.hu');
    component.password.setValue('Admin123');

    // Mark the form controls as pristine and untouched
    component.email.markAsTouched();
    component.email.markAsPristine();
    component.password.markAsTouched();
    component.password.markAsPristine();

    // calling submit login method in login component.
    component.submit();

    // expected display dialog content on error handling.
    const dialogData = {
      titleSection: '',
      messageSection: 'Network Error',
    };

    // Assertion
    expect(mockMatDialog.open).toHaveBeenCalledWith(GenericBannerComponent, {
      data: dialogData,
    });
  });
});

//------------------------------------------------------
//                       Test 3
//------------------------------------------------------

describe('LoginComponent with Unauthorized', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
  let mockAccountService: MockAccountService;
  let mockMatDialog: MatDialog;

  beforeEach(async () => {
    // intializing mockAccountService for the specific case
    const errorResponse = new HttpErrorResponse({
      error: 'Unauthorized',
      status: 401,
      statusText: 'Unauthorized',
    });
    mockAccountService = new MockAccountService(throwError(errorResponse));

    mockMatDialog = {
      open: jest.fn(),
    } as unknown as MatDialog;

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, MaterialModule, RouterTestingModule],
      declarations: [UserLoginComponent],
      providers: [
        LocalStorageService,
        UserValidationService,
        { provide: AccountService, useValue: mockAccountService },
        { provide: MatDialog, useValue: mockMatDialog },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserLoginComponent);
    component = fixture.componentInstance;
  });

  it('should handle Unauthorized', () => {
    // Set initial model values
    component.email.setValue('admin@fox.hu');
    component.password.setValue('Admin123');

    // Mark the form controls as pristine and untouched
    component.email.markAsTouched();
    component.email.markAsPristine();
    component.password.markAsTouched();
    component.password.markAsPristine();

    // calling submit login method in login component.
    component.submit();

    // expected display dialog content on error handling.
    const dialogData = {
      titleSection: 'Login failed',
      messageSection: 'Wrong credentials',
    };

    // Assertion
    expect(mockMatDialog.open).toHaveBeenCalledWith(GenericBannerComponent, {
      data: dialogData,
    });
  });
});
