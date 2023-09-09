import { TestBed } from '@angular/core/testing';

import { ItemValidationService } from './item-validation.service';

describe('ItemValidationService', () => {
  let service: ItemValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
