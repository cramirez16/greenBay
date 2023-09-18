import { TestBed } from '@angular/core/testing';
import { UserValidationService } from './user-validation.service';
import { HttpClientModule } from '@angular/common/http';

describe('UserValidationService', () => {
  let service: UserValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    });
    service = TestBed.inject(UserValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
