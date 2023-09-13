import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { UserValidationService } from '../../services/user-validation.service';
import { AccountService } from '../../services/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IUserRegisterRequestDto } from '../models/IUserRegisterRequestDto';
import { LocalStorageService } from '../../services/local-storage.service';
import { Router } from '@angular/router';
import {
  DialogContent,
  DialogPopupComponent,
} from '../dialog-popup/dialog-popup.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css'],
})
export class MyProfileComponent implements OnInit {
  snackBarRef: any;
  name = new FormControl('', [
    Validators.required,
    this._userValidator.validName,
  ]);
  email = new FormControl('', [
    Validators.required,
    this._userValidator.validEmail,
  ]);
  oldPassword = new FormControl('', [Validators.required]);
  password = new FormControl('', [this._userValidator.validPassword]);
  confirmPassword = new FormControl('', [
    (control) => {
      if (this.password.value !== '' && !control.value) {
        return { required: true };
      }
      return null;
    },
    this._userValidator.validPasswordMatch(this.password),
  ]);

  updateSuccess = false;

  constructor(
    private _userValidator: UserValidationService,
    private _accountService: AccountService,
    private _storage: LocalStorageService,
    private _snackBar: MatSnackBar,
    //private snackBarRef: MatSnackBarRef<MyProfileComponent>,
    private router: Router,
    private dialog: MatDialog
  ) {}

  getErrorMessage(
    control: FormControl,
    errorMessages: { [key: string]: string }
  ) {
    for (const errorCode in errorMessages) {
      if (control.hasError(errorCode)) {
        return errorMessages[errorCode];
      }
    }
    return null;
  }

  getNameError() {
    return this.getErrorMessage(this.name, {
      required: 'You must enter your name',
      nameLength: 'The name must contain at least 3 characters',
    });
  }

  getEmailError() {
    return this.getErrorMessage(this.email, {
      required: 'You must enter your email address',
      emailInvalid: 'Not a valid email address',
      emailExist: 'The email already exists',
    });
  }

  getOldPasswordError() {
    return this.getErrorMessage(this.oldPassword, {
      required: 'Please enter your password to update',
      passwordWrong: 'Wrong password',
    });
  }

  getPasswordError() {
    if (!this.password.dirty && this.confirmPassword.value === '') {
      this.password.markAsPristine();
      this.password.markAsUntouched();
      if (this.password.value === '') {
        this.confirmPassword.markAsPristine();
        this.confirmPassword.markAsUntouched();
      }
      return null;
    }
    return this.getErrorMessage(this.password, {
      passwordLength: 'Password must contain at least 6 characters',
      passwordLetter: 'Password must contain a letter',
      passwordNumber: 'Password must contain a number',
      passwordCase: 'Password must contain upper and lower case',
    });
  }

  getPasswordMatchError() {
    if (!this.confirmPassword.dirty && this.password.value === '') {
      this.confirmPassword.markAsPristine();
      this.confirmPassword.markAsUntouched();
      if (this.confirmPassword.value === '') {
        this.password.markAsPristine();
        this.password.markAsUntouched();
      }
      return null;
    }
    return this.getErrorMessage(this.confirmPassword, {
      passwordMismatch: 'Passwords do not match',
    });
  }

  openSnackBar(mensaje: string, accion: string) {
    this!.snackBarRef = this!._snackBar.open(mensaje, accion, {
      duration: 5000, // KYBC.Time in milliseconds.
      verticalPosition: 'bottom', // KYBC.Posible values: 'top' | 'bottom'.
      horizontalPosition: 'center', // KYBC.Posible values: 'start' | 'center' | 'end' | 'left' | 'right'.
    });
  }

  updateUser() {
    this.name.markAsTouched();
    this.email.markAsTouched();
    this.oldPassword.markAsTouched();
    this._accountService
      .checkPassword(this._storage.get('userId'), this.oldPassword.value!)
      .subscribe({
        next: async () => {
          let updatedPasswordIsValid = true;
          this.password.markAsTouched();
          this.confirmPassword.markAsTouched();
          if (this.password.value !== '' || this.confirmPassword.value !== '') {
            updatedPasswordIsValid =
              this.password.valid && this.confirmPassword.valid;
          }

          const userInfoIsValid = this.name.valid && this.email.valid;

          if (userInfoIsValid && updatedPasswordIsValid) {
            const updatedUser = {
              id: this._storage.get('userId'),
              name: this.name.value,
              password:
                this.password.value !== '' && this.confirmPassword.value !== ''
                  ? this.password.value
                  : this.oldPassword.value,
              email: this.email.value,
              role: this._storage.get('isAdmin') === 'true' ? 'Admin' : 'User',
            } as IUserRegisterRequestDto;

            this._accountService
              .updateUser(this._storage.get('userId')!, updatedUser)
              .subscribe({
                next: () => {
                  this._storage.set('name', this.email.value!);
                },
                error: () => {
                  this.email.setErrors({ emailExist: true });
                  this.email.markAsTouched();
                },
              });
            this.oldPassword.reset();
            this.password.reset();
            this.confirmPassword.reset();
            this.openSnackBar('Account user data updated!', '');
            this.snackBarRef.afterDismissed().subscribe(() => {
              this.router.navigateByUrl('/items');
            });
          }
        },
        error: () => {
          this.oldPassword.setErrors({ passwordWrong: true });
          this.oldPassword.markAsTouched();
        },
      });
  }

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
                  this._snackBar.open(
                    `Something went wrong. Try again later.`,
                    '',
                    {
                      duration: 10000,
                    }
                  );
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
