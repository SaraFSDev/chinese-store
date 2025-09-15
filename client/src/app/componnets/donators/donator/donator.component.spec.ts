import { ComponentFixture, TestBed } from '@angular/core/testing';

import { donatorComponent } from './donator.component';

describe('donatorComponent', () => {
  let component: donatorComponent;
  let fixture: ComponentFixture<donatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [donatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(donatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
