import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css'],
  standalone: true,
  imports: [MatTableModule, MaterialModule],
})
export class ItemsComponent implements OnInit {
  itemsListComponent: IItemResponseDto[] = [];
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];

  constructor(
    private itemService: ItemService,
    private cartService: CartService,
    public accountService: AccountService
  ) {}
  ngOnInit() {
    this.itemService.itemsListService.subscribe(
      (itemsListResponse: IItemResponseDto[]) => {
        this.itemsListComponent = itemsListResponse;
      }
    );
    this.itemService.getItems();
  }

  addToCart(itemId: number) {
    this.cartService.addItem(itemId);
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
