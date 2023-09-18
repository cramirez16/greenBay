import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericBannerComponent } from './generic-banner.component';
import { MaterialModule } from '../../shared/material/material.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

describe('GenericBannerComponent', () => {
  let component: GenericBannerComponent;
  let fixture: ComponentFixture<GenericBannerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GenericBannerComponent],
      imports: [MaterialModule],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} },
      ],
    });
    fixture = TestBed.createComponent(GenericBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
