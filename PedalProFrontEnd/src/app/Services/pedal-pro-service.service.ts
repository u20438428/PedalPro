import { Injectable } from '@angular/core';
import { of, tap } from 'rxjs';
import { CommonModule} from '@angular/common';
import { ModuleTwo } from '../Models/module-two';
import { PedalProRole } from '../Models/pedal-pro-role';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { TrainingModule } from '../Models/training-module';
import { TrainingMaterial } from '../Models/training-material';
import { VideoLink } from '../Models/video-link';
import { EmployeeType } from '../Models/employee-type';
import { EmployeeStatus } from '../Models/employee-status';
import { Employee } from '../Models/employee';
import { Price } from '../Models/price';
import { Package } from '../Models/package';
import { PackagePrice } from '../Models/package-price';
import { ClientType } from '../Models/client-type';
import { BicyclePart } from '../Models/bicycle-part';
import { BicycleCategory } from '../Models/bicycle-category';
import { BicycleBrand } from '../Models/bicycle-brand';
import { ImageType } from '../Models/image-type';
import { Bicycle } from '../Models/bicycle';
import { CustomDate } from '../Models/custom-date';
import { LoginUser } from '../Models/login-user';
import { User } from '../Models/user';
import { UserViewModel } from '../Models/user-view-model';
import { Booking } from '../Models/booking';
import { BookingType } from '../Models/booking-type';
import { Timeslot } from '../Models/timeslot';
import { Router } from '@angular/router';
import { UpdateEmployee } from '../Models/update-employee';
import { Workout } from '../Models/workout';
import { WorkoutType } from '../Models/workout-type';
import { ClientClient } from '../Models/client-client';
import { HelpCatergory } from '../Models/help-catergory';
import { Help } from '../Models/help';
import { Testingdate } from '../Models/testingdate';
import { Timeslotadd } from '../Models/timeslotadd';
import { DateWithTimeslotDto } from '../Models/date-with-timeslot-dto';
import { MaterialVid } from '../Models/material-vid';
import { AddCart } from '../Models/add-cart';
import { Cart } from '../Models/cart';
import { AddBrandTest } from '../Models/add-brand-test';
import { BrandImage } from '../Models/brand-image';
import { BrandTwo } from '../Models/brand-two';
import { Feedback } from '../Models/feedback';
import { FeedbackCatergory } from '../Models/feedback-catergory';
import { ComplexBooking } from '../Models/complex-booking';
import { catchError, retry, switchMap } from 'rxjs/operators';
import {  throwError, timer } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { LogoutDialogComponent } from '../Dialogs/logout-dialog/logout-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class PedalProServiceService {

  apiUrl='https://pedalproapi.azurewebsites.net/api/';

  constructor(private http: HttpClient,private router:Router,private dialog:MatDialog) {}

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  //Authentication starts
  
  registerUser(user: UserViewModel): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}Authentication/Register`, user);
  }
  updateDetails(userDetails: UserViewModel): Observable<any> {
    const url = `${this.apiUrl}AccountDetails/UpdateDetails`;
    return this.http.put(url, userDetails);
  }
  
  login(emailAddress: string, password: string): Observable<any> {
    const loginData = {
      emailAddress: emailAddress,
      password: password
    };

    return this.http.post<any>(`${this.apiUrl}Authentication/Login`, loginData).pipe(
      // Return the observable, the LoginComponent will handle the success
      tap((response) => {
        // Save the JWT token in local storage
        localStorage.setItem('jwt', response.value.token);
        localStorage.setItem('cartQuantity', '0');
        localStorage.setItem('cartId', '0');
        localStorage.setItem('bookingId', '0');
      })
    );
  }
  forgotPassword(emailAddress: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}Authentication/ForgotPassword?emailAddress=${emailAddress}`, null);
  }

  resetPassword(model: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}Authentication/ResetPassword`, model);
  }

  openErrorDialog(errorMessage: string, durationMs: number = 3000): void {
    const dialogRef = this.dialog.open(LogoutDialogComponent, {
      data: { message: errorMessage }
    });
  
    // Set a timeout to close the dialog after the specified duration (default is 3 seconds)
    setTimeout(() => {
      dialogRef.close();
    }, durationMs);
  }

  logout(): void {
    // Remove the JWT token from local storage
    localStorage.removeItem('jwt');
    this.router.navigate(['/login']); // Redirect to the register page
    localStorage.setItem('cartId', '0');
    localStorage.setItem('cartQuantity', '0');
    localStorage.setItem('bookingId', '0');
    const errorMessage = "You have been successfully logged out";
    this.openErrorDialog(errorMessage);
  }

  logouttwo(): void {
    // Remove the JWT token from local storage
    localStorage.removeItem('jwt');
    this.router.navigate(['/login']); // Redirect to the register page
    localStorage.setItem('cartId', '0');
    localStorage.setItem('cartQuantity', '0');
    localStorage.setItem('bookingId', '0');
    const errorMessage = "Lost Connection to the server. You have been logged out";
    this.openErrorDialog(errorMessage);
  }
  

  getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  isTokenValid(token: string): boolean {
    try {
      const tokenPayload = JSON.parse(atob(token.split('.')[1]));
      if (!tokenPayload.exp) {
        return false; // No expiration, consider it invalid
      }

      const expirationTime = new Date(tokenPayload.exp * 1000); // Convert to milliseconds
      const currentTime = new Date();

      return currentTime < expirationTime;
    } catch (error) {
      return false; // Token is invalid or cannot be parsed
    }
  }
  //Authentication ends


  GetRoles(): Observable<any>{
    return this.http.get(`${this.apiUrl}PedalProRole/GetAllRoles`)
    .pipe(map(result => result))
  }

  AddRole(addroles:PedalProRole):Observable<PedalProRole>{
    return this.http.post<PedalProRole>(`${this.apiUrl}PedalProRole/AddRoles`,addroles);
  }

  deleteRole(id:any):Observable<PedalProRole>{
    return this.http.delete<PedalProRole>(this.apiUrl+'PedalProRole/DeleteRole/'+id);
  }

  getRole(id:any):Observable<PedalProRole>{
    return this.http.get<PedalProRole>(this.apiUrl+'PedalProRole/GetRole/'+id)
  }

  updateRole(id:any, roleDetails:PedalProRole):Observable<PedalProRole>
  {
    return this.http.put<PedalProRole>(this.apiUrl+'PedalProRole/EditRole/'+id,roleDetails);
  }
  //End of pedalproroles

  

  //Start of Module Status
  GetStatuses(): Observable<any>{
    return this.http.get(`${this.apiUrl}TrainingModuleStatus/GetAllTrainingModuleStatuses`)
    .pipe(map(result => result))
  }
  //End of Module Status
  

  //Start of packages
  GetPackages(): Observable<any>{
    return this.http.get(`${this.apiUrl}Package/GetAllPackages`)
    .pipe(map(result => result))
  }

  handleHttpError(error: HttpErrorResponse) {
    // You can add your custom error handling logic here.
    return throwError('Something went wrong. Please try again later.');
  }

  AddPackage(addroles:Package):Observable<Package>{
    return this.http.post<Package>(`${this.apiUrl}Package/AddPackage`,addroles);
  }
  
  DeletePackage(id:any):Observable<Package>{
    return this.http.delete<Package>(this.apiUrl+'Package/DeletePackage/'+id);
  }

  GetPackage(id:any):Observable<Package>{
    return this.http.get<Package>(this.apiUrl+'Package/GetPackage/'+id)
  }

  EditPackage(id:any, roleDetails:Package):Observable<Package>
  {
    return this.http.put<Package>(this.apiUrl+'Package/EditPackage/'+id,roleDetails);
  }

  //End of packages


  //Start of prices
  GetPrice(id:any):Observable<Price>{
    return this.http.get<Price>(this.apiUrl+'Package/GetPrice/'+id)
  }
  //End of prices


  //Start of package prices
  GetPackagePrices(): Observable<any>{
    return this.http.get(`${this.apiUrl}Package/GetAllPackagePrices`)
    .pipe(map(result => result))
  }

  GetPackagePrice(id:any):Observable<PackagePrice>{
    return this.http.get<PackagePrice>(this.apiUrl+'Package/GetPackagePrice/'+id)
  }
  //End of package prices


  //Start of Training Modules
  GetModules(): Observable<any>{
    return this.http.get(`${this.apiUrl}TrainingModule/GetAllModules`)
    .pipe(map(result => result))
  }

  AddModule(addroles:TrainingModule):Observable<TrainingModule>{
    return this.http.post<TrainingModule>(`${this.apiUrl}TrainingModule/AddModule`,addroles);
  }
  
  DeleteModule(id:any):Observable<TrainingModule>{
    return this.http.delete<TrainingModule>(this.apiUrl+'TrainingModule/DeleteModule/'+id);
  }

  GetModuleTwo(id:any):Observable<TrainingModule>{
    return this.http.get<TrainingModule>(this.apiUrl+'TrainingModule/GetModule/'+id)
  }

  EditModule(id:any, roleDetails:TrainingModule):Observable<TrainingModule>
  {
    return this.http.put<TrainingModule>(this.apiUrl+'TrainingModule/EditModule/'+id,roleDetails);
  }
   //End of Training Modules


  //Start of Training Material
  AddMaterial(addroles:TrainingMaterial):Observable<TrainingMaterial>{
    return this.http.post<TrainingMaterial>(`${this.apiUrl}TrainingModule/AddTrainingMaterial`,addroles);
  }
  
  DeleteMaterial(id:any):Observable<TrainingMaterial>{
    return this.http.delete<TrainingMaterial>(this.apiUrl+'TrainingModule/DeleteTrainingMaterial/'+id);
  }

  GetMaterial(id:any):Observable<TrainingMaterial>{
    return this.http.get<TrainingMaterial>(this.apiUrl+'TrainingModule/GetTrainingMaterial/'+id)
  }

  EditMaterial(id:any, roleDetails:TrainingMaterial):Observable<TrainingMaterial>
  {
    return this.http.put<TrainingMaterial>(this.apiUrl+'TrainingModule/EditTrainingMaterial/'+id,roleDetails);
  }

  GetMaterials(): Observable<any>{
    return this.http.get(`${this.apiUrl}TrainingModule/GetAllTrainingMaterial`)
    .pipe(map(result => result))
  }
  //End of Training Material



  //Start of VideoTypes
  GetVideoTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}TrainingModule/GetAllVideoTypes`)
    .pipe(map(result => result))
  }
  //End of VideoTypes



  //Start of VideoLinks
  GetVideoLink(id:any):Observable<VideoLink>{
    return this.http.get<VideoLink>(this.apiUrl+'TrainingModule/GetVideoLink/'+id)
  }
  //End of VideoLinks


  //Start of employees
  GetEmployees(): Observable<any>{
    return this.http.get(`${this.apiUrl}Employee/GetAllEmployees`)
    .pipe(map(result => result))
  }
  GetEmployeesTwo(): Observable<any>{
    return this.http.get(`${this.apiUrl}Employee/GetAllEmployeeTwo`)
    .pipe(map(result => result))
  }

  AddEmployee(addroles:Employee):Observable<Employee>{
    return this.http.post<Employee>(`${this.apiUrl}Employee/AddEmployee`,addroles);
  }

  DeleteEmployee(id:any):Observable<Employee>{
    return this.http.delete<Employee>(this.apiUrl+'Employee/DeleteEmployee/'+id);
  }

  GetEmployee(id:any):Observable<Employee>{
    return this.http.get<Employee>(this.apiUrl+'Employee/GetEmployee/'+id)
  }

  EditEmployee(id:any, roleDetails:UpdateEmployee):Observable<UpdateEmployee>
  {
    return this.http.put<UpdateEmployee>(this.apiUrl+'Employee/EditEmployee/'+id,roleDetails);
  }
  //End of employees

  //Start of employee types
  GetEmployeetype(id:any):Observable<EmployeeType>{
    return this.http.get<EmployeeType>(this.apiUrl+'EmployeeType/GetEmployeeType/'+id)
  }

  restoreDatabase(): Observable<string> {
    const restoreUrl = `${this.apiUrl}Database/RestoreDatabase`; // Adjust the URL as needed

    return this.http.post<string>(restoreUrl, null);
  }

  GetEmployeeTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}EmployeeType/GetAllEmployeeTypes`)
    .pipe(map(result => result))
  }

  AddEmployeeType(addemptypes:EmployeeType):Observable<EmployeeType>{
    return this.http.post<EmployeeType>(`${this.apiUrl}EmployeeType/AddEmployeetypes`,addemptypes);
  }

  DeleteEmployeeType(id:any):Observable<EmployeeType>{
    return this.http.delete<EmployeeType>(this.apiUrl+'EmployeeType/DeleteEmployeeType/'+id);
  }

  EditEmployeeType(id:any, empTypeDetails:EmployeeType):Observable<EmployeeType>
  {
    return this.http.put<EmployeeType>(this.apiUrl+'EmployeeType/EditEmployeeType/'+id,empTypeDetails);
  }
  //End of employee types



  //Start of emp statuses
  GetEmployeeStatus(id:any):Observable<EmployeeStatus>{
    return this.http.get<EmployeeStatus>(this.apiUrl+'Employee/GetEmployeeStatus/'+id)
  }

  GetEmployeeStatuses(): Observable<any>{
    return this.http.get(`${this.apiUrl}Employee/GetAllEmployeeStatuses`)
    .pipe(map(result => result))
  }
  //End of emp statuses


  //Start of client types
  GetClientType(id:any):Observable<ClientType>{
    return this.http.get<ClientType>(this.apiUrl+'ClientType/GetAllClientType/'+id)
  }

  GetClientTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}ClientType/GetAllClientTypes`)
    .pipe(map(result => result))
  }

  AddClientType(addclienttypes:ClientType):Observable<ClientType>{
    return this.http.post<ClientType>(`${this.apiUrl}ClientType/AddClienttypes`,addclienttypes);
  }

  DeleteClientType(id:any):Observable<ClientType>{
    return this.http.delete<ClientType>(this.apiUrl+'ClientType/DeleteClientType/'+id);
  }

  EditClientType(id:any, clientTypeDetails:ClientType):Observable<ClientType>
  {
    return this.http.put<ClientType>(this.apiUrl+'ClientType/EditClientType/'+id,clientTypeDetails);
  }

  //End of client types


  //Start of bicycle parts
  GetBicyclePart(id:any):Observable<BicyclePart>{
    return this.http.get<BicyclePart>(this.apiUrl+'BicyclePart/GetBicyclePart/'+id)
  }

  GetBicycleParts(): Observable<any>{
    return this.http.get(`${this.apiUrl}BicyclePart/GetAllBicyclePart`)
    .pipe(map(result => result))
  }

  AddBicyclePart(addbicyclepart:BicyclePart):Observable<BicyclePart>{
    return this.http.post<BicyclePart>(`${this.apiUrl}BicyclePart/AddbicyclePart`,addbicyclepart);
  }

  DeleteBicyclePart(id:any):Observable<BicyclePart>{
    return this.http.delete<BicyclePart>(this.apiUrl+'BicyclePart/DeleteBicyclePart/'+id);
  }

  EditBicyclePart(id:any, bicyclePartDetails:BicyclePart):Observable<BicyclePart>
  {
    return this.http.put<BicyclePart>(this.apiUrl+'BicyclePart/EditbicyclePart/'+id,bicyclePartDetails);
  }
  //End of bicycle parts


  //Start of bicycle category
  GetBicycleCategory(id:any):Observable<BicycleCategory>{
    return this.http.get<BicycleCategory>(this.apiUrl+'BicycleCategory/GetAllBicycleCategory/'+id)
  }

  GetBicyclecategories(): Observable<any>{
    return this.http.get(`${this.apiUrl}BicycleCategory/GetAllBicycleCategories`)
    .pipe(map(result => result))
  }

  AddBicycleCategory(addbicyclecategory:BicycleCategory):Observable<BicycleCategory>{
    return this.http.post<BicycleCategory>(`${this.apiUrl}BicycleCategory/AddBicycleCategory`,addbicyclecategory);
  }

  DeleteBicycleCategory(id:any):Observable<BicycleCategory>{
    return this.http.delete<BicycleCategory>(this.apiUrl+'BicycleCategory/DeleteCategoryBicycle/'+id);
  }

  EditBicycleCategory(id:any, bicycleCategoryDetails:BicycleCategory):Observable<BicycleCategory>
  {
    return this.http.put<BicycleCategory>(this.apiUrl+'BicycleCategory/EditBicycleCategory/'+id,bicycleCategoryDetails);
  }
  //End of bicycle category


  //Start of bicycle brand
  GetBicycleBrand(id:any):Observable<BicycleBrand>{
    return this.http.get<BicycleBrand>(this.apiUrl+'BicycleBrand/GetBicycleBrand/'+id)
  }

  GetBicycleBrandTwo(id:any):Observable<BrandTwo>{
    return this.http.get<BrandTwo>(this.apiUrl+'BicycleBrand/GetBicycleBrand/'+id)
  }

  GetBicycleBrandImage(id:any):Observable<BrandImage>{
    return this.http.get<BrandImage>(this.apiUrl+'BicycleBrand/GetBrandImage/'+id)
  }

  GetBicycleBrands(): Observable<any>{
    return this.http.get(`${this.apiUrl}BicycleBrand/GetAllBicycleBrands`)
    .pipe(map(result => result))
  }

  AddBicycleBrand(addbicyclebrand:BicycleBrand):Observable<BicycleBrand>{
    return this.http.post<BicycleBrand>(`${this.apiUrl}BicycleBrand/AddBicycleBrand`,addbicyclebrand);
  }
  /*
  AddBicycleBrandTwo(brand: BicycleBrand, imageFile: FormData): Observable<BicycleBrand> {
    const url = `${this.apiUrl}BicycleBrand/AddBicycleBrandTwo`;
    return this.http.post<BicycleBrand>(url, imageFile, { params: { ...brand } });
  }*/

  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(`${this.apiUrl}BicycleBrand/TestImageUpload`, formData);
  }

  uploadProfileImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.put<any>(`${this.apiUrl}AccountDetails/UploadProfileImage`, formData);
  }

  AddBicycleBrandTwo(brand: AddBrandTest, file: File): Observable<BicycleBrand> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    formData.append('brandAdd', JSON.stringify(brand));

    return this.http.post<BicycleBrand>(`${this.apiUrl}BicycleBrand/AddBicycleBrandThree`, formData);
  }

  DeleteBicycleBrand(id:any):Observable<BicycleBrand>{
    return this.http.delete<BicycleBrand>(this.apiUrl+'BicycleBrand/DeleteBicycleBrand/'+id);
  }

  EditBicycleBrand(id:any, bicycleBrandDetails:BicycleBrand):Observable<BicycleBrand>
  {
    return this.http.put<BicycleBrand>(this.apiUrl+'BicycleBrand/EditBicycleBrand/'+id,bicycleBrandDetails);
  }
  //End of bicycle brand


  //Start of imagetype
  GetImageType(id:any):Observable<ImageType>{
    return this.http.get<ImageType>(this.apiUrl+'BicycleBrand/GetImageType/'+id)
  }

  GetImageTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}BicycleBrand/GetAllImageTypes`)
    .pipe(map(result => result))
  }
  //End of imagetype


  //Start of bicycle
  GetBicycle(id:any):Observable<Bicycle>{
    return this.http.get<Bicycle>(this.apiUrl+'Bicycle/GetBicycle/'+id)
  }

  GetBicycles(): Observable<any>{
    return this.http.get(`${this.apiUrl}Bicycle/GetAllBicycles`)
    .pipe(map(result => result))
  }

  AddBicycle(addbicyclebrand:Bicycle):Observable<Bicycle>{
    return this.http.post<Bicycle>(`${this.apiUrl}Bicycle/AddBicycle`,addbicyclebrand);
  }

  DeleteBicycle(id:any):Observable<Bicycle>{
    return this.http.delete<Bicycle>(this.apiUrl+'Bicycle/DeleteBicycle/'+id);
  }

  EditBicycle(id:any, bicycleBrandDetails:Bicycle):Observable<Bicycle>
  {
    return this.http.put<Bicycle>(this.apiUrl+'Bicycle/EditBicycle/'+id,bicycleBrandDetails);
  }
  //End of bicycle

  
  getDates(): Observable<CustomDate[]> {
    return this.http.get<CustomDate[]>(`${this.apiUrl}Schedule/GetDates`);
  }

  getDatesTwo(): Observable<CustomDate[]> {
    return this.http.get<CustomDate[]>(`${this.apiUrl}Schedule/GetDates`);
  }

  getTimeslots(dateId: number): Observable<Timeslot[]> {
    return this.http.get<Timeslot[]>(`${this.apiUrl}Schedule/GetTimeslotstwo/${dateId}`);
  }

  getTimeslotsTwo(date: Date): Observable<Timeslot[]> {
    return this.http.get<Timeslot[]>(`${this.apiUrl}Schedule/GetTimeslotsthree/${date}`);
  }

  AddBooking(addbooking:Booking):Observable<Booking>{
    return this.http.post<Booking>(`${this.apiUrl}Booking/AddBooking`,addbooking);
  }

  GetBookingTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}Booking/GetAllBookingTypes`)
    .pipe(map(result => result))
  }

  getDatesThree(): Observable<any[]> {
    // Simulating API call with mock data
    const mockDates = [
      { dateId: 1, date: '2023-07-18' },
      { dateId: 2, date: '2023-07-19' },
      { dateId: 3, date: '2023-07-20' },
      { dateId: 4, date: '2023-07-21' },
      { dateId: 5, date: '2023-07-22' },
      { dateId: 6, date: '2023-07-23' }
      // Add more dates here
    ];

    return of(mockDates);
  }


  //Workouts
  GetWorkouts(): Observable<any>{
    return this.http.get(`${this.apiUrl}Workout/GetAllWorkouts`)
    .pipe(map(result => result))
  }

  DeleteWorkout(id:any):Observable<Workout>{
    return this.http.delete<Workout>(this.apiUrl+'Workout/DeleteWorkout/'+id);
  }

  AddWorkout(addworkout:Workout):Observable<Workout>{
    return this.http.post<Workout>(`${this.apiUrl}Workout/AddWorkout`,addworkout);
  }

  //End of workouts


  //Workout Types
  GetWorkoutTypes(): Observable<any>{
    return this.http.get(`${this.apiUrl}Workout/GetAllWorkoutTypes`)
    .pipe(map(result => result))
  }

  GetWorkoutType(id:any):Observable<WorkoutType>{
    return this.http.get<WorkoutType>(this.apiUrl+'Workout/GetWorkoutType/'+id)
  }

  DeleteWorkoutType(id:any):Observable<WorkoutType>{
    return this.http.delete<WorkoutType>(this.apiUrl+'Workout/DeleteWorkoutType/'+id);
  }

  AddWorkoutType(addworkout:WorkoutType):Observable<WorkoutType>{
    return this.http.post<WorkoutType>(`${this.apiUrl}Workout/AddWorkoutType`,addworkout);
  }

  EditWorkoutType(id:any, bicycleBrandDetails:WorkoutType):Observable<WorkoutType>
  {
    return this.http.put<WorkoutType>(this.apiUrl+'Workout/EditWorkoutType/'+id,bicycleBrandDetails);
  }

  //End of workout type
  

  //Booking reminder and view client
  GetClientsClients(): Observable<any>{
    return this.http.get(`${this.apiUrl}PedalProUser/GetAllClients`)
    .pipe(map(result => result))
  }

  GetClientsClientsbookings(): Observable<any>{
    return this.http.get(`${this.apiUrl}PedalProUser/GetClientsWithBookings`)
    .pipe(map(result => result))
  }

 
  sendBookingReminder(clientId: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}PedalProUser/SendBookingReminder/${clientId}`, null);
  }

  sendBookingRemindertwo(clientId: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}PedalProUser/SendSms/${clientId}`, null);
  }


  //Start of help
  GetAllHelp(): Observable<any>{
    return this.http.get(`${this.apiUrl}Help/GetAllHelp`)
    .pipe(map(result => result))
  }

  AddHelp(addhelp:Help):Observable<Help>{
    return this.http.post<Help>(`${this.apiUrl}Help/AddHelp`,addhelp);
  }

  DeleteHelp(id:any):Observable<Help>{
    return this.http.delete<Help>(this.apiUrl+'Help/DeleteHelp/'+id);
  }

  GetHelp(id:any):Observable<Help>{
    return this.http.get<Help>(this.apiUrl+'Help/GetHelp/'+id)
  }

  UpdateHelp(id:any, helpDetails:Help):Observable<Help>
  {
    return this.http.put<Help>(this.apiUrl+'Help/EditHelp/'+id,helpDetails);
  }
  //End of Help

    //Start of Help Categories
    GetAllHelpCategories(): Observable<any>{
      return this.http.get(`${this.apiUrl}Help/GetAllHelpCategories`)
      .pipe(map(result => result))
    }
  
    
    GetHelpCategory(id:any):Observable<HelpCatergory>{
      return this.http.get<HelpCatergory>(this.apiUrl+'Help/GetHelpCategory/'+id)
    }

    //End of Categoriess


    

    getClientDetails(): Observable<any>{
      return this.http.get(`${this.apiUrl}PedalProUser/GetClientDetails`)
      .pipe(map(result => result))
    }

    getClientBookings(): Observable<any>{
      return this.http.get(`${this.apiUrl}Booking/GetAllBookings`)
      .pipe(map(result => result))
    }


    GetBookingType(id:any):Observable<BookingType>{
      return this.http.get<BookingType>(this.apiUrl+'BookingType/GetBookingType/'+id)
    }

    GetDateDate(id:any):Observable<Testingdate>{
      return this.http.get<Testingdate>(this.apiUrl+'Booking/GetDate/'+id)
    }

    
  
    AddBookingTypes(addbookingtypes:BookingType):Observable<BookingType>{
      return this.http.post<BookingType>(`${this.apiUrl}BookingType/AddBookingTypes`,addbookingtypes);
    }
  
    DeleteBookingType(id:any):Observable<BookingType>{
      return this.http.delete<BookingType>(this.apiUrl+'BookingType/DeleteBookingType/'+id);
    }
  
    
    EditBookingType(id:any, bookingTypeDetails:BookingType):Observable<BookingType>
    {
      return this.http.put<BookingType>(this.apiUrl+'BookingType/EditBookingType/'+id,bookingTypeDetails);
    }

    uploadDocument(file: File, title: string) {
      const formData: FormData = new FormData();
      formData.append('file', file, file.name);
      formData.append('title', title);

      const options = { headers: new HttpHeaders(), responseType: 'text' as 'json' };
  
      return this.http.post<any>(`${this.apiUrl}IndemnityForm/UploadDocument`, formData,options);
    }

    uploadDocumentClient(file: File, title: string) {
      const formData: FormData = new FormData();
      formData.append('file', file, file.name);
      formData.append('title', title);

      const options = { headers: new HttpHeaders(), responseType: 'text' as 'json' };
  
      return this.http.post<any>(`${this.apiUrl}IndemnityForm/ClientUploadDocument`, formData,options);
    }

    getLatestDocument(): Observable<Blob> {
      return this.http.get(`${this.apiUrl}IndemnityForm/GetLatestDocument`, {
        responseType: 'blob',
      });
    }

    getLatestPdfDocument(): Observable<Blob> {
      return this.http.get(`${this.apiUrl}IndemnityForm/GetUploadedDocument`, {
        responseType: 'blob',
      });
    }

    uploadImageIndemnity(formData: FormData) {
      return this.http.post(`${this.apiUrl}IndemnityForm/UploadImage`, formData);
    }

    //Start of Feedback
    GetAllFeedback(): Observable<any>{
      return this.http.get(`${this.apiUrl}Feedback/GetAllFeedback`)
      .pipe(map(result => result))
    }
  
    GetFeedback(id:any):Observable<Feedback>{
      return this.http.get<Feedback>(this.apiUrl+'Feedback/GetFeedback/'+id)
    }
  
    ProvideFeedback(providefeedback:Feedback):Observable<Feedback>{
      return this.http.post<Feedback>(`${this.apiUrl}Feedback/ProvideFeedback`,providefeedback);
    }
  
     //Start of Feedback Categories
    GetAllFeedbackCategories(): Observable<any>{
      return this.http.get(`${this.apiUrl}Feedback/GetAllFeedbackCategories`)
      .pipe(map(result => result))
    }

    GetFeedbackCategory(id:any):Observable<FeedbackCatergory>{
      return this.http.get<FeedbackCatergory>(this.apiUrl+'Feedback/GetFeedbackCategory/'+id)
    }

     //End of Feedback


     addBookingtwo(schedule: ComplexBooking): Observable<any> {
      return this.http.post(`${this.apiUrl}Booking/AddBooking`, schedule);
    }

    
    getLatestDocumenttwo(): Observable<ArrayBuffer> {
      return this.http.get(`${this.apiUrl}IndemnityForm/GetLatestDocument`, {
        responseType: 'arraybuffer',
      });
    }

    addTimeslot(dateWithTimeslotDto: DateWithTimeslotDto): Observable<any> {
      return this.http.post<any>(`${this.apiUrl}Schedule/AddTimeslot`, dateWithTimeslotDto);
    }

    

    

    DeleteTimeslot(id:any):Observable<DateWithTimeslotDto>{
      return this.http.delete<DateWithTimeslotDto>(this.apiUrl+'Schedule/DeleteTimeSlot/'+id);
    }

    
    
    updateTimeslot(id:any, helpDetails:DateWithTimeslotDto):Observable<DateWithTimeslotDto>
  {
    return this.http.put<DateWithTimeslotDto>(this.apiUrl+'Schedule/UpdateTimeslot/'+id,helpDetails);
  }

    getTimeslotById(id:any):Observable<DateWithTimeslotDto>{
      return this.http.get<DateWithTimeslotDto>(this.apiUrl+'Schedule/GetTimeSlot/'+id)
    }



    getMaterialVids(moduleId: number): Observable<MaterialVid[]> {
    return this.http.get<MaterialVid[]>(`${this.apiUrl}TrainingModule/GetAllTrainingContent/${moduleId}`);
  }


  addPackageToCart(packageId: number): Observable<any> {
    const cartId = parseInt(localStorage.getItem('cartId') || '0', 10);

    if (cartId === 0 || cartId === null) {
      // No cartId found in localStorage, let the backend create a new cart
      return this.http.post(`${this.apiUrl}Cart/AddPackageToCart`, { packageId });
    } else {
      // Call the backend with the retrieved cartId
      return this.http.post(`${this.apiUrl}Cart/AddPackageToCart`, { cartId, packageId });
    }
  }

  GetCart(id:any):Observable<Cart>{
    return this.http.get<Cart>(this.apiUrl+'Cart/GetCart/'+id)
  }

  initiatePayment(cartId: number): Observable<any> {
    const url = `${this.apiUrl}Checkout/initiate-payment?cartId=${cartId}`;
    return this.http.post<any>(url, {});
  }

  savePayment(cartId: number): Observable<any> {
    const url = `${this.apiUrl}Checkout/SavePayment?cartId=${cartId}`;
    return this.http.post<any>(url, {});
  }



  getWorkoutReportData(timeInterval: string): Observable<any> {
    const url = `${this.apiUrl}Report/generate?timeInterval=${timeInterval}`;
    return this.http.get(url);
  }

  getPackageReportData(): Observable<any> {
    const url = `${this.apiUrl}Report/GeneratePackageReport`;
    return this.http.get(url);
  }

  getRevenueportData(): Observable<any> {
    const url = `${this.apiUrl}Report/GenerateRevenueReport`;
    return this.http.get(url);
  }

  getReportData(): Observable<any> {
    const apiUrl = `${this.apiUrl}Report/GeneratePopularDaysReport`; 
    return this.http.get(apiUrl);
  }

  getbookingcheckouturl(bookingtypeid: number): Observable<any> {
    const url = `${this.apiUrl}Booking/GetBookingPaymentURL?bookingTypeid=${bookingtypeid}`;
    return this.http.post<any>(url, {});
  }

  saveBookingPayment(bookingId: number): Observable<any> {
    const url = `${this.apiUrl}Booking/SaveBookingPayment?bookingId=${bookingId}`;
    return this.http.post<any>(url, {});
  }

  deleteBooking(bookingId:number):Observable<any>{
    const url = `${this.apiUrl}Booking/DeleteBooking/${bookingId}`;
    return this.http.delete(url);
  }

  cancelBooking(bookingId: number, reason: string): Observable<any> {
    const url = `${this.apiUrl}Booking/CancelBooking/${bookingId}`;
    return this.http.delete(url, { params: { reason } });
  }

  getclientlistReportData(): Observable<any> {
    const apiUrl = `${this.apiUrl}Report/GenerateClientListReport`; 
    return this.http.get(apiUrl);
  }

  getpackagelistReportData(): Observable<any> {
    const apiUrl = `${this.apiUrl}Report/GeneratePackageListReport`; 
    return this.http.get(apiUrl);
  }

  getstaffReportData(): Observable<any> {
    const apiUrl = `${this.apiUrl}Report/GenerateStaffReport`; 
    return this.http.get(apiUrl);
  }

  getDateandTimeslot(id:number): Observable<any> {
    const apiUrl = `${this.apiUrl}Booking/GetDateandTimeBooking/`+id; 
    return this.http.get(apiUrl);
  }


  removePackageFromCart(cartId: number, packageId: number): Observable<Cart> {
    const url = `${this.apiUrl}Cart/RemovePackageFromCart`;
    const requestPayload = {
      cartId: cartId,
      packageId: packageId
    };

    return this.http.delete<Cart>(url, { body: requestPayload });
  }

  deactivateAccount(): Observable<any> {
    return this.http.put(`${this.apiUrl}AccountDetails/DeactivateMyAccount`, null);
  }

  reactivateAccount(emailAddress: string, password: string): Observable<any> {
    const loginData = {
      emailAddress: emailAddress,
      password: password
    };
    return this.http.put(`${this.apiUrl}Authentication/ReactivateAccount`, loginData);
  }

  getTimeslotsSlots(): Observable<any>{
    return this.http.get(`${this.apiUrl}Schedule/Gettimeslots`)
    .pipe(map(result => result))
  }


  GetEmployeeBookings(): Observable<any>{
    return this.http.get(`${this.apiUrl}Booking/GetAllEmployeeBookings`)
    .pipe(map(result => result))
  }

  SetAttendance(statusId: number, bookingId: number): Observable<any> {
    const url = `${this.apiUrl}Booking/SetAttendance/${statusId}/${bookingId}`;
    return this.http.put(url, null); 
  }

  GetEmployeeAttendance(): Observable<any>{
    return this.http.get(`${this.apiUrl}Booking/GetEmployeeAttendanceStatistics`)
    .pipe(map(result => result))
  }
  
  GetClientPayments(): Observable<any>{
    return this.http.get(`${this.apiUrl}Booking/GetAllClientPayments`)
    .pipe(map(result => result))
  }

  updateHoursBackup(hours: number): Observable<any> {
    const url = `${this.apiUrl}Database/UpdateHoursBackup/${hours}`;
    return this.http.put(url, null);
  }

  updateVAT(percentage: number): Observable<any> {
    const url = `${this.apiUrl}Vat/EditVAT/${percentage}`;
    return this.http.put(url, null);
  }

  getVAT(): Observable<any> {
    const url = `${this.apiUrl}Vat/GetVat`;
    return this.http.get(url);
  }

  GetIncomingPayments(): Observable<any>{
    return this.http.get(`${this.apiUrl}Booking/GetAllPayments`)
    .pipe(map(result => result))
  }


  getAllBicycleBrandsClient(bicycleCategoryId: number): Observable<any> {
    const url = `${this.apiUrl}BicycleBrand/GetAllBicycleBrandsClient/${bicycleCategoryId}`;
    return this.http.get(url);
  }

}
