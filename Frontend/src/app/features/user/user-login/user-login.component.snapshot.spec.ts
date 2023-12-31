import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountService } from '../../../core/services/account.service';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { IUserLoginRequestDto } from '../../../core/models/IUserLoginRequestDto';
import { Observable } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UserLoginComponent } from './user-login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MaterialModule } from '../../../shared/material/material.module';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../../core/services/jwt-decoder.service';

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

describe('[snapshot] UserLoginComponent', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
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

    fixture = TestBed.createComponent(UserLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('[snapshot] should renders markup to snapshot', () => {
    expect(fixture).toMatchSnapshot();
  });
});
