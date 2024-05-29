import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddvehicleInfoComponent } from './addvehicle-info.component';

describe('AddvehicleInfoComponent', () => {
  let component: AddvehicleInfoComponent;
  let fixture: ComponentFixture<AddvehicleInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddvehicleInfoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddvehicleInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
