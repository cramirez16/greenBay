import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { UserValidationService } from '../../services/user-validation.service';
import { IUserLoginRequestDto } from '../models/IUserLoginRequestDto';
import { IUserLoginResponseDto } from '../models/IUserLoginResponseDto';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MessagePopupComponent } from '../message-popup/message-popup.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  email = new FormControl('', [
    Validators.required,
    this._userValidator.validEmail,
  ]);
  password = new FormControl('', [
    Validators.required,
    this._userValidator.validPassword,
  ]);

  constructor(
    private _accountService: AccountService,
    private _jwtDecoder: JwtDecoderService,
    private _userValidator: UserValidationService,
    private _router: Router,
    private _dialog: MatDialog
  ) {}

  submit() {
    this.email.markAsTouched();
    this.password.markAsTouched();

    const formIsValid = this.email.valid && this.password.valid;

    if (formIsValid) {
      const model: IUserLoginRequestDto = {
        email: this.email.value!,
        password: this.password.value!,
      };
      //this.accountService$ typeof Observable<IUserLoginResponseDto>
      // await this.accountService$.login(parameter1,...) Promises<IUserLoginResponseDto>
      this._accountService.login(model).subscribe({
        next: (response: IUserLoginResponseDto) => {
          // response JsonString deserialize into IUserLoginResponseDto object type.
          // decode the jwt string and store the claims into browser localStorage
          this._jwtDecoder.decode(response.tokenKey);
          this._router.navigate(['items']);
        },
        error: (response: HttpErrorResponse) => {
          const dialogData = {
            title: '',
            description: '',
          };
          dialogData.title = response.status === 0 ? '' : 'Login failed';
          dialogData.description =
            response.status === 0 ? 'Network Error' : 'Wrong credentials';
          // }
          this._dialog.open(MessagePopupComponent, {
            data: dialogData,
          });
        },
      });
    }
  }

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

  getEmailError() {
    return this.getErrorMessage(this.email, {
      required: 'You must enter your email address',
      emailInvalid: 'Not a valid email address',
      emailExist: 'The email already exists',
    });
  }

  getPasswordError() {
    return this.getErrorMessage(this.password, {
      required: 'You must enter a password',
      passwordLength: 'Password must contain at least 6 characters',
      passwordLetter: 'Password must contain a letter',
      passwordNumber: 'Password must contain a number',
      passwordCase: 'Password must contain upper and lower case',
    });
  }
}
