import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { UserValidationService } from '../../services/user-validation.service';
import { IUserLoginRequestDto } from '../models/IUserLoginRequestDto';
import { BannerComponent } from '../banner/banner.component';
import { IUserLoginResponseDto } from '../models/IUserLoginResponseDto';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MessagePopupComponent } from '../message-popup/message-popup.component';

@Component({
  selector: 'app-loginew',
  templateUrl: './loginew.component.html',
  styleUrls: ['./loginew.component.css'],
})
export class LoginewComponent {
  email = new FormControl('', [
    Validators.required,
    this.userValidator.validEmail,
  ]);
  password = new FormControl('', [
    Validators.required,
    this.userValidator.validPassword,
  ]);

  constructor(
    private accountService$: AccountService,
    private jwtDecoder: JwtDecoderService,
    private userValidator: UserValidationService,
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
  //----------------------
  sendForm() {
    this.email.markAsTouched();
    this.password.markAsTouched();

    const formIsValid = this.email.valid && this.password.valid;

    if (formIsValid) {
      const model: IUserLoginRequestDto = {
        email: this.email.value!,
        password: this.password.value!,
      };
      this.accountService$.login(model).subscribe({
        next: (response: IUserLoginResponseDto) => {
          this.jwtDecoder.decode(response.tokenKey);
          this.router.navigate(['/']);
        },
        error: (response: HttpErrorResponse) => {
          const dialogData = {
            title: '',
            description: '',
          };
          // if (response.error.emailNotValidated) {
          //   dialogData.title = 'Login failed';
          //   dialogData.description =
          //     'Email not confirmed yet, please check you email account.';
          // } else {
          dialogData.title = response.status === 0 ? '' : 'Login failed';
          dialogData.description =
            response.status === 0 ? 'Network Error' : 'Wrong credentials';
          // }
          this.dialog.open(MessagePopupComponent, {
            data: dialogData,
          });
        },
      });
    }
  }
}
