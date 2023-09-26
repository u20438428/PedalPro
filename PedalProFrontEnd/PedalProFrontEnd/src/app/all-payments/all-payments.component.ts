import { Component,OnInit } from '@angular/core';
import { PedalProServiceService } from '../Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Component({
  selector: 'app-all-payments',
  templateUrl: './all-payments.component.html',
  styleUrls: ['./all-payments.component.css']
})
export class AllPaymentsComponent implements OnInit{

  paymentdata:any[]=[];
  searchTerm:string='';

  constructor(private service:PedalProServiceService,private dialog:MatDialog){}

  ngOnInit(): void {
    this.FetchClientPayments();
  }

  FetchClientPayments() {
    this.service.GetIncomingPayments().subscribe((data) => {
      this.paymentdata = data;
    },(error)=>{
      const errorsMessage = error.error;
      console.log(error);
      this.openErrorDialog(errorsMessage);
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

  filteredPayments(){
    return this.paymentdata.filter((employee:any)=>{
      const name=employee.client.toLowerCase();
      const paymentDate = employee.paymentDate;
      const transactionType=employee.transactionType.toLowerCase();
      const payfor=employee.paymentfor.toLowerCase();
      const term = this.searchTerm.toLowerCase();
      return name.includes(term) || paymentDate.includes(term)|| transactionType.includes(term)||payfor.includes(term);
    })
  }

}
