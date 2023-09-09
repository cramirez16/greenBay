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

  bidItem(bid: number) {
    const bidRequestDto: IBidRequestDto = {
      bidAmount: bid,
      biderId: this.storage.get('userId'),
      itemId: this.storage.get('itemId'),
    };

    return this.http.post<IBidResponseDto>(this.baseURL, bidRequestDto);
  }
}
