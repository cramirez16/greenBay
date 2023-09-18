import { TestBed } from '@angular/core/testing';
import { BidService } from './bid.service';
import { HttpClientModule } from '@angular/common/http';

describe('BidService', () => {
  let service: BidService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    });
    service = TestBed.inject(BidService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
