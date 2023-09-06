import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagePopupComponent } from './message-popup.component';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

describe('MessagePopupComponent', () => {
  let component: MessagePopupComponent;
  let fixture: ComponentFixture<MessagePopupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessagePopupComponent],
      imports: [MatDialogModule],
      providers: [{ provide: MAT_DIALOG_DATA, useValue: {} }],
    });
    fixture = TestBed.createComponent(MessagePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
