import { CoverType } from "../enums/cover-type.enum";
import { Language } from "../enums/language.enum";

export interface SearchBooksFilterDto {
  term?: string | null;
  authorId?: number | null;
  publisherId?: number | null;
  language?: Language | null;
  coverType?: CoverType | null;
  publicationYear?: number | null;
  genreIds?: number[] | null;
  availableOnly?: boolean | null;
}