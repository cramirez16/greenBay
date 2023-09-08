import { Component, SecurityContext } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { IItemRequestDto } from '../models/IItemRequestDto';
import { Router } from '@angular/router';
import { DateRegulatorService } from '../../services/date-regulator.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-create-item',
  templateUrl: './create-item.component.html',
  styleUrls: ['./create-item.component.css'],
})
export class CreateItemComponent {
  isImageLoaded: boolean = false;
  timeStamp: string = new Date().toUTCString();
  item: IItemRequestDto = {
    name: '',
    description: '',
    photoUrl: '',
    price: 0,
    bid: 0,
    isSellable: true,
    sellerId: 0,
    creationDate: this.timeStamp,
    updateDate: this.timeStamp,
  };

  constructor(
    private router: Router,
    private itemService: ItemService,
    private dateService: DateRegulatorService,
    private localStorageService: LocalStorageService,
    private sanitizer: DomSanitizer
  ) {}

  onSubmit() {
    this.item.creationDate = this.dateService.convertDate(
      this.item.creationDate
    );
    this.item.updateDate = this.item.creationDate;
    this.item.sellerId = this.localStorageService.get('userId');
    this.itemService.createItem(this.item).subscribe({
      next: () => {
        console.log('Item created successfully');
        this.navigateToLandingPage();
      },
      error: (error) => console.log('Error creating ticket:', error),
    });
  }

  navigateToLandingPage() {
    this.router.navigateByUrl('/items');
  }

  onUrlChange() {
    const safeUrl: SafeUrl = this.sanitizer.bypassSecurityTrustUrl(
      this.item.photoUrl
    );

    if (this.item.photoUrl !== '') {
      // Convert the SafeUrl to a string
      this.item.photoUrl =
        this.sanitizer.sanitize(SecurityContext.URL, safeUrl) || '';
      this.isImageLoaded = true;
    } else {
      this.isImageLoaded = false;
    }
  }
}
