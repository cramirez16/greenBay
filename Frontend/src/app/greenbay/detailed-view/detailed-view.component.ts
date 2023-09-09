import { Component, OnInit, HostListener } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { CartService } from 'src/app/services/cart.service';
import { BidService } from 'src/app/services/bid.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-detailed-view',
  templateUrl: './detailed-view.component.html',
  styleUrls: ['./detailed-view.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class DetailedViewComponent implements OnInit {
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
    bidsList: [],
  };
  itemId: string = '';
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];
  gridCols: number = 3;

  constructor(
    private itemService: ItemService,
    private bidService: BidService,
    private cartService: CartService,
    public accountService: AccountService,
    public _localStorage: LocalStorageService
  ) {}
  ngOnInit() {
    this.itemId = this._localStorage.get('itemId');
    this.itemService.getItemtById(this.itemId).subscribe({
      next: (response: any) => {
        this.item = response as IItemResponseDto;
        console.log(this.item);
      },
      error: (e) => {
        console.log(e.error);
      },
    });
  }

  // addToCart(itemId: number) {
  //   this.cartService.addItem(itemId);
  // }

  bidItem(itemId: string, bid: number) {
    this.bidService.bidItem(parseInt(itemId), bid);
  }

  onDeleteTicket(itemId: number, bid: number) {
    this.itemService.deleteItem(itemId).subscribe({
      next: () => {
        this.itemService.getItems();
      },
      error: (error: any) => console.log(error),
    });
  }
}
