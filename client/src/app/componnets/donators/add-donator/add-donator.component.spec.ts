import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdddonatorComponent } from './add-donator.component';

describe('AdddonatorComponent', () => {
  let component: AdddonatorComponent;
  let fixture: ComponentFixture<AdddonatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdddonatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdddonatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
