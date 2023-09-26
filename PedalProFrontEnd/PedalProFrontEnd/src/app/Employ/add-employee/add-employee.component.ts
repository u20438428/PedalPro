import { Component, OnInit } from '@angular/core';
import { PedalProServiceService } from '../../Services/pedal-pro-service.service';
import { Employee } from '../../Models/employee';
import { Router } from '@angular/router';
import { PedalProRole } from 'src/app/Models/pedal-pro-role';
import { EmployeeStatus } from '../../Models/employee-status';
import { EmployeeType } from '../../Models/employee-type';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent implements OnInit{
  constructor(private dialog:MatDialog,private dataservice:PedalProServiceService,private router:Router){}
  isHelpVisible = false;

  passwordErrors: string[] = [];
  //Employee Array
  addEmployees:Employee={
    employeeId:0,
    empTitle:'',
    empName:'',
    empSurname:'',
    empPhoneNum:'',
    empStatusId:0,
    empTypeId:0,
    //roleId:0,
    //username:'',
    password:'',
    emailAddress:''
  }

  //Role Array
  Roles:PedalProRole[]=[];
  empStatuses:EmployeeStatus[]=[];
  empTypesTwo:EmployeeType[]=[];

  ngOnInit(): void {
    this.GetEmpStatuses();
    this.GetTypes();
    
  }
  toggleHelp() {
    const modelDiv = document.getElementById('helphelp');
    if (modelDiv != null) {
      if (modelDiv.style.display === 'block') {
        modelDiv.style.display = 'none';
      } else {
        modelDiv.style.display = 'block';
      }
    }
  }
  
  validatePassword(password: string) {
    this.passwordErrors = [];

    if (!/[A-Z]/.test(password)) {
      this.passwordErrors.push('Password must contain at least one capital letter.');
    }

    if (!/\d/.test(password)) {
      this.passwordErrors.push('Password must contain at least one number.');
    }

    if (password.length < 7) {
      this.passwordErrors.push('Password must be at least 7 characters long.');
    }
    if (password=='') {
      this.passwordErrors.push('Password is required.');
    }
  }

  //Employee Status
  GetEmpStatuses(){
    this.dataservice.GetEmployeeStatuses().subscribe(result=>{
      let empStatusList:any[]=result
      empStatusList.forEach((element)=>{
        this. empStatuses.push(element)
      });
    })
    return this.empStatuses;
  }

  // Get Employee Type
  GetTypes(){
    this.dataservice.GetEmployeeTypes().subscribe(result=>{
      let empTypeList:any[]=result
      empTypeList.forEach((element)=>{
        this.empTypesTwo.push(element)
      });
    })
    return this.empTypesTwo;
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

//Add An Employee to the system
  addEmployee(){
    if(this.addEmployees.empName && this.addEmployees.emailAddress && this.addEmployees.empPhoneNum && this.addEmployees.empSurname && this.addEmployees.empStatusId && this.addEmployees.empTitle && this.addEmployees.password && this.addEmployees.empTypeId)
    {
      this.dataservice.AddEmployee(this.addEmployees).subscribe({
        next:(course)=>{
          this.openModal();
        },
        error:(err)=>{
          const errorMessage = err.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
        }
      });
    }else{
      this.openErrorDialog('Validation error: Please fill in all fields.');
    }
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

  Logout()
  {
    this.dataservice.logout();
  }
}
