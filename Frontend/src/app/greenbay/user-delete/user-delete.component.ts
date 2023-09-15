import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import {
  DialogContent,
  DialogPopupComponent,
} from '../dialog-popup/dialog-popup.component';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.css'],
})
export class UserDeleteComponent implements OnInit {
  name = new FormControl('');
  email = new FormControl('');

  constructor(
    // private _userValidator: UserValidationService,
    private _accountService: AccountService,
    private _storage: LocalStorageService,
    // private _snackBar: MatSnackBar,
    //private snackBarRef: MatSnackBarRef<MyProfileComponent>,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this._accountService.getUserInfo(this._storage.get('userId')!).subscribe({
      next: (response: any) => {
        this.name.setValue(response.name);
        this.email.setValue(response.email);
      },
      error: (res) => {},
    });
  }

  deleteUser() {
    const deleteDialog: DialogContent = {
      title: 'Remove account',
      description:
        'Please confirm that you want to remove your account and all your personal data.',
      buttons: [
        {
          buttonText: 'Remove',
          buttonColor: 'warn',
          func: () => {
            this._accountService
              .deleteUser(this._storage.get('userId')!)
              .subscribe({
                next: () => {
                  this._accountService.logout();
                  this.router.navigateByUrl('/');
                  this.dialog.closeAll();
                },
                error: (error: Error) => {
                  this.dialog.closeAll();
                  // this._snackBar.open(
                  //   `Something went wrong. Try again later.`,
                  //   '',
                  //   {
                  //     duration: 10000,
                  //   }
                  // );
                },
              });
          },
        },
        {
          buttonText: 'Cancel',
          buttonColor: 'primary',
          func: () => {
            this.dialog.closeAll();
          },
        },
      ],
    };
    this.dialog.open(DialogPopupComponent, {
      data: deleteDialog,
    });
  }
}
