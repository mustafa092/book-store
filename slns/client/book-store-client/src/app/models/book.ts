export interface ILoadingBooksRequest {
  searchKey: string;
  pageNumber: number;
  pageSize: number;
}
export interface ILoadingBooksResponse {
  bookId: number;
  bookTitle: string;
  bookDescription: string;
  author: string;
  publishDate: string;
  coverBase64: string;
}
