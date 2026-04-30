import { HttpClient } from "@angular/common/http";
import { inject } from "@angular/core";

export abstract class BaseResource {
    protected readonly http = inject(HttpClient);

    protected abstract readonly baseUrl: string;

    public composeQueryString(object: any): string {
    let result = '';
    let isFirst = true;

    if (object) {
      Object.keys(object)
        .filter(key => object[key] !== null && object[key] !== undefined)
        .forEach(key => {
          let value = object[key];
          if (value instanceof Date) {
            value = value.toISOString();
          }

          if (isFirst) {
            result = '?' + key + '=' + value;
            isFirst = false;
          } else {
            result += '&' + key + '=' + value;
          }
        });
    }

    return result;
  }
}