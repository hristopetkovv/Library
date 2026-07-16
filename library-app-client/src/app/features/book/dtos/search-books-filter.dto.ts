import { CoverType } from "../enums/cover-type.enum";
import { Language } from "../enums/language.enum";

export interface SearchBooksFilterDto {
  term?: string | null;
  language?: Language | null;
  coverType?: CoverType | null;
  genreIds?: number[] | null;
  availableOnly?: boolean | null;
}