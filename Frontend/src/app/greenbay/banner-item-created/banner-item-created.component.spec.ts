import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannerItemCreatedComponent } from './banner-item-created.component';

describe('BannerItemCreatedComponent', () => {
  let component: BannerItemCreatedComponent;
  let fixture: ComponentFixture<BannerItemCreatedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BannerItemCreatedComponent]
    });
    fixture = TestBed.createComponent(BannerItemCreatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
