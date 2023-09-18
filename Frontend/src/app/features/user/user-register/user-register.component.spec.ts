import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserRegisterComponent } from './user-register.component';
import { MaterialModule } from '../../../shared/material/material.module';
import { HttpClientModule } from '@angular/common/http';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';

describe('UserRegisterComponent', () => {
  let component: UserRegisterComponent;
  let fixture: ComponentFixture<UserRegisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserRegisterComponent],
      imports: [
        MaterialModule,
        HttpClientModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(UserRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
