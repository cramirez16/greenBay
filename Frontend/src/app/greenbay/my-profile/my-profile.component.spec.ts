import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyProfileComponent } from './my-profile.component';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from '../../material/material.module';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';

describe('MyProfileComponent', () => {
  let component: MyProfileComponent;
  let fixture: ComponentFixture<MyProfileComponent>;

  beforeEach(() => {
    // mockMatDialog = {
    //   open: jest.fn(),
    // } as unknown as MatDialog;

    // mockMatSnackBar = {
    //   open: jest.fn(),
    // } as unknown as MatSnackBar;
    TestBed.configureTestingModule({
      declarations: [MyProfileComponent],
      imports: [
        HttpClientModule,
        MaterialModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
      // providers: [
      //   { provide: MatDialog, useValue: mockMatDialog },
      //   { provide: MatSnackBar, useValue: mockMatSnackBar },
      // ],
    });
    fixture = TestBed.createComponent(MyProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
