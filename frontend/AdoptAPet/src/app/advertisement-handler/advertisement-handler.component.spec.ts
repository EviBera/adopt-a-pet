import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementHandlerComponent } from './advertisement-handler.component';

describe('AdvertisementHandlerComponent', () => {
  let component: AdvertisementHandlerComponent;
  let fixture: ComponentFixture<AdvertisementHandlerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementHandlerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementHandlerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
