import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ItemValidationService {
  constructor() {}

  validText(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const nameLength = /^.{3,255}$/;

    if (!nameLength.test(value)) {
      return { nameLength: true };
    }
    return null;
  }

  validUrl(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const urlRegex =
      /^(http(s):\/\/.)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)$/;

    if (!urlRegex.test(value)) {
      return { urlInvalid: true };
    }
    return null;
  }

  validPrice(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value <= 0) {
      return { priceInvalid: true };
    }
    return null;
  }
}
