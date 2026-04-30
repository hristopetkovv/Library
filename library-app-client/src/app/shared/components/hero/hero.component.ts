import { Component, inject, OnInit, signal } from "@angular/core";
import { CountUpDirective } from "ngx-countup";
import { LibraryStatsDto } from "../../../features/library/dtos/library-stats.dto";
import { TranslatePipe } from "@ngx-translate/core";
import { LibraryResource } from "../../../features/library/resources/library.resource";

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [CountUpDirective, TranslatePipe],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.css'
})
export class HeroComponent implements OnInit {
  private libraryResource = inject(LibraryResource);

  stats = signal<LibraryStatsDto>({ totalBooks: 1200, totalAuthors: 500, totalPublishers: 45 });

  ngOnInit(): void {
    this.libraryResource.getLibraryStats().subscribe({
      //next: (data) => this.stats.set(data)
    });
  }
}