import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannerBidComponent } from './banner-bid.component';

describe('BannerBidComponent', () => {
  let component: BannerBidComponent;
  let fixture: ComponentFixture<BannerBidComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BannerBidComponent]
    });
    fixture = TestBed.createComponent(BannerBidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
