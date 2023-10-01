import { Component } from '@angular/core';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Component({
  selector: 'app-restore-database',
  templateUrl: './restore-database.component.html',
  styleUrls: ['./restore-database.component.css']
})
export class RestoreDatabaseComponent {
  constructor(private service: PedalProServiceService,private dialog:MatDialog) {}

  restoreDatabase() {
    this.service.restoreDatabase().subscribe(
      (response) => {
        this.openModal();
      },
      (error) => {
        const errorMessage = error.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
    
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  openModal()
  {
    const modelDiv=document.getElementById('myModal');
    if(modelDiv!=null)
    {
      modelDiv.style.display='block';
    }
  }

  Logout()
  {
    
    this.service.logout();
  }
}
