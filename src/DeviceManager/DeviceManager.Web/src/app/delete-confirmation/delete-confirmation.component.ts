import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-delete-confirmation',
  template: `<h1 mat-dialog-title>{{'LBL_WARNING' | translate}}</h1>
  <div mat-dialog-content>
    <p>{{ 'ARE_YOU_SURE' | translate }}</p>
  </div>
  <div mat-dialog-actions>
    <button mat-button (click)="onNoClick(data.dto)" cdkFocusInitial>{{ 'NO' | translate}}</button>
    <button mat-button (click)="onYesClick(data.dto)">{{ 'YES' | translate }}</button>
  </div>
  `
})
export class DeleteConfirmationComponent {

  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  onNoClick(dto: any): void {
    this.data.confirmation = 'NO';
    this.dialogRef.close(this.data);
  }

  onYesClick(dto: any): void {
    this.data.confirmation = 'YES';
    this.dialogRef.close(this.data);
  }

}
