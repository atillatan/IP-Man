import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  head = 'Cross Platform Desktop Application';
  users = null;

  constructor(private http: HttpClient) {
    this.listUsers();
  }

  listUsers(): void {
    this.http.get<any>(`http://localhost:5000/spa/getusers`).subscribe(data => {
      this.users = data;
    });
  }

}
