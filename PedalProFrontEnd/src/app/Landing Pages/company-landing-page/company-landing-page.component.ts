import { Component,OnInit } from '@angular/core';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { Employee } from 'src/app/Models/employee';
import { ChartDataset } from 'chart.js'; // Import the ChartDataSets type
import html2canvas from 'html2canvas';
import * as jsPDF from 'jspdf';
import { Feedback } from 'src/app/Models/feedback';
import { PackagePrice } from '../../Models/package-price';
import { Price } from '../../Models/price';
import { Package } from 'src/app/Models/package';
import { ClientClient } from 'src/app/Models/client-client';

@Component({
  selector: 'app-company-landing-page',
  templateUrl: './company-landing-page.component.html',
  styleUrls: ['./company-landing-page.component.css']
})
export class CompanyLandingPageComponent implements OnInit{
  empbookingdata:any={};
  empattendancedata:any={};
  reportDatatwo: any = {}; // Initialize as an empty object
  chartLabels = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  chartData: ChartDataset[] = []; // Use ChartDataSets type
  chartOptions: any = {
    responsive: true,
    maintainAspectRatio: false,
    elements: {
      arc: {
        backgroundColor: ['red', 'blue', 'green', 'black', 'purple', 'yellow', 'cyan']
      }
    }
  };
  
  disableBackButton: boolean = true;

  employees:Employee[]=[];

  latestEmployees: any[] = [];

  latestFeedback: any[] = [];

  latestPackages: any[] = [];

  clientfeedback:Feedback[]=[];

  clients:ClientClient[]=[];

  numclient:any=0
  numpackage:any=0;

  nuCli:any=0;

  packagePrices:PackagePrice[]=[];
  packages:Package[]=[];
  prices:Price[]=[];

  ngOnInit(): void {
    history.pushState({}, '', window.location.href);
    window.onpopstate = (event) => {
      if (this.disableBackButton) {
        event.preventDefault();
      }
    };

    this.preventBackButton();

    this.GetEmployees();
    this.GetAllFeedback();

    this.FetchEmpBookings();

    this.service.getReportData().subscribe(data => {
      // Convert numbers to ChartDataSets
      this.chartData = [{
        data: [
          data.sunday,
          data.monday,
          data.tuesday,
          data.wednesday,
          data.thursday,
          data.friday,
          data.saturday
        ],
        label: 'Bookings'
      }];
    });

    this.fetchWorkoutReportData();

    this.GetPackagePrices();
    this.GetClients();
    this.FetchEmpAttendance();
    
  }

  constructor(private service:PedalProServiceService){}
  
  GetClients()
  {
    this.service.GetClientsClientsbookings().subscribe(result=>{
      let roleList:any[]=result
      roleList.forEach((element)=>{
        this.clients.push(element)
      });
      if (roleList.length > 0) {
        // Reverse the employee list to get the latest employees first
        const reversedEmployeeList = roleList.reverse();
        this.nuCli=roleList.length
        // Get the last three employees
        
    } else {
        // If the employee list is empty, clear the latestEmployees variable
        
    }
    })
    return this.clients;
  }

  FetchEmpBookings() {
    this.service.GetEmployeeBookings().subscribe((data) => {
      this.empbookingdata = data.slice(0, 3);
    });
  }

  FetchEmpAttendance(){
    this.service.GetEmployeeAttendance().subscribe((data) => {
      this.empattendancedata = data;
    });
  }

  GetPackagePrices(){
    this.service.GetPackagePrices().subscribe(result=>{
      let packagePriceList:any[]=result
      packagePriceList.forEach((element)=>{
        this.packagePrices.push(element)
      });
      if (packagePriceList.length > 0) {
        // Reverse the employee list to get the latest employees first
        const reversedEmployeeList = packagePriceList.reverse();
        this.numpackage=packagePriceList.length
        // Get the last three employees
        this.latestPackages = reversedEmployeeList.slice(0, 3);
    } else {
        // If the employee list is empty, clear the latestEmployees variable
        this.latestPackages = [];
        this.numpackage=0;
    }
    })
    return this.packagePrices;
  }

  GetPackageName(id: any) {
    const packaged = this.packages.find(m => m.packageId === id);
  
    if (packaged) {
      return packaged.packageName;
    } else {
      this.service.GetPackage(id).subscribe(result => {
        this.packages.push(result);
        return result.packageName;
      },error=>{
        return "Brand does not exist"
      });
    }
    // add a return statement here to handle the case where the module is not found
    return undefined;
  }

  Logout()
  {
    this.service.logout();
  }

  preventBackButton(): void {
    history.replaceState(null, document.title, location.href);
    window.addEventListener('popstate', () => {
      history.pushState(null, document.title, location.href);
    });
  }

  GetEmployees(){
    this.service.GetEmployees().subscribe(result=>{
      let employeeList:any[]=result
      employeeList.forEach((element)=>{
        this. employees.push(element)
      });

      if (employeeList.length > 0) {
        // Reverse the employee list to get the latest employees first
        const reversedEmployeeList = employeeList.reverse();
        this.numclient=employeeList.length
        // Get the last three employees
        this.latestEmployees = reversedEmployeeList.slice(0, 3);
    } else {
        // If the employee list is empty, clear the latestEmployees variable
        this.latestEmployees = [];
        this.numclient=0;
    }
    })
    return this.employees;
  }

  fetchWorkoutReportData() {
    // Fetch workout report data from your backend API
    this.service.getReportData().subscribe((data) => {
      this.reportDatatwo = data.bookingInfo;
    });
  }

  GetAllFeedback()
  {
    this.service.GetAllFeedback().subscribe(result=>{
      let feedbackList:any[]=result
      feedbackList.forEach((element)=>{
        this.clientfeedback.push(element)
      });

      if (feedbackList.length > 0) {
        // Reverse the employee list to get the latest employees first
        const reversedEmployeeList = feedbackList.reverse();

        // Get the last three employees
        this.latestFeedback = reversedEmployeeList.slice(0, 3);
    } else {
        // If the employee list is empty, clear the latestEmployees variable
        this.latestFeedback = [];
    }
    })
    
    return this.clientfeedback;
    
  }

}
