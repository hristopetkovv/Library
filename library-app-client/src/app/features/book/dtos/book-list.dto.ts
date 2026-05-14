import { GenreDto } from "./genre.dto";

export interface BookListDto {
    id: number;
    title: string;
    authorName: string;
    publisherName: string;
    isbn: string;
    availableCopies: number;
    genres: GenreDto[];
    coverImageUrl: string;
}