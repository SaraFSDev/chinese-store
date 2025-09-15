import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasesUsersComponent } from './purchases-users.component';

describe('PurchasesUsersComponent', () => {
  let component: PurchasesUsersComponent;
  let fixture: ComponentFixture<PurchasesUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PurchasesUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PurchasesUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
