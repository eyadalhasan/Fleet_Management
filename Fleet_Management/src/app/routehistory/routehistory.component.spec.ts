import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoutehistoryComponent } from './routehistory.component';

describe('RoutehistoryComponent', () => {
  let component: RoutehistoryComponent;
  let fixture: ComponentFixture<RoutehistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RoutehistoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RoutehistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
