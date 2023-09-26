import { Component ,Inject} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-error-dialog',
  template: `
    <div class="error-dialog">
      <h2>An error occured</h2>
      <p>Message: {{ data.message }}</p>
      <button [mat-dialog-close]="true" id="cancel">Close</button>
    </div>
  `,
  styles: [
    `
      .error-dialog {
        text-align: center;
        padding: 20px;
        border-radius: 20px;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        background-color: white;
        
      }

      h2 {
        color: #f44336;
      }

      p {
        margin-bottom: 16px;
      }

      button {
        background-color: #f44336;
        color: white;
        border:1px solid black
      }

      #cancel{
        
        background-color: white;
        color: rgb(219, 0, 0);
        border: none;
        padding: 10px 20px;
        cursor: pointer;
        border-radius: 30px;
        transition: background-color 0.6s ease-in-out;
      }
      
      #submit{
        background-color: black;
        color: #ffffff;
        border: none;
        padding: 10px 20px;
        cursor: pointer;
        transition: background-color 0.6s ease-in-out;
        border-radius: 30px;
      }
      
      #submit:hover{
        background-color: white;
        color: #199709;
        border:black solid 1px;
      }
      #cancel:hover{
        background-color: black;
        color: rgb(255, 255, 255);
        
      }
    `,
  ],
})


export class ErrorDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { message: string }) {}
}
