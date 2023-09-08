import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { CartService } from 'src/app/services/cart.service';
import { BidService } from 'src/app/services/bid.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css'],
  standalone: true,
  imports: [MatTableModule, MaterialModule],
})
export class ItemsComponent implements OnInit {
  // Store items list component which we can display after get the list from the server.
  itemsListComponent: IItemResponseDto[] = [];
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];

  constructor(
    private itemService: ItemService,
    private bidService: BidService,
    private cartService: CartService,
    public accountService: AccountService
  ) {}
  ngOnInit() {
    // listen for get list success/error ( in the service )
    this.itemService.itemsListService.subscribe(
      (itemsListResponse: IItemResponseDto[]) => {
        this.itemsListComponent = itemsListResponse;
      }
    );
    // Calls getItems function in service to trigger get method.
    this.itemService.getItems();
  }

  // addToCart(itemId: number) {
  //   this.cartService.addItem(itemId);
  // }

  bidItem(itemId: number) {
    this.bidService.bidItem(itemId);
  }

  onDeleteTicket(itemId: number) {
    this.itemService.deleteItem(itemId).subscribe({
      next: () => {
        this.itemService.getItems();
      },
      error: (error: any) => console.log(error),
    });
  }
}
