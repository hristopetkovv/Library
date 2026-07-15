import { BorrowingStatus } from "../enums/borrowings-status.enum";

export interface BorrowingBasicDto {
  bookTitle: string;
  borrowDate: string;
  dueDate: string;
  returnDate: string | null;
  status: BorrowingStatus;
}