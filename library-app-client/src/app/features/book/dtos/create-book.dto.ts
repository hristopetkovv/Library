import { CoverType } from "../enums/cover-type.enum";
import { Language } from "../enums/language.enum";

export interface CreateBookDto {
  title: string;
  authorId: number;
  publisherId: number;
  isbn: string;
  description: string | null;
  pages: number;
  language: Language;
  coverType: CoverType;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  genreIds: number[];
}