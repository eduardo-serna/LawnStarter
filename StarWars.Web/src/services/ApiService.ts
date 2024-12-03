import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable, of, tap } from "rxjs";

@Injectable({
    providedIn: 'root'
  })
export class ApiService {
  apiUrlPeople = 'http://localhost:5069/get';
  apiUrlFilm = '';
  private cache: { name: string; resources: string, objectType: string}[] | null = null; // Store all cached data

  constructor(private http: HttpClient) { }

  getAll(isPeople: boolean): Observable<any[]> {
    const apiUrl = isPeople ? this.apiUrlPeople : this.apiUrlFilm; // THIS CAN BE HANDLE BETTER IN A ENVIROMENT VARIABLE
    if (this.cache) {
      // If cache is available, return it as an Observable
      return of(this.cache);
    }

    return this.http.get<any[]>(`${apiUrl}`).pipe(
        map((response) =>  response.map((item) => ({
          name: item.name,
          resources: isPeople ? item.films : item.characters,
          objectType: isPeople ? 'people' : 'film'}))
        ),
        tap((data) => (this.cache = data)) // Cache the data
      );
  }

  /**
   * Filters the cached data based on the query.
   * If the cache is empty, it fetches data from the API first.
   */
  filter(query: string, isPeople: boolean): Observable<{ name: string; resources: string; objectType: string }[]> {
    if (this.cache) {
      // Perform filtering on the cache
      const filteredResults = this.cache.filter((item) =>
        item.name.toLowerCase().includes(query.toLowerCase())
      );
      return of(filteredResults);
    }

    return this.getAll(isPeople).pipe(
      map((data) =>
        data.filter((item) => item.objectType === (isPeople ? 'people' : 'film') &&
        item.name.toLowerCase().includes(query.toLowerCase()))
      )
    );
  }
}