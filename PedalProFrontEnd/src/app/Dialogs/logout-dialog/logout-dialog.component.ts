import { Component ,Inject,OnInit} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-logout-dialog',
  template: `
    
    <div class="logged-out-dialog">
    <mat-icon class="icon">exit_to_app</mat-icon>
      <h5 class="title">{{ data.message }}</h5>
    </div>
  `,
  styles: [
    `
      .logged-out-dialog {
  text-align: center;
  padding: 15px;
  background-color: white;
  
  border-radius: 10px;
}

.icon {
  font-size: 20px;
  color: black; /* Use a color that fits your branding */
}

.title {
  font-family: 'Arial', sans-serif;
  font-size: 20px;
  margin: 10px 0;
  font-weight:normal
}

.message {
  font-size: 18px;
  color: #333;
  margin: 10px 0;
}

.button {
  background-color: #2196F3;
  color: #fff;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.button:hover {
  background-color: #0e7ae6;
}

    `,
  ],
})
export class LogoutDialogComponent{
  constructor(@Inject(MAT_DIALOG_DATA) public data: { message: string }) {}

  
}
