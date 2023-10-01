
import { Component ,OnInit} from '@angular/core';
import { PedalProRole } from 'src/app/Models/pedal-pro-role';
import { TrainingModule } from '../../Models/training-module';
import { ModuleStatus } from '../../Models/module-status';
import { PedalProServiceService } from '../../Services/pedal-pro-service.service';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { NgModule } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { Bicycle } from 'src/app/Models/bicycle';
import { BicycleBrand } from 'src/app/Models/bicycle-brand';
import { BrandTwo } from 'src/app/Models/brand-two';
import { BicycleCategory } from 'src/app/Models/bicycle-category';
import { Workout } from 'src/app/Models/workout';
import { WorkoutType } from 'src/app/Models/workout-type';
import { switchMap } from 'rxjs/operators';
import { BookingType } from 'src/app/Models/booking-type';

@Component({
  selector: 'app-client-landing-page',
  templateUrl: './client-landing-page.component.html',
  styleUrls: ['./client-landing-page.component.css']
})
export class ClientLandingPageComponent implements OnInit{
  constructor(private dialog:MatDialog,private service:PedalProServiceService,private router:Router, private http:HttpClient){}
  modules:TrainingModule[]=[];
  clientDetails: any;
  disableBackButton: boolean = true;
  cartnumber:any;
  workoutType:WorkoutType[]=[];
  clientpaymentdata:any={};
  workouts:Workout[]=[];

  private hasPageReloaded = false; 
  numNum:number=0;
  bicycles:Bicycle[]=[];
  category:BicycleCategory[]=[];
  brand:BrandTwo[]=[];
  lastBicycle: any; 
  lastWorkout: any;
  private hasPageReloadedKey = 'hasPageReloaded';
  categor:BookingType[]=[];
  clientTypes:any[]=[];
  

  ngOnInit(): void {
    this.GetModules();
    this.fetchClientDetails();
    this.GetBicycles();
    this.GetWorkouts();
    this.FetchClientPayments();
    this.GetBookings();

    const hasReloaded = localStorage.getItem(this.hasPageReloadedKey);

    if (!hasReloaded) {

      window.location.reload();


      localStorage.setItem(this.hasPageReloadedKey, 'true');
    }


    history.pushState({}, '', window.location.href);
    window.onpopstate = (event) => {
      if (this.disableBackButton) {
        event.preventDefault();
      }
    };

    this.preventBackButton();

    const storedCartQuantity = localStorage.getItem('cartQuantity');
    this.cartnumber = storedCartQuantity ? parseInt(storedCartQuantity, 10) : 0;

    
    

  }

  GetBookings() {
    this.service.getClientBookings().subscribe((result) => {
      let count = 0; 
  
      result.forEach((element: { scheduleId: number; dateDate: any; timeTime: any; }) => {
        if (count < 1) { 
          this.service.getDateandTimeslot(element.scheduleId).subscribe((nullDate) => {
            element.dateDate = nullDate.dateDate;
            element.timeTime = nullDate.timeTime;
            this.clientTypes.push(element);
          });
          count++;
        }
      });
    });
  
    return this.clientTypes; 
  }
  GetModules(){
    this.service.GetModules().subscribe(result=>{
      let moduleList:any[]=result
      moduleList.forEach((element)=>{
        this. modules.push(element)
      });
    })
    return this.modules;
  }

  GetType(id: any) {
    const categories = this.categor.find(m => m.bookingTypeId === id);
  
    if (categories) {
      return categories.bookingTypeName;
    } else {
      this.service.GetBookingType(id).subscribe(result => {
      
        
        this.categor.push(result);
        return result.bookingTypeName;
      });
    }
    return 'Booking Type does not exist';
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  preventBackButton(): void {
    history.replaceState(null, document.title, location.href);
    window.addEventListener('popstate', () => {
      history.pushState(null, document.title, location.href);
    });
  }

 

  Logout()
  {
    this.service.logout();
  }
  fetchClientDetails() {
    this.service.getClientDetails().subscribe(
      (response) => {
        this.clientDetails = response;
      },
      (err)=>{
        const errorMessage = err.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
  }

  GetBicycles()
  {
    this.service.GetBicycles().subscribe(result=>{
      let bicycleList:any[]=result
      bicycleList.forEach((element)=>{
        this.bicycles.push(element)
      });
      this.lastBicycle = this.bicycles.pop();

      
    })
    
    return this.bicycles;
    
  }

  GetCategory(id: any) {
    const categories = this.category.find(m => m.bicycleCategoryId === id);
  
    if (categories) {
      return categories.bicycleCategoryName;
    } else {
      this.service.GetBicycleCategory(id).subscribe(result => {
        this.category.push(result);
        return result.bicycleCategoryName;
      });
    }
  
    return 'Category does not exist';
  }

  GetBrand(id: any) {
    const brand = this.brand.find(m => m.bicycleBrandId === id);
  
    if (brand) {
      return brand.brandName;
    } else {
      this.service.GetBicycleBrandTwo(id).subscribe(result => {
        const brandImageId = result.brandImageId;
        this.service.GetBicycleBrandImage(brandImageId).subscribe(imageResult => {
          result.brandName = imageResult.imageUrl; 
          this.brand.push(result);
          return this.brand;
        },error=>{
          return "Brand does not exist"
        });
      });
  
      return undefined;
    }
  }

  

  GetWorkouts()
  {
    this.service.GetWorkouts().subscribe(result=>{
      let workoutList:any[]=result
      workoutList.forEach((element)=>{
        this.workouts.push(element)
      });

      this.lastWorkout=this.workouts.pop();
    })
    
    return this.workouts;
    
  }

  GetWorkoutType(id: any) {
    const categories = this.workoutType.find(m => m.workoutTypeId === id);
  
    if (categories) {
      return categories.workoutTypeName;
    } else {
      this.service.GetWorkoutType(id).subscribe(result => {
        this.workoutType.push(result);
        return result.workoutTypeName;
      });
    }
  
    return 'Type does not exist';
  }

  convertDurationFromTimespan(timespanString: string): string {
    const [hours, minutes, seconds] = timespanString.split(':');
    return `${hours}h ${minutes}m ${seconds}s`;
  }
  
  FetchClientPayments() {
    this.service.GetClientPayments().subscribe((data) => {
      this.clientpaymentdata = data.slice(0, 1);
    });
  }


  
  

}
