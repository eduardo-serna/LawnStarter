import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
  })
  export class ApiService {
    apiUrl = 'http://localhost:5069/get';

    constructor(private http: HttpClient) { }

    get(): Observable<any[]> {
        var results = this.http.get<any[]>(`${this.apiUrl}`);
        return results;
    }
  }