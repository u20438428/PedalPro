import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyLandingPageComponent } from './company-landing-page.component';

describe('CompanyLandingPageComponent', () => {
  let component: CompanyLandingPageComponent;
  let fixture: ComponentFixture<CompanyLandingPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyLandingPageComponent]
    });
    fixture = TestBed.createComponent(CompanyLandingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
