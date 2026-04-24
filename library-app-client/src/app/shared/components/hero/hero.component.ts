import { Component, inject, OnInit } from "@angular/core";
import { LibraryService } from "../../../core/services/library/library.service";
import { CountUpDirective } from "ngx-countup";

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [CountUpDirective],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.css'
})
export class HeroComponent implements OnInit {
  public libraryService = inject(LibraryService);

  ngOnInit(): void {
    this.libraryService.loadStats();
  }
}