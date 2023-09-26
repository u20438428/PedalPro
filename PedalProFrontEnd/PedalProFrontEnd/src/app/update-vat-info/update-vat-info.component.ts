import { Component,OnInit } from '@angular/core';
import { PedalProServiceService } from '../Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-update-vat-info',
  templateUrl: './update-vat-info.component.html',
  styleUrls: ['./update-vat-info.component.css']
})
export class UpdateVatInfoComponent implements OnInit{
  percentage:number=0;

  vatinfo:any={};

  ngOnInit(): void {
    this.callGetVAT();
  }

  constructor(private databaseService: PedalProServiceService,private dialog:MatDialog,private router:Router) {}

  updateVatPercentage(): void {
    if(this.percentage){
      this.databaseService.updateVAT(this.percentage).subscribe(
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


  callGetVAT() {
    this.databaseService.getVAT().subscribe(
      (data: any) => {
       this.vatinfo=data;
      },
      (error: any) => {
        const errorMessage = error.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
  }
}
