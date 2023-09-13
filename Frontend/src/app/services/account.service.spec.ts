import { TestBed } from '@angular/core/testing';
import { AccountService } from './account.service';
import { HttpClientModule } from '@angular/common/http';

describe('AccountService', () => {
  let service: AccountService;
  // let mockMatDialog: MatDialog;
  // let mockMatSnackBar: MatSnackBar;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      // providers: [
      //   { provide: MatDialog, useValue: mockMatDialog },
      //   { provide: MatSnackBar, useValue: mockMatSnackBar },
      // ],
    });
    service = TestBed.inject(AccountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
