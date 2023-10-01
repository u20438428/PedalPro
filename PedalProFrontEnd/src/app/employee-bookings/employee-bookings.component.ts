import { Component,OnInit } from '@angular/core';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { Router } from '@angular/router';
@Component({
  selector: 'app-employee-bookings',
  templateUrl: './employee-bookings.component.html',
  styleUrls: ['./employee-bookings.component.css']
})
export class EmployeeBookingsComponent implements OnInit{

  empbookingdata:any={};

  constructor(private service:PedalProServiceService,private dialog:MatDialog,private router:Router){}
  ngOnInit(): void {
  this.FetchEmpBookings();
  }

  FetchEmpBookings(){
    this.service.GetEmployeeBookings().subscribe((data) => {
      this.empbookingdata = data;
    },error=>
    {
      const errorMessage = error.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
    });

  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  Logout()
  {
    this.service.logout();
  }

  ReloadPage()
  {
    location.reload();
  }


  SetAttended(bookingid:number){
    this.service.SetAttendance(1, bookingid).subscribe((response) => {
      this.openModal();
    },error=>{
      const errorMessage = error.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
    });
  }

  SetNotAttended(bookingid:number){
    this.service.SetAttendance(2, bookingid).subscribe((response) => {
    },error=>{
      const errorMessage = error.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
    });
  }

  cancel_continue(){
    this.router.navigate(['viewEmployees']);
  }

  //Notification
  openModal()
  {
    const modelDiv=document.getElementById('myModal');
    if(modelDiv!=null)
    {
      modelDiv.style.display='block';
    }
  }

}
