import { Component, OnInit, HostListener } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { CartService } from 'src/app/services/cart.service';
import { BidService } from 'src/app/services/bid.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css'],
  standalone: true,
  imports: [MaterialModule],
})
export class ItemsComponent implements OnInit {
  // Store items list component which we can display after get the list from the server.
  itemsListComponent: IItemResponseDto[] = [];
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];
  gridCols: number = 3;

  constructor(
    private itemService: ItemService,
    private bidService: BidService,
    private cartService: CartService,
    public accountService: AccountService,
    private _localStorage: LocalStorageService,
    private router: Router
  ) {}
  ngOnInit() {
    this.updateGridParameters();
    // listen for get list success/error ( in the service )
    this.itemService.itemsListService.subscribe(
      (itemsListResponse: IItemResponseDto[]) => {
        this.itemsListComponent = itemsListResponse.filter(
          (item) => item.isSellable
        );
      }
    );
    // Calls getItems function in service to trigger get method.
    this.itemService.getItems();
  }

  // addToCart(itemId: number) {
  //   this.cartService.addItem(itemId);
  // }

  onDetailedView(itemId: number) {
    this._localStorage.set('itemId', itemId);
    this.router.navigate(['detailed-view']);
  }

  onDeleteTicket(itemId: number) {
    this.itemService.deleteItem(itemId).subscribe({
      next: () => {
        this.itemService.getItems();
      },
      error: (error: any) => console.log(error),
    });
  }
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.updateGridParameters();
  }
  updateGridParameters() {
    if (window.innerWidth < 780) {
      this.gridCols = 1; // Adjust the number of columns for smaller screens
    } else if (window.innerWidth < 1220) {
      this.gridCols = 2; // Adjust the number of columns for medium screens
    } else {
      this.gridCols = 3; // Default number of columns for larger screens
    }
  }
}
