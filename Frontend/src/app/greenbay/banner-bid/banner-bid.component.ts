import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IBidResponseDto } from '../models/IBidResponseDto';

@Component({
  selector: 'app-banner-bid',
  templateUrl: './banner-bid.component.html',
  styleUrls: ['./banner-bid.component.css'],
})
export class BannerBidComponent {
  bidAmount: number = 0;
  message: string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA)
    private bidResponse: { bidAmount: number; message: string },
    private dialogRef: MatDialogRef<BannerBidComponent>
  ) {
    this.bidAmount = bidResponse.bidAmount;
    this.message = bidResponse.message;
  }

  closePopUp() {
    this.dialogRef.close();
  }
}
