import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateRegulatorService {
  getDate(dateString: string) {
    const date = new Date(dateString);
    const options: Intl.DateTimeFormatOptions = {
      weekday: 'long',
      day: 'numeric',
      month: 'long',
      hour: 'numeric',
      minute: 'numeric',
    };
    return new Intl.DateTimeFormat('en', options).format(date);
  }

  convertDate(dateTimeString: string): string {
    const date = new Date(dateTimeString);
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0'); // Months are 0-indexed
    const day = String(date.getUTCDate()).padStart(2, '0');
    const hours = String(date.getUTCHours()).padStart(2, '0');
    const minutes = String(date.getUTCMinutes()).padStart(2, '0');
    const seconds = String(date.getUTCSeconds()).padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`;
  }
}

// import { Injectable } from '@angular/core';
// import { default as dayjs } from 'dayjs';

// @Injectable({
//   providedIn: 'root',
// })
// export class DateRegulatorService {
//   getDate(dateString: string) {
//     const dateObj = dayjs(dateString);
//     const dayName = dateObj.locale('en').format('dddd');
//     const day = dateObj.format('D');
//     const monthName = dateObj.format('MMMM');
//     const time = dateObj.format('HH:mm');
//     return `${dayName} ${day} ${monthName} ${time}`;
//   }

//   convertDate(dateTimeString: string): string {
//     const date = new Date(dateTimeString);
//     const year = date.getUTCFullYear();
//     const month = String(date.getUTCMonth() + 1).padStart(2, '0'); // Months are 0-indexed
//     const day = String(date.getUTCDate()).padStart(2, '0');
//     const hours = String(date.getUTCHours()).padStart(2, '0');
//     const minutes = String(date.getUTCMinutes()).padStart(2, '0');
//     const seconds = String(date.getUTCSeconds()).padStart(2, '0');

//     return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`;
//   }
// }
