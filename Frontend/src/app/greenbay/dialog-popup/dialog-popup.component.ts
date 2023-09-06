import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

type ButtonColor =
  | 'basic'
  | 'primary'
  | 'accent'
  | 'warn'
  | 'disabled'
  | 'link';

type Button = { buttonText: string; buttonColor: ButtonColor; func: Function };

export interface DialogContent {
  title: string;
  description: string;
  buttons: Button[];
}
@Component({
  selector: 'app-dialog-popup',
  templateUrl: './dialog-popup.component.html',
  styleUrls: ['./dialog-popup.component.css'],
})
export class DialogPopupComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA)
    public dialog: DialogContent
  ) {}
}
