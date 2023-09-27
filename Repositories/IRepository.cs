using PedalProAPI.Models;
using System;

namespace PedalProAPI.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        Task<bool> SaveChangesAsync();

        // Role
        /*
        Task<PedalProRole[]> GetAllRoleAsync();

        Task<PedalProRole> GetRoleAsync(int roleId);

        */

        Task<Schedule> GetEmployeeScheduleAsync(int employeeId);

        //Duplicate schecking
        Task<BrandImage> GetBrandImageByNameAsync(string name);

        Task<BicycleBrand> GetBicycleBrandByNameAsync(string name);

        Task<BicycleCategory> GetBicycleCategoryByNameAsync(string name);

        Task<BicyclePart> GetBicyclePartByNameAsync(string name);

        Task<BookingType> GetBookingTypeByNameAsync(string name);

        Task<ClientType> GetClientTypeByNameAsync(string name);

        Task<EmployeeType> GetEmployeeTypeByNameAsync(string name);

        Task<Package> GetPackageByNameAsync(string name);

        Task<Help> GetHelpByNameAsync(string name);

        Task<TrainingModule> GetModuleByNameAsync(string name);

        Task<TrainingMaterial> GetMaterialByNameAsync(string name);

        //End of duplicate checking



        Task<ClientPackage[]> GetClientPackagesAsync(int clientId);

        //Employee type
        Task<EmployeeType[]> GetAllEmployeeTypeAsync();

        Task<EmployeeType> GetEmployeeTypeAsync(int employeeTypeId);

        Task<BrandImage> GetbrandImageAsync(int brandImageId);

        Task<BicycleBrand[]> GetBrandCategoriesAsync(int categoryId);
        //Setup
        Task<Setup> GetSetup(int setupId);

        Task<Service> GetService(int serviceid);

        Task<TrainingSession> GettrainingSession(int sessionId);

        Task<BookingRevenue> GetBookingRevenue(string name);

        Task<Administrator> GetAdmin(string userId);

        Task<DatabaseTimer> GetDatabaseTimer(int timerId);
        Task<DatabaseTimer> GetDatabaseTimerTwo(int timerId);


        Task<Schedule> GetScheduleThird(int ScheduleId);
        //Booking
        Task<Booking[]> GetAllBookingAsync();

        Task<ClientIndemnityForm> GetUplaodedPdfDocument();

        Task<Booking> GetBookingAsync(int bookingId);

        Task<Refund[]> GetAllRefunds();

        //Employee
        Task<Employee[]> GetAllEmployeeAsync();

        Task<Employee> GetEmployeeAsync(int employeeId);

        Task<Booking> GetEmployeeBooking(int scheduleId);

        Task<Schedule[]> GetEmployeeBookingTwo(int employeeId);

        Task<BookingStatus> GetBookingStatus(int statusId);
        //
        Task<List<Workout>> GetWorkoutDataBetweenDates(DateTime startDate, DateTime endDate, int clientId);


        //Employee status 
        Task<EmployeeStatus[]> GetAllEmployeeStatusAsync();

        Task<EmployeeStatus> GetEmployeeStatusAsync(int employeeStatusId);


        //
        Task<Checkout[]> GetAllCheckouts();
        Task<VAT> GetVat(int vatId);


        Task<Payment[]> GetPayments();


        //User
        Task<PedalProUser> GetUserAsync(string userId);


        //Training module status
        Task<TrainingModuleStatus[]> GetAllTrainingModuleStatusAsync();

        Task<TrainingModuleStatus> GetTrainingModuleStatusAsync(int trainingModuleStatusId);


        //Training material
        Task<TrainingMaterial[]> GetAllTrainingMaterialAsync();

        Task<TrainingMaterial> GetTrainingMaterialAsync(int trainingMaterialId);


        //Training Module
        Task<TrainingModule[]> GetAllTrainingModuleAsync();

        Task<TrainingModule> GetTrainingModuleAsync(int trainingModuleId);


        //Video type
        Task<VideoType[]> GetAllVideoTypeAsync();


        //Package
        Task<Package> GetPackageAsync(int packageId);
        Task<Package[]> GetAllPackageAsync();
        Task<PackagePrice> GetPackageAssocAsync(int packageId);



        //Price
        Task<Price> GetPriceAsync(int priceId);

        Task<Price[]> GetPrices();
        Task<PackagePrice[]> GetAllPackagePriceAsync();


        //Video links
        Task<VideoLink> GetVideoLinkAsync(int videoLinkId);
        Task<VideoLink[]> GetTrainingMateVidAsync(int videolinkId);


        Task<BrandImage[]> GetImgBrandVidAsync(int brandImageId);



        //Other
        Task<TrainingMaterial[]> GetTrainingMaatModAsync(int moduleId);


        //Bicycle Part
        Task<BicyclePart[]> GetAllBicyclePartAsync();
        Task<BicyclePart> GetBicyclePartAsync(int bicycelPartId);


        //Client type
        Task<ClientType[]> GetAllClientTypeAsync();
        Task<ClientType> GetClientTypeAsync(int clientTypeId);
        
        
        
        // Bicycle Category
        Task<BicycleCategory> GetBicycleCategoryAsync(int bicycleCategoryId);
        Task<BicycleCategory[]> GetAllBicycleCategoryAsync();


        //BicycleBrand
        Task<BicycleBrand> GetBicycleBrandAsync(int bicycleBrandId);
        Task<BicycleBrand[]> GetAllBicycleBrandAsync();


        //BrandImage
        Task<BrandImage> GetBrandImageAsync(int brandImageId);
        Task<BrandImage[]> GetAllBrandImageAsync();


        //ImageType
        Task<ImageType> GetImageTypeAsync(int imageTypeId);
        Task<ImageType[]> GetAllImageTypeAsync();


        //Bicycle
        Task<Bicycle> GetBicycleAsync(int bicycleId);
        Task<Bicycle[]> GetAllBicycleAsync();
        Task<Bicycle[]> GetAllBicyclesAsyncTwo(int clientId);


        //Workouts
        Task<Workout[]> GetAllWorkoutsAsync(int clientId);

        //Workout types
        Task<WorkoutType> GetWorkoutType(int workoutTypeId);


        //ClientGet
        Task<Client> GetClient(string userId);



        //ScheduleGet
        Task<Schedule> GetSchedule(int scheduleId);

        Task<Schedule> GetScheduleSecond(int dateslotId);



        Task<Booking[]> GetAllBookingAsyncTwo(int clientId);



        //Dateslot for booking
        Task<DateSlot> GetDateslotFour(int timeslotId);


        //Booking types
        Task<BookingType[]> GetBookingTypes();

        Task<BookingType> GetBookingTypetwo(int bookingTypeId);

        //User3000
        Task<PedalProUser> GetUserFromEmp(string userId);

        Task<WorkoutType[]> GetAllWorkoutTypes();

        Task<Workout> GetWorkout(int workoutId);

        Task<Booking[]> GetBookingsReminder(int clientId);

        Task<Client> GetClientClient(int clientId);




        Task<BookingType[]> GetAllBookingTypeAsync();

        Task<BookingType> GetBookingTypeAsync(int bookingTypeId);

        //Help
        Task<Help[]> GetAllHelpAsync();

        Task<Help> GetHelpAsync(int helpId);


        //Help Category 
        Task<HelpCategory[]> GetAllHelpCategoriesAsync();

        Task<HelpCategory> GetHelpCategoryAsync(int helpCategoryId);

        Task<DateSlot> getDateBooking(int dateslotId);

        Task<Date> GetDateFive(int dateId);

        Task<IndemnityForm> GetLatestDocument();

        Task<Timeslot> GetTimeslotAsync(int timeslotId);

        Task<TrainingMaterial[]> GetTrainingMaterialsVid(int moduleId);


        Task<Feedback[]> GetAllFeedbackAsync();
        Task<Feedback> GetFeedbackAsync(int feedbackId);

        //FeedbackCategory
        Task<FeedbackCategory[]> GetAllFeedbackCategoriesAsync();
        Task<FeedbackCategory> GetFeedbackCategoryAsync(int feedbackcategoryId);


        //Cart
        Task<Cart> GetCartOne(int cartId);
        Task<Cart> GetCartWithPackages(int cartId);

        Task<PackageRevenue> GetPackageRevenue(string name);

        Task<Package> GetPackageName(string name);

        Task<PackageRevenue[]> GetAllPackageRevenuesAsync();

        Task<Schedule[]> GetAllSchedulesAsync();

        Task<BookingRevenue[]> GetAllBookingrevenue();

        Task<BookingType> GetBookingTypeName(string name);

        Task<Employee> GetEmployee(string userId);

        Task<Payment[]> GetClientPayments(int clientId);

        Task<Payment> GetPayment(int paymentId);

        Task<Checkout> GetPaymentCheckout(int paymentId);

    }
}
