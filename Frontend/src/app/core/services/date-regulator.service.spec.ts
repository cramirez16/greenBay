import { TestBed } from '@angular/core/testing';

import { DateRegulatorService } from './date-regulator.service';

describe('DateRegulatorService', () => {
  let service: DateRegulatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DateRegulatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
