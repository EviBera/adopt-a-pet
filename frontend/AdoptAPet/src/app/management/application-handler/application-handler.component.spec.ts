import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationHandlerComponent } from './application-handler.component';

describe('ApplicationHandlerComponent', () => {
  let component: ApplicationHandlerComponent;
  let fixture: ComponentFixture<ApplicationHandlerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicationHandlerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationHandlerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
