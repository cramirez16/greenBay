import { Component, Inject, OnInit } from '@angular/core';
import { IUserRegisterRequestDto } from '../models/IUserRegisterRequestDto';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.css'],
})
export class BannerComponent {
  name: string = '';
  email: string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) private user: IUserRegisterRequestDto,
    private dialogRef: MatDialogRef<BannerComponent>
  ) {
    this.name = user.name;
    this.email = user.email;
  }

  closePopUp() {
    this.dialogRef.close();
  }
}
