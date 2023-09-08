import { Injectable, computed, signal } from '@angular/core';
import { ItemService } from './item.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { IItemResponseDto } from '../greenbay/models/IItemResponseDto';
import { IItemRequestDto } from '../greenbay/models/IItemRequestDto';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseURL = `${environment.SERVER_URL}/api/Cart/`;
  cartRequest = new BehaviorSubject<IItemRequestDto[]>([]);
  cartResponse = new BehaviorSubject<IItemResponseDto[]>([]);
  cartItemCount: number = 0;

  constructor(private http: HttpClient) {}

  addItem(itemId: number) {
    const url = `${this.baseURL}add/${itemId}`;
    this.http.post<IItemRequestDto>(url, null).subscribe({
      next: (response) => {
        this.cartRequest.next(this.cartRequest.value.concat(response));
        this.cartItemCount++;
      },
      error: () => {},
    });
  }

  listCart() {
    return this.http.get<IItemResponseDto[]>(this.baseURL);
  }

  checkoutCart(): Observable<void> {
    this.cartItemCount = 0;
    return this.http.get<void>(`${environment.SERVER_URL}/checkout`);
  }
}
