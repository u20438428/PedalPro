import { Component } from '@angular/core';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { UserViewModel } from 'src/app/Models/user-view-model';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: UserViewModel = {
    clientName:"",
    clientTitle:"",
    clientSurname:"",
    clientDateOfBirth: new Date(), // Initialize with the current date or a specific default date
    clientPhoneNum:"",
    clientPhysicalAddress:"",
    password:"",
    emailAddress:""
  }
  loading: boolean = false;

  isHelpVisible = false;
  passwordErrors: string[] = [];

  constructor(private dialog:MatDialog,private service: PedalProServiceService, private router:Router,private datePipe: DatePipe) {
    
   }

   getCurrentDate(): string {
    // Get the current date and format it as 'yyyy-MM-dd'
    return new Date().toISOString().split('T')[0];
  }

  getTwoYearsAgo(): string {
    const currentDate = new Date();
    currentDate.setFullYear(currentDate.getFullYear() - 2);
    
    // Format the date as 'yyyy-MM-dd'
    return currentDate.toISOString().split('T')[0];
  }

  registerUser(): void {
    this.loading = true;
    this.service.registerUser(this.user).subscribe(
      (response) => {
        
        this.loading = false;
        this.openModal();
        //this.router.navigate(['/login']);
      },
      (error) => {
        this.loading = false;
        const errorMessage = error.error || 'An error occurred';

        this.openErrorDialog(errorMessage); 
      }
    );
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
    if (password.length == 0) {
      this.passwordErrors.push('Password is required.');
    }
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

  cancel_continue(){
    this.router.navigate(['login']);
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
}
