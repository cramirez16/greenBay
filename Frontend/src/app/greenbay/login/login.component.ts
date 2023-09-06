import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { IUserLoginResponseDto } from '../models/IUserLoginResponseDto';
import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { FormlyFormOptions, FormlyFieldConfig } from '@ngx-formly/core';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserLoginRequestDto } from '../models/IUserLoginRequestDto';
import { of } from 'rxjs';
import { MessagePopupComponent } from '../message-popup/message-popup.component';
import { MatDialog } from '@angular/material/dialog';
import { UserValidationService } from '../../services/user-validation.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm = new FormGroup({});
  loginModel: any = {};
  loginFields: FormlyFieldConfig[];

  constructor(
    private accountService$: AccountService,
    private router: Router,
    private dialog: MatDialog,
    private userValidation: UserValidationService,
    private jwtDecoder: JwtDecoderService
  ) {
    this.loginForm = new FormGroup({});
    this.loginModel = {};
    this.loginFields = [
      {
        key: 'email',
        type: 'input',
        props: {
          label: 'E-mail',
          required: true,
        },
        validators: {
          email: {
            expression: this.userValidation.validEmailLogin,
            message: (error: any, field: FormlyFieldConfig) =>
              `"${field.formControl!.value}" is not a valid email Address`,
          },
        },
        validation: {
          messages: {
            required: `Please enter your email`,
          },
        },
      },
      {
        key: 'password',
        type: 'input',
        props: {
          type: 'password',
          label: 'Password',
          required: true,
        },
        validation: {
          messages: {
            required: `Please enter your password`,
          },
        },
      },
    ];
  }

  submit(model: IUserLoginRequestDto) {
    if (this.loginForm.valid) {
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
