import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { TranslatePipe } from '@ngx-translate/core';
import { SortOptionItem } from '../../models/sort-option-item.model';
 
@Component({
  selector: 'app-sort-select',
  standalone: true,
  imports: [FormsModule, NzSelectModule, TranslatePipe],
  templateUrl: './sort-select.component.html',
  styleUrl: './sort-select.component.css',
})
export class SortSelectComponent<T extends string> {
  readonly options = input.required<SortOptionItem<T>[]>();
  readonly value = input.required<T>();
  readonly valueChange = output<T>();
 
  onChange(newValue: T): void {
    this.valueChange.emit(newValue);
  }
}