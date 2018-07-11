import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-unauthorized',
  template: `<br>
  <mat-card>
    <span style="color:red; font-weight: bolder;">{{message}}</span>
  </mat-card>
  `
})
export class UnauthorizedComponent implements OnInit {

  public message: string;
  public values: any[] = [];

  constructor() {
    this.message = '401: You have no rights to access this. Please Login';
  }

  ngOnInit() {
  }

}
