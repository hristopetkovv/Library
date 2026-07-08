import { Category } from "../enums/category.enum";

export interface GenreDto {
    genreId: number;
    genreName: string;
    genreNameBg: string;
    genreCategory: Category
}