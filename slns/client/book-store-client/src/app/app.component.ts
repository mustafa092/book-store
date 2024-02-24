import {Component, HostListener, OnDestroy, OnInit} from '@angular/core';
import {BooksDataService} from "./services/books-data.service";
import {ILoadingBooksRequest, ILoadingBooksResponse} from "./models/book";
import {debounceTime, distinctUntilChanged, filter, Subscription} from "rxjs";
import {FormControl} from "@angular/forms";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
// implements OnInit

export class AppComponent implements OnInit, OnDestroy {

  books: ILoadingBooksResponse[] = [];
  bookSearchParams: ILoadingBooksRequest = {} as ILoadingBooksRequest;
  private booksDataSubscription: Subscription = new Subscription();
  searchTerm: string = '';
  isLoading: boolean = false;
  private isFetching: boolean = false;
  private lastScrollTop: number = 0;
  private scrollDebounceTimer: any;
  searchInput: FormControl = new FormControl('');

  constructor(private bookData: BooksDataService) {
  }

  ngOnInit(): void {
    this.initSearch();
    this.handleUserInput();
  }

  handleUserInput(): void {
    this.searchInput.valueChanges.pipe(
      debounceTime(3000), // Wait for 3 seconds after the last event before emitting last event
      filter(term => term.length >= 3 || term.length === 0), // Emit only if term length is 3 or more (or empty for reset)
      distinctUntilChanged() // Emit only if the current value is different than the last
    ).subscribe(term => {
      this.bookSearchParams.searchKey = term;
      this.bookSearchParams.pageNumber = 1; // Reset to page 1 for new search
      this.isLoading = true;
      this.bookData.search(this.bookSearchParams);
    });
  }


  initSearch(): void {

    this.bookSearchParams = {
      pageNumber: 1,
      pageSize: 10,
      searchKey: ''
    };
    this.isLoading = true;
    this.bookData.search(this.bookSearchParams);

    this.booksDataSubscription = this.bookData.books$().subscribe(
      (newBooks) => {
        if (this.bookSearchParams.pageNumber > 1) {
          // Append new data to existing data
          this.books = [...this.books, ...newBooks];
        } else {
          // For the first page, or after a new search, replace the data
          this.books = newBooks;
        }
        this.isLoading = false;
        this.isFetching = false;
      },
      (error) => {
        // Handle error
        this.isLoading = false;
        this.isFetching = false;
      }
    );
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(): void {
    const scrollTop = (document.documentElement.scrollTop || document.body.scrollTop);
    const pos = scrollTop + window.innerHeight;
    const max = document.documentElement.scrollHeight;

    // Check if scrolling down and near the bottom
    if (scrollTop > this.lastScrollTop && max - pos < 100 && !this.isFetching) {
      if (this.scrollDebounceTimer) clearTimeout(this.scrollDebounceTimer);
      this.scrollDebounceTimer = setTimeout(() => {
        this.loadMoreBooks();
      }, 100);
    }
    this.lastScrollTop = scrollTop;
  }


  loadMoreBooks(): void {
    if (this.isFetching) return; // Prevent multiple requests
    this.isFetching = true;
    this.isLoading = true; // Enable loading indicator
    this.bookSearchParams.pageNumber++;
    this.bookData.search(this.bookSearchParams);
  }

  searchBooks(): void {
    this.isLoading = true;
    this.bookSearchParams.pageNumber = 1; // Reset to page 1 for new searches
    this.bookSearchParams.searchKey = this.searchTerm;
    this.bookData.search(this.bookSearchParams);
  }

  ngOnDestroy(): void {
    if (this.scrollDebounceTimer) clearTimeout(this.scrollDebounceTimer);
    this.booksDataSubscription.unsubscribe();
  }
}
