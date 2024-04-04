import {Component, ContentChildren} from '@angular/core';
import {Chart} from 'chart.js/auto';
import {formatDate} from '@angular/common';
import {TemperatureReadings} from "../shared/interfaces/readings";
import {ReadingsService} from "../shared/services/readings.service";
import {DataPickerComponent} from "../shared/dynamic/data-picker/data-picker.component";
import {DataForm} from "../shared/interfaces/data-form";
import {ReadingType} from "../shared/reading-type";
import {DetailLevel} from "../shared/detail-level";

@ContentChildren(DataPickerComponent)
@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrl: './temperature.component.css'
})
export class TemperatureComponent{

  private readings: TemperatureReadings[] = [];
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
                  label: 'Temperature',
                  data: this.readings.map(x => x.temperature)
                },
                {
                  label: 'Humidity',
                  data: this.readings.map(x => x.humidity)
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
