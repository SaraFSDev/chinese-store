import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDonatorComponent } from './edit-donator.component';

describe('EditDonatorComponent', () => {
  let component: EditDonatorComponent;
  let fixture: ComponentFixture<EditDonatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditDonatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDonatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
