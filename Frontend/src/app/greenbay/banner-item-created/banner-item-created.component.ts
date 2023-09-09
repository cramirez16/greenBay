import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IItemRequestDto } from '../models/IItemRequestDto';

@Component({
  selector: 'app-banner-item-created',
  templateUrl: './banner-item-created.component.html',
  styleUrls: ['./banner-item-created.component.css'],
})
export class BannerItemCreatedComponent {
  title: string = '';
  description: string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) private item: IItemRequestDto,
    private dialogRef: MatDialogRef<BannerItemCreatedComponent>
  ) {
    this.title = item.name;
    this.description = item.description;
  }

  closePopUp() {
    this.dialogRef.close();
  }
}
