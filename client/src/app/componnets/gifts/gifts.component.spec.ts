import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeGiftsComponent } from './gifts.component';

describe('HomeGiftsComponent', () => {
  let component: HomeGiftsComponent;
  let fixture: ComponentFixture<HomeGiftsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeGiftsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeGiftsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
