import { Component } from '@angular/core';
import { PedalProServiceService } from '../Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-update-database-timer',
  templateUrl: './update-database-timer.component.html',
  styleUrls: ['./update-database-timer.component.css']
})
export class UpdateDatabaseTimerComponent {
  hours: number=0; // Declare the 'hours' variable to store the input value

  constructor(private databaseService: PedalProServiceService,private dialog:MatDialog,private router:Router) {}

  updateDatabaseHours(): void {
    if(this.hours){
      this.databaseService.updateHoursBackup(this.hours).subscribe(
        (response) => {
          this.openModal();
        },
        (error) => {
          const errorMessage = error.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
        }
      );
    }
    else{
      this.openErrorDialog('Validation error: Please fill in all fields.');
    }
  }

  Logout()
  {
    this.databaseService.logout();
  }

  //redirect
  cancel_continue(){
    this.router.navigate(['companyLanding']);
  }

  //modal-pop-up
  openModal()
  {
    const modelDiv=document.getElementById('myModal');
    if(modelDiv!=null)
    {
      modelDiv.style.display='block';
    }
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

}
