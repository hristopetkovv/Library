import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { HeroComponent } from './shared/components/hero/hero.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { TranslateService } from '@ngx-translate/core';
import { bg_BG, en_US, NzI18nService } from 'ng-zorro-antd/i18n';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NzLayoutModule, HeroComponent, HeaderComponent, FooterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  private translate = inject(TranslateService);
  private nzI18n = inject(NzI18nService);
  
  constructor() {
    this.translate.addLangs(['bg', 'en']);
    
    let savedLang = localStorage.getItem('lang');
    
    if (!savedLang) {
      savedLang = 'bg';
      localStorage.setItem('lang', savedLang);
    }
    
    this.translate.use(savedLang);
    this.updateZorroLocale(savedLang);
  }

  private updateZorroLocale(lang: string): void {
    if (lang === 'bg') {
      this.nzI18n.setLocale(bg_BG);
    } else if (lang === 'en') {
      this.nzI18n.setLocale(en_US);
    } else {
      this.nzI18n.setLocale(bg_BG);
    }
  }
}
