import { Injectable, computed, signal } from '@angular/core';
import { ItemService } from './item.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { IBidResponseDto } from '../greenbay/models/IBidResponseDto';
import { IBidRequestDto } from '../greenbay/models/IBidRequestDto';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class BidService {
  baseURL = `${environment.SERVER_URL}/api/bid/`;
  bidResponse = new BehaviorSubject<IBidResponseDto[]>([]);

  constructor(private http: HttpClient, public storage: LocalStorageService) {}

  bidItem(itemId: number) {
    const bidRequestDto: IBidRequestDto = {
      bidAmount: 10,
      biderId: this.storage.get('userId'),
      itemId: itemId,
    };

    this.http.post<IBidResponseDto>(this.baseURL, bidRequestDto).subscribe({
      next: (response) => {
        console.log(response);
        this.bidResponse.next([response]);
      },
      error: (error) => {
        console.log(error.error);
      },
    });
  }
}
