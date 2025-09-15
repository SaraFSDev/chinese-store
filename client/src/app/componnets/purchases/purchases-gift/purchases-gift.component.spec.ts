import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasesGiftComponent } from './purchases-gift.component';

describe('PurchasesGiftComponent', () => {
  let component: PurchasesGiftComponent;
  let fixture: ComponentFixture<PurchasesGiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PurchasesGiftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PurchasesGiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
