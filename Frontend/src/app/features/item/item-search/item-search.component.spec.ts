import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemSearchComponent } from './item-search.component';
import { MaterialModule } from '../../../shared/material/material.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ItemSearchComponent', () => {
  let component: ItemSearchComponent;
  let fixture: ComponentFixture<ItemSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [],
      imports: [
        ItemSearchComponent,
        MaterialModule,
        HttpClientModule,
        BrowserAnimationsModule,
      ],
    });
    fixture = TestBed.createComponent(ItemSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
