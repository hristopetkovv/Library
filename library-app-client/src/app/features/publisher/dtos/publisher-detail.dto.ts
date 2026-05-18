import { BookListDto } from "../../book/dtos/book-list.dto";

export interface PublisherDetailDto {
    id: number;
    name: string;
    books: BookListDto[];
}