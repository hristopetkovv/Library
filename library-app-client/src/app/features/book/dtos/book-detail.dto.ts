import { AuthorBasicDto } from "../../author/dtos/author-basic.dto";
import { PublisherBasicDto } from "../../publisher/dtos/publisher-basic.dto";
import { CoverType } from "../enums/cover-type.enum";
import { Language } from "../enums/language.enum";
import { GenreDto } from "./genre.dto";

export interface BookDetailDto {
    id: number;
    title: string;
    author: AuthorBasicDto;
    publisher: PublisherBasicDto;
    isbn: string;
    description: string;
    pages: number;
    language: Language;
    coverType: CoverType;
    publicationYear: number;
    totalCopies: number;
    availableCopies: number;
    genres: GenreDto[];
    coverImageUrl: string;
}