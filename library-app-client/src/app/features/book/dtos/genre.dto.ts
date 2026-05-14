import { Category } from "../enums/category.enum";

export interface GenreDto {
    id: number;
    name: string;
    nameBg: string;
    category: Category
}