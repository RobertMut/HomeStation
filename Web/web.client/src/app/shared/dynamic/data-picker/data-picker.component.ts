import {Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Device} from "../../interfaces/device";
import {FormControl, FormGroup, FormsModule, Validators, ReactiveFormsModule} from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatButtonModule} from '@angular/material/button';
import {provideNativeDateAdapter} from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {JsonPipe} from '@angular/common';

import {DetailLevel, Details} from "../../detail-level";
import {DataForm} from "../../interfaces/data-form";
import {ReadingType} from "../../reading-type";

@Component({
  selector: 'app-data-picker',
  templateUrl: './data-picker.component.html',
  styleUrl: './data-picker.component.css',
  imports: [MatFormFieldModule, MatSelectModule, MatInputModule, FormsModule, MatButtonModule, MatDatepickerModule, FormsModule, ReactiveFormsModule, JsonPipe],
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [provideNativeDateAdapter()],
})
export class DataPickerComponent {
  @Output() onSubmit: EventEmitter<DataForm>;

  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  numberFormControl = new FormControl('', [Validators.required]);
  protected devices: Device[] = [];

  protected details: Details[] = [
    {value: DetailLevel[DetailLevel.Detailed]},
    {value: DetailLevel[DetailLevel.Normal]},
    {value: DetailLevel[DetailLevel.Less]}
  ]

  protected form: DataForm;

  constructor(private http: HttpClient) {
    this.onSubmit = new EventEmitter();
    this.form = {} as DataForm;
    this.getDevices();
  }

  publishValues(){
    this.onSubmit.emit(this.form);
  }

  private getDevices() {
    this.http.get<Device[]>('/api/Device').subscribe(
      (result) => {
        this.devices = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }
}

