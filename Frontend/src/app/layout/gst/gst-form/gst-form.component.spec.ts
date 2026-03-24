import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GstFormComponent } from './gst-form.component';

describe('GstFormComponent', () => {
  let component: GstFormComponent;
  let fixture: ComponentFixture<GstFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GstFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GstFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
