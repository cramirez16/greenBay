import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { IItemResponseDto } from '../../core/models/IItemResponseDto';
import { BehaviorSubject } from 'rxjs';
import { IItemRequestDto } from '../../core/models/IItemRequestDto';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  baseURL = `${environment.SERVER_URL}/api/item/`;
  // rxjs ---> BehaviorSubjects
  // Subject type, has initial value [],
  // subscribers will receive the last emmited value upon subscription.
  // itemsListService = new BehaviorSubject<IItemResponseDto[]>([]);
  itemsListService = new BehaviorSubject<{
    itemsPaginated: IItemResponseDto[];
    totalElements: number;
  }>({ itemsPaginated: [], totalElements: 0 });
  error = new BehaviorSubject<any>({});
  constructor(private http: HttpClient) {}

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

  getItemsPaginated(pageNumber: number, pageSize: number) {
    this.http
      .get<any>(
        `${this.baseURL}paginated?PageNumber=${pageNumber}&PageSize=${pageSize}`
      )
      .subscribe({
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

  getItemById(id: string) {
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
