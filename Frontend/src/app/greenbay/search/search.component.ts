import { Component } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { ItemValidationService } from '../../services/item-validation.service';
import { MatDialog } from '@angular/material/dialog';
import { BannerBidComponent } from '../banner-bid/banner-bid.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class SearchComponent {
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
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];
  gridCols: number = 3;

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
            bidAmount: parseInt(this.searchId.value!),
            message: 'Item not found!',
          });
        }
      },
    });
  }

  bannerItem(bid: { bidAmount: number; message: string }) {
    this.dialog.open(BannerBidComponent, {
      data: bid,
    });
  }

  onBackClick() {
    this.router.navigate(['items']);
  }
}
