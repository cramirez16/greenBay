import { Component, OnInit, HostListener } from '@angular/core';
import { IItemResponseDto } from '../models/IItemResponseDto';
import { AccountService } from '../../services/account.service';
import { ItemService } from '../../services/item.service';
import { MaterialModule } from 'src/app/material/material.module';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Router } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';

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
  userMoney: number = this._localStorage.get('money');
  pageNumber: number = 1;
  pageSize: number = 1;
  totalElements!: number;
  pageEvent: PageEvent = new PageEvent();
  // grid parameters.
  displayedColumns: string[] = ['name', 'description', 'photoUrl'];
  gridCols: number = 3;

  constructor(
    private itemService: ItemService,
    public accountService: AccountService,
    private _localStorage: LocalStorageService,
    private router: Router
  ) {}
  ngOnInit() {
    this.updateGridParameters();
    // listen for get list success/error ( in the service )
    this.itemService.itemsListService.subscribe(
      (itemsListResponse: {
        itemsPaginated: IItemResponseDto[];
        totalElements: number;
      }) => {
        this.itemsListComponent = itemsListResponse.itemsPaginated.filter(
          (item) => item.isSellable
        );
        this.totalElements = itemsListResponse.totalElements;
        this.userMoney = this._localStorage.get('money');
      }
    );
    // Calls getItems function in service to trigger get method.
    // (pageNumber, pageSize)
    this.itemService.getItemsPaginated(this.pageNumber, this.pageSize);
  }

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

  handlePageEvent(e: PageEvent) {
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageNumber = e.pageIndex + 1;
    this.updateGridParameters();
    this.itemService.getItemsPaginated(this.pageNumber, this.pageSize);
  }
}
