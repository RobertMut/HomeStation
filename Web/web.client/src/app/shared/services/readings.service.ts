import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import {DetailLevel} from "../detail-level";
import {TemperatureReadings} from "../interfaces/readings";
import {ReadingType} from "../reading-type";

const api = "/api/Air/";
@Injectable({
  providedIn: 'root'
})
export class ReadingsService {
  constructor(private http: HttpClient) { }

  public getReadings(readingType: string, device: number | undefined, startDate: Date | undefined, endDate: Date | undefined, detail: string)
    : Observable<any[]> {
    let url = readingType + "/" + device?.toString() + "/" + startDate?.toISOString() + "/" + endDate?.toISOString() + "/" + detail
    return this.getResponse<any[]>(url);
  }

  private getResponse<Type>(url: string): Observable<Type>{
    return this.http.get<Type>(api + url);
  }
}