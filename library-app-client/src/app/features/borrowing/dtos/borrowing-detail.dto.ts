import { BookListDto } from "../../book/dtos/book-list.dto";
import { BorrowingStatus } from "../enums/borrowings-status.enum";

export interface BorrowingDetailDto {
  id: number;
  book: BookListDto;
  userEmail: string;
  borrowDate: string;
  dueDate: string;
  returnDate: string | null;
  status: BorrowingStatus;
}