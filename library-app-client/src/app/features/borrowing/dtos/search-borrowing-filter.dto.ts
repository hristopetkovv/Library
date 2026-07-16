import { BorrowingStatus } from "../enums/borrowings-status.enum";

export interface SearchBorrowingsFilterDto {
  bookTitle?: string | null;
  authorName?: string | null;
  userEmail?: string | null;
  isbn?: string | null;
}