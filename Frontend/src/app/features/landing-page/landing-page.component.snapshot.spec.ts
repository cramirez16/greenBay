import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LandingPageComponent } from './landing-page.component';
import { MaterialModule } from '../../material/material.module';

describe('LandingPageComponent', () => {
  let component: LandingPageComponent;
  let fixture: ComponentFixture<LandingPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LandingPageComponent],
      imports: [MaterialModule],
    });
    fixture = TestBed.createComponent(LandingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('[snapshot] should renders markup to snapshot', () => {
    expect(fixture).toMatchSnapshot();
  });
});
