import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { HttpClient } from '@angular/common/http';
import {formatDate} from '@angular/common';

interface HomeStationReadings {
  id: number;
  temperature: number;
  pressure: number;
  humidity: number;
  pm2_5: number;
  pm1_0: number;
  pm10: number;
  readDate: Date;
}

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent implements OnInit {

  private readings: HomeStationReadings[] = [];
  public chart: any;

  constructor(private http: HttpClient) {}
  ngOnInit() {
    this.getReadings();
  }

  getReadings() {
    this.http.get<HomeStationReadings[]>('/api/Air/1/Day/1').subscribe(
      (result) => {
        this.readings = result;
        this.chart = new Chart(
          "chart",
          {
            type: 'line',
            data: {
              xLabels: this.readings.map(x => formatDate(x.readDate, 'dd-HH-ss', 'en-US')),
              datasets: [
                {
                  label: 'Temperature',
                  data: this.readings.map(x => x.temperature)
                },
                {
                  label: 'Pressure',
                  data: this.readings.map(x => x.pressure)
                },
                {
                  label: 'Humidity',
                  data: this.readings.map(x => x.humidity)
                },
                {
                  label: 'PM2.5',
                  data: this.readings.map(x => x.pm2_5)
                },
                {
                  label: 'PM10',
                  data: this.readings.map(x => x.pm10)
                },
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
