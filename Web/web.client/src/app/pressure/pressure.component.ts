import { Component, ContentChildren, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { HttpClient } from '@angular/common/http';
import {formatDate} from '@angular/common';
import {ReadingsService} from "../shared/services/readings.service";
import {PressureReadings} from "../shared/interfaces/readings";
import {DataPickerComponent} from "../shared/dynamic/data-picker/data-picker.component";
import {DataForm} from "../shared/interfaces/data-form";
import {ReadingType} from "../shared/reading-type";
import {DetailLevel} from "../shared/detail-level";

@ContentChildren(DataPickerComponent)
@Component({
  selector: 'app-pressure',
  templateUrl: './pressure.component.html',
  styleUrl: './pressure.component.css'
})
export class PressureComponent {

  private readings: PressureReadings[] = [];
  public chart: any;

  constructor(private readingsService: ReadingsService) {
  }


  getReadings(event: DataForm) {
    this.readingsService.getReadings(
      ReadingType[ReadingType.Climate],
      event.device,
      event.startDate,
      event.endDate,
      event.selectedDetail).subscribe(
      (result) => {
        this.readings = result;
        if(this.chart != undefined){
          this.chart.destroy();

        }
        this.chart = new Chart(
          "chart",
          {
            type: 'line',
            data: {
              xLabels: this.readings.map(x => formatDate(x.readDate, 'yyyy-MM-dd hh-mm-ss', 'en-US')),
              datasets: [
                {
                  label: 'Pressure',
                  data: this.readings.map(x => x.pressure)
                }
              ]
            }
          }
        );
      },
      (error) => {
        console.error(error);
      }
    );
  }

  protected readonly DataPickerComponent = DataPickerComponent;
}
