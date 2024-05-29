import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRoutehistoryComponent } from './add-routehistory.component';

describe('AddRoutehistoryComponent', () => {
  let component: AddRoutehistoryComponent;
  let fixture: ComponentFixture<AddRoutehistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddRoutehistoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddRoutehistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
