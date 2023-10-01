import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateVatInfoComponent } from './update-vat-info.component';

describe('UpdateVatInfoComponent', () => {
  let component: UpdateVatInfoComponent;
  let fixture: ComponentFixture<UpdateVatInfoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateVatInfoComponent]
    });
    fixture = TestBed.createComponent(UpdateVatInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
