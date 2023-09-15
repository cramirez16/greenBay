import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateItemComponent } from './create-item.component';
import { HttpClientModule } from '@angular/common/http';
import {
  NoopAnimationsModule,
  BrowserAnimationsModule,
} from '@angular/platform-browser/animations';
import { MaterialModule } from '../../material/material.module';

describe('CreateItemComponent', () => {
  let component: CreateItemComponent;
  let fixture: ComponentFixture<CreateItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateItemComponent],
      imports: [
        HttpClientModule,
        MaterialModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(CreateItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
