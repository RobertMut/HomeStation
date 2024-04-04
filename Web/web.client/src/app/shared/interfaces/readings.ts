interface ReadingBase {
  id: number;
  readDate: Date;
}
export interface TemperatureReadings extends ReadingBase {
  temperature: number;
  humidity: number;
}

export interface PressureReadings extends ReadingBase {
  pressure: number;
}

export interface AirQualityReadings extends ReadingBase {
  pm2_5: number;
  pm1_0: number;
  pm10: number;
}
