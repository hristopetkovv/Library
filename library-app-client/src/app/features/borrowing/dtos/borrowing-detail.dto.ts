import { BookBorrowingDto } from "../../book/dtos/book-borrowing.dto";
import { BorrowingStatus } from "../enums/borrowings-status.enum";

export interface BorrowingDetailDto {
  id: number;
  book: BookBorrowingDto;
  userEmail: string;
  borrowDate: string;
  dueDate: string;
  returnDate: string | null;
  status: BorrowingStatus;
}