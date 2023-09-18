import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-generic-banner',
  templateUrl: './generic-banner.component.html',
  styleUrls: ['./generic-banner.component.css'],
})
export class GenericBannerComponent {
  title: string = '';
  message: string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) private item: any,
    private dialogRef: MatDialogRef<GenericBannerComponent>
  ) {
    this.title = item.titleSection;
    this.message = item.messageSection;
  }

  closePopUp() {
    this.dialogRef.close();
  }
}
