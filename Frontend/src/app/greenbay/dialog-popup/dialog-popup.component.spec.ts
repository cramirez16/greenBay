import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogPopupComponent } from './dialog-popup.component';
import { MaterialModule } from '../../material/material.module';

describe('DialogPopupComponent', () => {
  let component: DialogPopupComponent;
  let fixture: ComponentFixture<DialogPopupComponent>;

  const mockDialog = {
    title: 'Test Title',
    description: 'Test Description',
    buttons: [
      {
        buttonText: 'Test Button',
        buttonColor: 'primary',
        func: () => console.log('Test Function'),
      },
    ],
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogPopupComponent],
      imports: [MaterialModule],
      providers: [{ provide: MAT_DIALOG_DATA, useValue: mockDialog }],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have correct dialog data', () => {
    expect(component.dialog).toEqual(mockDialog);
  });
});
