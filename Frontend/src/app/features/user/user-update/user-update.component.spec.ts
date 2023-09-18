import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserUpdateComponent } from './user-update.component';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from '../../../shared/material/material.module';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';

describe('UserUpdateComponent', () => {
  let component: UserUpdateComponent;
  let fixture: ComponentFixture<UserUpdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserUpdateComponent],
      imports: [
        HttpClientModule,
        MaterialModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(UserUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
