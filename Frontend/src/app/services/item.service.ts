import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IItemResponseDto } from '../greenbay/models/IItemResponseDto';
import { BehaviorSubject, of, Observable } from 'rxjs';
import { DateRegulatorService } from './date-regulator.service';
import { IItemRequestDto } from '../greenbay/models/IItemRequestDto';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  baseURL = `${environment.SERVER_URL}/api/item/`;
  itemsListService = new BehaviorSubject<IItemResponseDto[]>([]);
  error = new BehaviorSubject<any>({});
  constructor(
    private http: HttpClient,
    private dateRegulatorService: DateRegulatorService
  ) {}

  getItems() {
    this.http.get<any>(this.baseURL).subscribe({
      next: (a) => {
        this.itemsListService.next(a);
      },
      error: (e) => {
        this.error.next(e);
      },
    });
  }

  createItem(itemRequestDTO: IItemRequestDto) {
    return this.http.post(this.baseURL, itemRequestDTO);
  }

  getItemtById(id: string) {
    return this.http.get(this.baseURL + id);
  }

  updateItem(id: string, data: IItemRequestDto) {
    return this.http.put(this.baseURL + id, data);
  }
  deleteItem(itemId: number) {
    const deleteUrl = `${this.baseURL}${itemId}`;
    return this.http.delete(deleteUrl);
  }
}
