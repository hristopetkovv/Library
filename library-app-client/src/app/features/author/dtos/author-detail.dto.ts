import { BookBasicDto } from "../../book/dtos/book-basic.dto";

export interface AuthorDetailDto {
    id: number;
    name: string;
    biography: string;
    books: BookBasicDto[];
}