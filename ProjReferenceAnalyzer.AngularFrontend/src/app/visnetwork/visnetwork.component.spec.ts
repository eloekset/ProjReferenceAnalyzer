import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisnetworkComponent } from './visnetwork.component';

describe('VisnetworkComponent', () => {
  let component: VisnetworkComponent;
  let fixture: ComponentFixture<VisnetworkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VisnetworkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisnetworkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
