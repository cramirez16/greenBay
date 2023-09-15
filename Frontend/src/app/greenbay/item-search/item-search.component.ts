import { Component } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from '../../material/material.module';
import { LocalStorageService } from '../../services/local-storage.service';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { ItemValidationService } from '../../services/item-validation.service';
import { MatDialog } from '@angular/material/dialog';
import { GenericBannerComponent } from '../generic-banner/generic-banner.component';

@Component({
  selector: 'app-item-search',
  templateUrl: './item-search.component.html',
  styleUrls: ['./item-search.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class ItemSearchComponent {
  searchId = new FormControl('', [
    Validators.required,
    this.itemValidator.validPrice,
  ]);
  // Store items list component which we can display after get the list from the server.
  item: IItemResponseDto = {
    id: 0,
    name: '',
    description: '',
    photoUrl: '',
    bid: 0,
    price: 0,
    isSellable: false,
    creationDate: '',
    updateDate: '',
    sellerId: 0,
    sellerName: '',
    buyerId: 0,
    bids: [],
  };
  itemId: string = '';

  constructor(
    private itemService: ItemService,
    public accountService: AccountService,
    public _localStorage: LocalStorageService,
    private router: Router,
    private itemValidator: ItemValidationService,
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

  getSearchIdError() {
    return this.getErrorMessage(this.searchId, {
      required: 'You must enter an id',
      bidInvalid: 'Invalid id, only integers and positive values are allowed.',
    });
  }

  searchById() {
    this.searchId.markAsTouched();
    if (!this.searchId.valid) return;

    this.itemService.getItemById(this.searchId.value!).subscribe({
      next: (response: any) => {
        console.log(response);
        this.item = response as IItemResponseDto;
      },
      error: (error) => {
        if (error.error.itemNotFound) {
          this.bannerItem({
            itemId: parseInt(this.searchId.value!),
            message: 'Item not found!',
          });
        }
      },
    });
  }

  bannerItem(templateText: { itemId: number; message: string }) {
    const message1 = `Id = \n${templateText.itemId}\n`;
    const message2 = `${templateText.message}\n`;
    this.dialog.open(GenericBannerComponent, {
      data: { titleSection: message1, messageSection: message2 },
    });
  }

  onBackClick() {
    this.router.navigate(['items']);
  }
}
