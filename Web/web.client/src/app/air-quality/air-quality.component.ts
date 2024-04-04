import {Component, ContentChild, OnInit} from '@angular/core';
import {Chart} from 'chart.js/auto';
import {formatDate} from '@angular/common';
import {AirQualityReadings} from '../shared/interfaces/readings';
import {ReadingsService} from "../shared/services/readings.service";
import {DataPickerComponent} from "../shared/dynamic/data-picker/data-picker.component";
import {DataForm} from "../shared/interfaces/data-form";
import {ReadingType} from "../shared/reading-type";
import {DetailLevel} from "../shared/detail-level";

@Component({
  selector: 'app-air-quality',
  templateUrl: './air-quality.component.html',
  styleUrl: './air-quality.component.css'
})
export class AirQualityComponent {

  private readings: AirQualityReadings[] = [];
  public chart: any;

  constructor(private readingsService: ReadingsService) {
  }

  ngOnInit() {
  }

  getReadings(event: DataForm) {
    this.readingsService.getReadings(
      ReadingType[ReadingType.Quality],
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
                  label: 'PM1.0',
                  data: this.readings.map(x => x.pm1_0)
                },
                {
                  label: 'PM2.5',
                  data: this.readings.map(x => x.pm2_5)
                },
                {
                  label: 'PM10',
                  data: this.readings.map(x => x.pm10)
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
}
