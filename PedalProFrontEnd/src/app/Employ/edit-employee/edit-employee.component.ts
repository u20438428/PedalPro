import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PedalProServiceService } from '../../Services/pedal-pro-service.service';
import { Employee } from '../../Models/employee';
import { PedalProRole } from 'src/app/Models/pedal-pro-role';
import { EmployeeStatus } from '../../Models/employee-status';
import { EmployeeType } from '../../Models/employee-type';
import { UpdateEmployee } from 'src/app/Models/update-employee';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Component({
  selector: 'app-edit-employee',
  templateUrl: './edit-employee.component.html',
  styleUrls: ['./edit-employee.component.css']
})
export class EditEmployeeComponent implements OnInit{
  constructor(private dialog:MatDialog,private dataservice:PedalProServiceService, private router:Router, private route:ActivatedRoute){

  }

  editEmployees:UpdateEmployee={
    employeeId:0,
    empTitle:'',
    empName:'',
    empSurname:'',
    //empPhoneNum:'',
    empStatusId:0,
    empTypeId:0,
    //roleId:0,
    //username:'',
    //password:'',
    //emailAddress:''
  }

  Roles:PedalProRole[]=[];
  empStatuses:EmployeeStatus[]=[];
  empTypesTwo:EmployeeType[]=[];


  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next:(params)=>{
        const id=params.get('id');
        if(id)
        {
          this.dataservice.GetEmployee(id).subscribe({
            next:(response)=>{
              this.editEmployees=response;
            },
            error:(err)=>{
              const errorMessage = err.error || 'An error occurred';
              this.openErrorDialog(errorMessage);
            }
          })
        }
      }
    })

    this.GetEmpStatuses();
    this.GetTypes();
    
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  //Update Employee Details
  UpdateEmployee(){
    if(this.editEmployees.empName  && this.editEmployees.empSurname && this.editEmployees.empStatusId && this.editEmployees.empTitle &&  this.editEmployees.empTypeId)
    {
      this.dataservice.EditEmployee(this.editEmployees.employeeId,this.editEmployees).subscribe({
  
        
        
        next:(response)=>{
          this.openModal();
        },
        error:(err)=>{
          console.log(this.editEmployees);
          const errorMessage = err.error || 'An error occurred';
          this.openErrorDialog(errorMessage);
        }
      })
    }else{
      this.openErrorDialog('Validation error: Please fill in all fields.');
    }
    
  }// Redirect to View Employee Screen
  cancel_continue(){
    this.router.navigate(['viewEmployees'])
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



  GetEmpStatuses(){
    this.dataservice.GetEmployeeStatuses().subscribe(result=>{
      let empStatusList:any[]=result
      empStatusList.forEach((element)=>{
        this. empStatuses.push(element)
      });
    })
    return this.empStatuses;
  }
//Get Employee Types
  GetTypes(){
    this.dataservice.GetEmployeeTypes().subscribe(result=>{
      let empTypeList:any[]=result
      empTypeList.forEach((element)=>{
        this.empTypesTwo.push(element)
      });
    })
    return this.empTypesTwo;
  }

  Logout()
  {
    this.dataservice.logout();
  }
}
