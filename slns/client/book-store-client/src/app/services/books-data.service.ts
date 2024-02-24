import {Injectable} from '@angular/core';
import {ILoadingBooksRequest, ILoadingBooksResponse} from "../models/book";
import {BehaviorSubject} from "rxjs";
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class BooksDataService {
  baseUrl = environment.baseUrl;

  // create behavior subject to store books data
  books = new BehaviorSubject<ILoadingBooksResponse[]>([]);

  books$() {
    return this.books.asObservable();
  }

  constructor(private http: HttpClient) {
  }

  search(request: ILoadingBooksRequest) {
    let params = new HttpParams()
      .set('searchKey', request.searchKey)
      .set('pageNumber', request.pageNumber.toString())
      .set('pageSize', request.pageSize.toString());

    this.http.get<ILoadingBooksResponse[]>(`${this.baseUrl}books`, {params})
      .subscribe((response) => {
        this.books.next(response);
      });
  }
}
