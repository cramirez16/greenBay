import { Injectable } from '@angular/core';
import { AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class UserValidationService {
  constructor() {}

  validName(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const nameLength = /^.{3,255}$/;

    if (!nameLength.test(value)) {
      return { nameLength: true };
    }
    return null;
  }

  validEmail(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailRegex.test(value)) {
      return { emailInvalid: true };
    }
    return null;
  }

  validPassword(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const passwordLetter = /[a-zA-Z]+/;
    const passwordNumber = /\d+/;
    const passwordLetterCase = /(?=.*[a-z])(?=.*[A-Z])/;
    const passwordLength = /^.{6,255}$/;

    if (!passwordLetter.test(value)) {
      return { passwordLetter: true };
    }
    if (!passwordNumber.test(value)) {
      return { passwordNumber: true };
    }
    if (!passwordLetterCase.test(value)) {
      return { passwordCase: true };
    }
    if (!passwordLength.test(value)) {
      return { passwordLength: true };
    }
    return null;
  }

  validPasswordMatch(otherControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.value;
      const confirmPassword = otherControl.value;

      if (
        password !== confirmPassword ||
        password === '' ||
        confirmPassword === ''
      ) {
        return { passwordMismatch: true };
      }
      return null;
    };
  }
}
