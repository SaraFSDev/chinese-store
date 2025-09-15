import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagmentGiftsComponent } from './managment-gifts.component';

describe('GiftsComponent', () => {
  let component: ManagmentGiftsComponent;
  let fixture: ComponentFixture<ManagmentGiftsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagmentGiftsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagmentGiftsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
