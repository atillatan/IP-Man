import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-login',
  template: `
     <form [formGroup]="loginForm" (ngSubmit)="submit()">
       <input type="email" formControlName="email"><br>
       <input type="password" formControlName="password">
       <input type="submit" value="submit">
     </form>
  `
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  ngOnInit() {
    this.loginForm = new FormGroup({
      email: new FormControl(),
      password: new FormControl()
    });
  }

  submit() {
    alert(this.loginForm.value.email + this.loginForm.value.password);
  }

}




