import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BannerBidComponent } from './banner-bid.component';
import { MaterialModule } from '../../material/material.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
describe('BannerBidComponent', () => {
  let component: BannerBidComponent;
  let fixture: ComponentFixture<BannerBidComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BannerBidComponent],
      imports: [MaterialModule],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} },
      ],
    });
    fixture = TestBed.createComponent(BannerBidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
