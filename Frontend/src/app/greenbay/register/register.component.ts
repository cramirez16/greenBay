import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { UserValidationService } from '../../services/user-validation.service';
import { IUserRegisterRequestDto } from '../models/IUserRegisterRequestDto';
import { BannerComponent } from '../banner/banner.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  name = new FormControl('', [
    Validators.required,
    this.userValidator.validName,
  ]);
  email = new FormControl('', [
    Validators.required,
    this.userValidator.validEmail,
  ]);
  password = new FormControl('', [
    Validators.required,
    this.userValidator.validPassword,
  ]);
  confirmPassword = new FormControl('', [
    Validators.required,
    this.userValidator.validPasswordMatch(this.password),
  ]);

  constructor(
    private userValidator: UserValidationService,
    private router: Router,
    private accountService: AccountService,
    private dialog: MatDialog
  ) {
    this.password.valueChanges.subscribe(() => {
      this.confirmPassword.updateValueAndValidity();
    });
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

  getNameError() {
    return this.getErrorMessage(this.name, {
      required: 'You must enter your name',
      nameLength: 'The name must contain at least 3 characters',
      nameExist: 'The name already exist',
    });
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

  getPasswordMatchError() {
    return this.getErrorMessage(this.confirmPassword, {
      required: 'You must re-enter the password',
      passwordMismatch: 'Passwords do not match',
    });
  }

  sendForm() {
    this.name.markAsTouched();
    this.email.markAsTouched();
    this.password.markAsTouched();
    this.confirmPassword.markAsTouched();

    const formIsValid =
      this.name.valid &&
      this.email.valid &&
      this.password.valid &&
      this.confirmPassword.valid;

    if (formIsValid) {
      const newUserRegister = {
        name: this.name.value,
        password: this.password.value,
        email: this.email.value,
      } as IUserRegisterRequestDto;
      this.accountService.register(newUserRegister).subscribe({
        next: (response: any) => {
          this.bannerUser(response);
          this.router.navigateByUrl('login');
        },
        error: (response) => {
          if (response.error.takenEmail) {
            this.email.setErrors({ emailExist: true });
            this.email.markAsTouched();
          }
        },
      });
    }
  }

  navigateToLandingPage() {
    this.router.navigateByUrl('/');
  }

  bannerUser(user: IUserRegisterRequestDto) {
    this.dialog.open(BannerComponent, {
      data: user,
    });
  }
}
