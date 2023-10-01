import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { DateWithTimeslotDto } from 'src/app/Models/date-with-timeslot-dto';
import { Employee } from 'src/app/Models/employee';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Component({
  selector: 'app-edittimeslots',
  templateUrl: './edittimeslots.component.html',
  styleUrls: ['./edittimeslots.component.css']
})
export class EdittimeslotsComponent {
  dateWithTimeslotDto: DateWithTimeslotDto = {
    date: new Date(),
    startTime: '',
    endTime: '',
    employeeId: 0,
    timeslotId:0
  };
  isNewTimeslot: boolean = false;

  employees:Employee[]=[];

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private service: PedalProServiceService,private dialog:MatDialog,private router:Router
  ) { }

  ngOnInit() {
    this.isNewTimeslot = this.route.snapshot.paramMap.get('id') === 'new';
    if (!this.isNewTimeslot) {
      this.loadTimeslotDetails();
    }

    this.GetModules();
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  getCurrentDate(): Date {
    return new Date(); // Returns the current date
  }

  cancel_continue() {
    this.router.navigate(['/ViewSchedule']);
  }

  // modal pop-up
  openModal() {
    const modelDiv = document.getElementById('myModal');
    if (modelDiv != null) {
      modelDiv.style.display = 'block';
    }
  }

  GetModules(){
    this.service.GetEmployeesTwo().subscribe(result=>{
      let moduleList:any[]=result
      moduleList.forEach((element)=>{
        this.employees.push(element)
      });
    })
    return this.employees;
  }

  loadTimeslotDetails() {
    const timeslotIdParam = this.route.snapshot.paramMap.get('id');
    if (timeslotIdParam !== null) {
      const timeslotId = +timeslotIdParam;
      this.service.getTimeslotById(timeslotId).subscribe(response => {
        this.dateWithTimeslotDto = response;
      });
    } else {
      // Handle the case when the timeslotId is null, e.g., show an error message or redirect to another page.
    }
  }

  saveTimeslot() {
    if (this.isNewTimeslot) {
      this.service.addTimeslot(this.dateWithTimeslotDto)
        .subscribe(response => {
          console.log('Timeslot created successfully:', response);
          this.openModal();
        }, err=>{
          const errorMessage = err.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
        });
    } else {
      this.service.updateTimeslot(this.dateWithTimeslotDto.timeslotId, this.dateWithTimeslotDto)
        .subscribe(response => {
          console.log('Timeslot updated successfully:', response);
          this.openModal();
        }, error=>{
          const errorMessage = error.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
        });
    }
  }

  goBack() {
    this.location.back();
  }

  Logout()
  {
    this.service.logout();
  }
}
