import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemCreateComponent } from './item-create.component';
import { HttpClientModule } from '@angular/common/http';
import {
  NoopAnimationsModule,
  BrowserAnimationsModule,
} from '@angular/platform-browser/animations';
import { MaterialModule } from '../../../shared/material/material.module';

describe('ItemCreateComponent', () => {
  let component: ItemCreateComponent;
  let fixture: ComponentFixture<ItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ItemCreateComponent],
      imports: [
        HttpClientModule,
        MaterialModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(ItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
