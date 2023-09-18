import { Component, SecurityContext } from '@angular/core';
import { ItemService } from '../../../core/services/item.service';
import { IItemRequestDto } from '../../../core/models/IItemRequestDto';
import { Router } from '@angular/router';
import { DateRegulatorService } from '../../../core/services/date-regulator.service';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ItemValidationService } from '../../../core/services/item-validation.service';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { GenericBannerComponent } from '../../generic-banner/generic-banner.component';

@Component({
  selector: 'app-item-create',
  templateUrl: './item-create.component.html',
  styleUrls: ['./item-create.component.css'],
})
export class ItemCreateComponent {
  title = new FormControl('', [
    Validators.required,
    this.itemValidator.validText,
  ]);
  description = new FormControl('', [
    Validators.required,
    this.itemValidator.validText,
  ]);
  photoUrl = new FormControl('', [
    Validators.required,
    this.itemValidator.validUrl,
  ]);

  price = new FormControl('', [
    Validators.required,
    this.itemValidator.validPrice,
  ]);

  isImageLoaded: boolean = false;
  timeStamp: string = new Date().toUTCString();
  item: IItemRequestDto = {
    name: '',
    description: '',
    photoUrl: '',
    price: 0,
    bid: 0,
    isSellable: true,
    sellerId: 0,
    creationDate: this.timeStamp,
    updateDate: this.timeStamp,
  };

  constructor(
    private itemValidator: ItemValidationService,
    private router: Router,
    private itemService: ItemService,
    private dateService: DateRegulatorService,
    private localStorageService: LocalStorageService,
    private sanitizer: DomSanitizer,
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

  getTitleError() {
    return this.getErrorMessage(this.title, {
      required: 'You must enter a title',
      nameLength: 'The title must contain at least 3 characters',
      nameExist: 'The title already exist',
    });
  }

  getDescriptionError() {
    return this.getErrorMessage(this.description, {
      required: 'You must enter a description',
      descriptionLength: 'The description must contain at least 3 characters',
      descriptionExist: 'The description already exist',
    });
  }

  getPhotoUrlError() {
    return this.getErrorMessage(this.photoUrl, {
      required: 'You must enter your photo url',
      photoUrlInvalid: 'Not a valid url address',
      photoUrlExist: 'The photo url already exists',
    });
  }

  getPriceError() {
    return this.getErrorMessage(this.price, {
      required: 'You must enter a price',
      priceInvalid: 'Not a valid price, only bigger than zero allowed.',
    });
  }

  onSubmit() {
    this.title.markAsTouched();
    this.description.markAsTouched();
    this.photoUrl.markAsTouched();
    this.price.markAsTouched();

    const formIsValid =
      this.title.valid &&
      this.description.valid &&
      this.photoUrl.valid &&
      this.price.valid;

    if (!formIsValid) return;

    this.item.name = this.title.value!;
    this.item.description = this.description.value!;
    this.item.photoUrl = this.photoUrl.value!;
    this.item.price = Number(this.price.value!);
    this.item.creationDate = this.dateService.convertDate(
      this.item.creationDate
    );
    this.item.updateDate = this.item.creationDate;
    this.item.sellerId = this.localStorageService.get('userId');
    this.itemService.createItem(this.item).subscribe({
      next: (response: any) => {
        this.bannerItem(response);
        this.navigateToLandingPage();
      },
      error: (error) => console.log('Error creating ticket:', error),
    });
  }

  bannerItem(item: IItemRequestDto) {
    const message1 = `You just have been add a new item\ntitle: ${item.name}\n`;
    const message2 = `${item.description}\nto GreenBay Service!\nThank you for add a new item!\n`;
    this.dialog.open(GenericBannerComponent, {
      data: { titleSection: message1, messageSection: message2 },
    });
  }

  navigateToLandingPage() {
    this.router.navigateByUrl('/items');
  }

  onUrlChange() {
    const safeUrl: SafeUrl = this.sanitizer.bypassSecurityTrustUrl(
      this.photoUrl.value!
    );

    if (this.photoUrl.value! !== '') {
      // Convert the SafeUrl to a string
      this.photoUrl.setValue(
        this.sanitizer.sanitize(SecurityContext.URL, safeUrl) || ''
      );

      this.isImageLoaded = true;
    } else {
      this.isImageLoaded = false;
    }
  }
}
