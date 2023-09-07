import { TestBed } from '@angular/core/testing';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from './account.service';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

describe('AccountService', () => {
  let service: AccountService;
  let mockMatDialog: MatDialog;
  let mockMatSnackBar: MatSnackBar;

  beforeEach(() => {
    mockMatDialog = {
      open: jest.fn(),
    } as unknown as MatDialog;

    mockMatSnackBar = {
      open: jest.fn(),
    } as unknown as MatSnackBar;

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [
        { provide: MatDialog, useValue: mockMatDialog },
        { provide: MatSnackBar, useValue: mockMatSnackBar },
      ],
    });
    service = TestBed.inject(AccountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
