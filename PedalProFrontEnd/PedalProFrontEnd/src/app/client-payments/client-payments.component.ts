import { Component,OnInit } from '@angular/core';
import { PedalProServiceService } from '../Services/pedal-pro-service.service';

@Component({
  selector: 'app-client-payments',
  templateUrl: './client-payments.component.html',
  styleUrls: ['./client-payments.component.css']
})
export class ClientPaymentsComponent implements OnInit{

  constructor(private service:PedalProServiceService){}

  ngOnInit(): void {
  this.FetchClientPayments();
  }

  clientpaymentdata:any={};
  searchTerm:string='';


  FetchClientPayments() {
    this.service.GetClientPayments().subscribe((data) => {
      this.clientpaymentdata = data;
    });
  }

  filteredPayments(){
    return this.clientpaymentdata.filter((employee:any)=>{
      const name=employee.client.toLowerCase();
      const paymentDate = employee.paymentDate;
      const transactionType=employee.transactionType.toLowerCase();
      const payfor=employee.paymentfor.toLowerCase();
      const term = this.searchTerm.toLowerCase();
      return name.includes(term) || paymentDate.includes(term)|| transactionType.includes(term)||payfor.includes(term);
    })
  }

}
