import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemDetailedViewComponent } from './item-detailed-view.component';
import { HttpClientModule } from '@angular/common/http';
import {
  NoopAnimationsModule,
  BrowserAnimationsModule,
} from '@angular/platform-browser/animations';
import { MaterialModule } from '../../../shared/material/material.module';

describe('ItemDetailedViewComponent', () => {
  let component: ItemDetailedViewComponent;
  let fixture: ComponentFixture<ItemDetailedViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [],
      imports: [
        ItemDetailedViewComponent,
        HttpClientModule,
        MaterialModule,
        NoopAnimationsModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(ItemDetailedViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
