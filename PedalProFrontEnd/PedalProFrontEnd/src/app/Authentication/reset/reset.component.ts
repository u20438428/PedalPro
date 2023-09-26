import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';


@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class ResetComponent implements OnInit{
  resetPasswordForm!: FormGroup;
  submitted = false;
  errorMessage: string = "";

  constructor(
    private formBuilder: FormBuilder,
    private dialog:MatDialog,
    private route: ActivatedRoute,
    private router: Router,
    private service: PedalProServiceService,private location: Location
  ) { }

  ngOnInit(): void {
    this.resetPasswordForm = this.formBuilder.group({
      code: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(7)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(7)]]
    });
  }

  get f() { return this.resetPasswordForm.controls; }

  
  onSubmit(): void {
    

    if (this.f['password'].value !== this.f['confirmPassword'].value) {
      
      const errorsMessage = "Passwords do not match";
      this.openErrorDialog(errorsMessage);
          
    }
    else{
      this.service.resetPassword(this.resetPasswordForm.value)
      .subscribe(
        response => {
          // Password reset successful, redirect to login page or any other appropriate page
          this.openModal();
        },
        error => {
          const errorsMessage = error.error;
          this.openErrorDialog(errorsMessage);
        }
      );
    }

    
  }

  goBack(): void {
    this.location.back();
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
}
