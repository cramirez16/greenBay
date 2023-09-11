import { Component, OnInit } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { BidService } from 'src/app/services/bid.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { ItemValidationService } from '../../services/item-validation.service';
import { MatDialog } from '@angular/material/dialog';
import { BannerBidComponent } from '../banner-bid/banner-bid.component';

@Component({
  selector: 'app-detailed-view',
  templateUrl: './detailed-view.component.html',
  styleUrls: ['./detailed-view.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class DetailedViewComponent implements OnInit {
  bid = new FormControl('', [
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
    private bidService: BidService,
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

  getBidError() {
    return this.getErrorMessage(this.bid, {
      required: 'You must enter a bid',
      bidInvalid: 'Not a valid bid, only bigger than zero allowed.',
    });
  }

  ngOnInit() {
    this.itemId = this._localStorage.get('itemId');
    this.itemService.getItemById(this.itemId).subscribe({
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

    this.bidService.bidItem(this.item.bid).subscribe({
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
          console.log(response);
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
    this.dialog.open(BannerBidComponent, {
      data: bid,
    });
  }

  onBackClick() {
    this.router.navigate(['items']);
  }
}
