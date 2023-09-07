import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MessagePopupComponent } from '../message-popup/message-popup.component';
import { IItemResponseDto } from '../models/IItemResponseDto';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: IItemResponseDto[] | undefined;

  constructor(
    public cartService: CartService,
    private Router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.listCart();
  }

  listCart() {
    this.cartService.listCart().subscribe({
      next: (response: IItemResponseDto[]) => {
        this.cartItems = response;
      },
      error: (error: any) => console.log(error),
    });
  }

  checkoutCart() {
    this.cartService.checkoutCart().subscribe({
      next: () => {
        this.Router.navigate(['/']);
        this.dialog.open(MessagePopupComponent, {
          data: {
            title: `Successfully purchased!`,
            description: `You will get a receipt email!`,
          },
        });
      },
      error: (error: any) => console.log(error),
    });
  }
}
