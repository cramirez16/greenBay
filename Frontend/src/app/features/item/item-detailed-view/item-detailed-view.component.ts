import { Component, OnInit } from '@angular/core';
import { IItemResponseDto } from '../../../core/models/IItemResponseDto';
import { ItemService } from '../../../core/services/item.service';
import { MaterialModule } from '../../../shared/material/material.module';
import { BidService } from '../../../core/services/bid.service';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { ItemValidationService } from '../../../core/services/item-validation.service';
import { MatDialog } from '@angular/material/dialog';
import { GenericBannerComponent } from '../../generic-banner/generic-banner.component';

@Component({
  selector: 'app-item-detailed-view',
  templateUrl: './item-detailed-view.component.html',
  styleUrls: ['./item-detailed-view.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class ItemDetailedViewComponent implements OnInit {
  bid = new FormControl('', [
    Validators.required,
    this._itemValidator.validPrice,
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
    private _itemService: ItemService,
    private _bidService: BidService,
    private _localStorage: LocalStorageService,
    private _router: Router,
    private _itemValidator: ItemValidationService,
    private _dialog: MatDialog
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

  getBidError() {
    return this.getErrorMessage(this.bid, {
      required: 'You must enter a bid',
      bidInvalid: 'Not a valid bid, only bigger than zero allowed.',
    });
  }

  ngOnInit() {
    this.itemId = this._localStorage.get('itemId');
    this._itemService.getItemById(this.itemId).subscribe({
      next: (response: any) => {
        this.item = response as IItemResponseDto;
      },
      error: (e) => {
        console.log(e.error);
      },
    });
  }

  bidItem() {
    this.bid.markAsTouched();

    if (!this.bid.valid) return;

    this.item.bid = Number(this.bid.value!);

    this._bidService.bidItem(this.item.bid).subscribe({
      next: (response: any) => {
        if (response.bidLow) {
          this.bannerItem({
            bidAmount: this.item.bid,
            message: 'Bid too Low!',
          });
          this.onBackClick();
        }
        if (response.bidSuccess) {
          this.bannerItem({
            bidAmount: this.item.bid,
            message: 'Bid done!',
          });
          this.onBackClick();
        }
        if (response.itemSold) {
          this.bannerItem({
            bidAmount: this.item.bid,
            message: 'Congratulations, you bought the item!',
          });
          this._localStorage.set('money', `${response.userMoney}`);
          this.onBackClick();
        }
      },
      error: (error) => {
        if (error.error.notEnoughtMoneyToBid) {
          this.bannerItem({
            bidAmount: this.item.bid,
            message: 'Insufficient funds!',
          });
          console.log('Error creating ticket:', error);
          this.onBackClick();
        }
      },
    });
  }

  bannerItem(bid: { bidAmount: number; message: string }) {
    const message1 = `Bid amount: ${bid.bidAmount}\n`;
    const message2 = `${bid.message}\n`;
    this._dialog.open(GenericBannerComponent, {
      data: { titleSection: message1, messageSection: message2 },
    });
  }

  onBackClick() {
    this._router.navigate(['items']);
  }
}
