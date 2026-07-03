import { GenreDto } from "./genre.dto";

export interface BookListDto {
    id: number;
    title: string;
    authorName: string;
    publisherName: string;
    isbn: string;
    pages: number;
    totalCopies: number; 
    availableCopies: number;
    genres: GenreDto[];
    coverImageUrl: string;
}