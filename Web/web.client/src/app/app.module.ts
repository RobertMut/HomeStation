import { HttpClientModule } from '@angular/common/http';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {FormsModule,} from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatTabsModule} from '@angular/material/tabs';
import {MatButtonModule} from '@angular/material/button';
import {MatDatepickerModule} from '@angular/material/datepicker';

import { TemperatureComponent } from './temperature/temperature.component';
import { PressureComponent } from './pressure/pressure.component';
import { AirQualityComponent } from './air-quality/air-quality.component';
import { DataPickerComponent } from './shared/dynamic/data-picker/data-picker.component'
import {ReadingsService} from "./shared/services/readings.service";

@NgModule({
  declarations: [
    AppComponent,
    TemperatureComponent,
    PressureComponent,
    AirQualityComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, DataPickerComponent,
    MatFormFieldModule, MatSelectModule, MatInputModule, MatTabsModule, FormsModule, MatButtonModule,
    MatFormFieldModule, MatDatepickerModule
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
