import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftsDonatorComponent } from './gifts-donator.component';

describe('GiftsDonatorComponent', () => {
  let component: GiftsDonatorComponent;
  let fixture: ComponentFixture<GiftsDonatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftsDonatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftsDonatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
