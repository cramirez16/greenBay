import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyProfileComponent } from './my-profile.component';
import { HttpClientModule } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';

describe('MyProfileComponent', () => {
  let component: MyProfileComponent;
  let fixture: ComponentFixture<MyProfileComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyProfileComponent],
      imports: [HttpClientModule, MatDialog],
    });
    fixture = TestBed.createComponent(MyProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
