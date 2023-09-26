import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDatabaseTimerComponent } from './update-database-timer.component';

describe('UpdateDatabaseTimerComponent', () => {
  let component: UpdateDatabaseTimerComponent;
  let fixture: ComponentFixture<UpdateDatabaseTimerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateDatabaseTimerComponent]
    });
    fixture = TestBed.createComponent(UpdateDatabaseTimerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
