import { DatePipe } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { TranslatePipe } from "@ngx-translate/core";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzTagModule } from "ng-zorro-antd/tag";
import { finalize } from "rxjs";
import { FormsModule } from "@angular/forms";
import { FavoriteBooksResource } from "../../../../book/resources/favorite-books.resource";
import { UserFavoriteBookDto } from "../../../dtos/user-favorite-book.dto";

@Component({
  selector: 'app-favorite-books',
  standalone: true,
  imports: [
    TranslatePipe,
    FormsModule,
    NzTableModule,
    NzTagModule,
    DatePipe,
  ],
  templateUrl: './favorite-books.component.html',
})
export class FavoriteBooksComponent implements OnInit {
  private readonly favoriteBooksResource = inject(FavoriteBooksResource);

  readonly favoriteBooks = signal<UserFavoriteBookDto[]>([]);
  readonly isLoading = signal(false);

  ngOnInit(): void {
    this.isLoading.set(true);

    this.favoriteBooksResource.getMine()
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: fb => this.favoriteBooks.set(fb) });
  }
}