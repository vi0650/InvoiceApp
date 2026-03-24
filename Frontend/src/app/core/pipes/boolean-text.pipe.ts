import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'booleanText',
  standalone: false
})

export class BooleanTextPipe implements PipeTransform {

  transform(value: boolean): string {
    return value ? 'Active' : 'Inactive';  // Fixed typo
  }

}
@Pipe({
  name: 'liveText',
  standalone: false
})

export class LiveTextPipe implements PipeTransform {
  transform(value: boolean): string {
    return value ? 'Online' : 'Offline';
  }
}
