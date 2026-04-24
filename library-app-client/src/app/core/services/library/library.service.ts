import { inject, Injectable, signal } from "@angular/core";
import { LibraryResource } from "../../resources/library/library.resource";
import { LibraryStatsDto } from "../../dtos/stats/library-stats.dto";
import { NzMessageService } from "ng-zorro-antd/message";

@Injectable({ providedIn: 'root' })
export class LibraryService {
  private readonly libraryResource = inject(LibraryResource);
  private message = inject(NzMessageService);
  
  stats = signal<LibraryStatsDto>({ totalBooks: 1200, totalAuthors: 500, totalPublishers: 45 });

  loadStats() {
    this.libraryResource.getLibraryStats().subscribe({
      //next: (data) => this.stats.set(data),
      error: (err) => this.message.error(err.error?.detail)
    });
  }
}