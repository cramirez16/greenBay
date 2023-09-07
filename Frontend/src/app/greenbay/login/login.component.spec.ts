import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginewComponent } from './login.component';

describe('LoginewComponent', () => {
  let component: LoginewComponent;
  let fixture: ComponentFixture<LoginewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LoginewComponent],
    });
    fixture = TestBed.createComponent(LoginewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
