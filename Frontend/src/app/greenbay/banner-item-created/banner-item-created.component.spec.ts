import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannerItemCreatedComponent } from './banner-item-created.component';
import { MaterialModule } from '../../material/material.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

describe('BannerItemCreatedComponent', () => {
  let component: BannerItemCreatedComponent;
  let fixture: ComponentFixture<BannerItemCreatedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BannerItemCreatedComponent],
      imports: [MaterialModule],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} },
      ],
    });
    fixture = TestBed.createComponent(BannerItemCreatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
