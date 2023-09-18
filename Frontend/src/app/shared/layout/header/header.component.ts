import { Component } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  constructor(public accountService: AccountService) {}
}
