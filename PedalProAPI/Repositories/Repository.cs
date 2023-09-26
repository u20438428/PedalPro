using Google.Api;
using Microsoft.EntityFrameworkCore;
using PedalProAPI.Context;
using PedalProAPI.Models;


namespace PedalProAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly PedalProDbContext _pedalProDbContext;

        public Repository(PedalProDbContext pedalProDbContext)
        {
            _pedalProDbContext = pedalProDbContext;
        }

        //Other
        public void Add<T>(T entity) where T : class
        {
            _pedalProDbContext.Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _pedalProDbContext.AddRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            _pedalProDbContext.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _pedalProDbContext.SaveChangesAsync() > 0;
        }


        

        public async Task<ClientPackage[]> GetClientPackagesAsync(int clientId)
        {
            IQueryable<ClientPackage> query = _pedalProDbContext.ClientPackages.Where(c => c.ClientId == clientId);
            return await query.ToArrayAsync();
        }


        public async Task<BicycleBrand[]> GetBrandCategoriesAsync(int categoryId)
        {
            IQueryable<BicycleBrand> query = _pedalProDbContext.BicycleBrands.Where(c => c.BicycleCategoryId == categoryId);
            return await query.ToArrayAsync();
        }


        //Duplicate schecking
        public async Task<BrandImage> GetBrandImageByNameAsync(string name)
        {
            IQueryable<BrandImage> query = _pedalProDbContext.BrandImages.Where(c => c.BrandImgName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BicycleBrand> GetBicycleBrandByNameAsync(string name)
        {
            IQueryable<BicycleBrand> query = _pedalProDbContext.BicycleBrands.Where(c => c.BrandName == name);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<BicycleCategory> GetBicycleCategoryByNameAsync(string name)
        {
            IQueryable<BicycleCategory> query = _pedalProDbContext.BicycleCategories.Where(c => c.BicycleCategoryName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BicyclePart> GetBicyclePartByNameAsync(string name)
        {
            IQueryable<BicyclePart> query = _pedalProDbContext.BicycleParts.Where(c => c.BicyclePartName == name);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<BookingType> GetBookingTypeByNameAsync(string name)
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes.Where(c => c.BookingTypeName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Schedule> GetEmployeeScheduleAsync(int employeeId)
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules.Where(c => c.EmployeeId == employeeId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<ClientType> GetClientTypeByNameAsync(string name)
        {
            IQueryable<ClientType> query = _pedalProDbContext.ClientTypes.Where(c => c.ClientTypeName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<EmployeeType> GetEmployeeTypeByNameAsync(string name)
        {
            IQueryable<EmployeeType> query = _pedalProDbContext.EmployeeTypes.Where(c => c.EmpTypeName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Package> GetPackageByNameAsync(string name)
        {
            IQueryable<Package> query = _pedalProDbContext.Packages.Where(c => c.PackageName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Help> GetHelpByNameAsync(string name)
        {
            IQueryable<Help> query = _pedalProDbContext.Helps.Where(c => c.HelpName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TrainingModule> GetModuleByNameAsync(string name)
        {
            IQueryable<TrainingModule> query = _pedalProDbContext.TrainingModules.Where(c => c.TrainingModuleName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TrainingMaterial> GetMaterialByNameAsync(string name)
        {
            IQueryable<TrainingMaterial> query = _pedalProDbContext.TrainingMaterials.Where(c => c.TrainingMaterialName == name);
            return await query.FirstOrDefaultAsync();
        }


        //End of duplicate checking


        //Booking type

        public async Task<BookingType[]> GetBookingTypes()
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes;
            return await query.ToArrayAsync();
        }


        //EmployeeType
        public async Task<EmployeeType[]> GetAllEmployeeTypeAsync()
        {
            IQueryable<EmployeeType> query = _pedalProDbContext.EmployeeTypes;
            return await query.ToArrayAsync();
        }

        public async Task<EmployeeType> GetEmployeeTypeAsync(int employeeTypeId)
        {
            IQueryable<EmployeeType> query = _pedalProDbContext.EmployeeTypes.Where(c => c.EmpTypeId == employeeTypeId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BrandImage> GetbrandImageAsync(int brandImageId)
        {
            IQueryable<BrandImage> query = _pedalProDbContext.BrandImages.Where(c => c.BrandImageId == brandImageId);
            return await query.FirstOrDefaultAsync();
        }


        //Booking
        public async Task<Booking[]> GetAllBookingAsync()
        {
            IQueryable<Booking> query = _pedalProDbContext.Bookings;
            return await query.ToArrayAsync();
        }

        public async Task<Booking> GetBookingAsync(int bookingId)
        {
            IQueryable<Booking> query = _pedalProDbContext.Bookings.Where(c => c.BookingId == bookingId);
            return await query.FirstOrDefaultAsync();
        }



        //Employee
        public async Task<Employee[]> GetAllEmployeeAsync()
        {
            IQueryable<Employee> query = _pedalProDbContext.Employees;
            return await query.ToArrayAsync();
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            IQueryable<Employee> query = _pedalProDbContext.Employees.Where(c => c.EmployeeId == employeeId);
            return await query.FirstOrDefaultAsync();
        }

        

        //Employee Status
        public async Task<EmployeeStatus[]> GetAllEmployeeStatusAsync()
        {
            IQueryable<EmployeeStatus> query = _pedalProDbContext.EmployeeStatuses;
            return await query.ToArrayAsync();
        }

        public async Task<EmployeeStatus> GetEmployeeStatusAsync(int employeeStatusId)
        {
            IQueryable<EmployeeStatus> query = _pedalProDbContext.EmployeeStatuses.Where(c => c.EmpStatusId == employeeStatusId);
            return await query.FirstOrDefaultAsync();
        }




        //PedalProUser
        public async Task<PedalProUser> GetUserAsync(string userId)
        {
            IQueryable<PedalProUser> query = _pedalProDbContext.PedalProUsers.Where(c => c.Id == userId);
            return await query.FirstOrDefaultAsync();
        }


        //Training Module Status
        public async Task<TrainingModuleStatus[]> GetAllTrainingModuleStatusAsync()
        {
            IQueryable<TrainingModuleStatus> query = _pedalProDbContext.TrainingModuleStatuses;
            return await query.ToArrayAsync();
        }

        public async Task<TrainingModuleStatus> GetTrainingModuleStatusAsync(int trainingModuleStatusId)
        {
            IQueryable<TrainingModuleStatus> query = _pedalProDbContext.TrainingModuleStatuses.Where(c => c.TrainingModuleStatusId == trainingModuleStatusId);
            return await query.FirstOrDefaultAsync();
        }



        //Training Material
        public async Task<TrainingMaterial[]> GetAllTrainingMaterialAsync()
        {
            IQueryable<TrainingMaterial> query = _pedalProDbContext.TrainingMaterials;
            return await query.ToArrayAsync();
        }

        public async Task<TrainingMaterial> GetTrainingMaterialAsync(int trainingMaterialId)
        {
            IQueryable<TrainingMaterial> query = _pedalProDbContext.TrainingMaterials.Where(c => c.TrainingMaterialId == trainingMaterialId);
            return await query.FirstOrDefaultAsync();
        }




        //Other training mat/mod
        public async Task<TrainingMaterial[]> GetTrainingMaatModAsync(int moduleId)
        {
            IQueryable<TrainingMaterial> query = _pedalProDbContext.TrainingMaterials.Where(c => c.TrainingModuleId == moduleId);
            return await query.ToArrayAsync();
        }




        //Training Module
        public async Task<TrainingModule[]> GetAllTrainingModuleAsync()
        {
            IQueryable<TrainingModule> query = _pedalProDbContext.TrainingModules;
            return await query.ToArrayAsync();
        }

        public async Task<TrainingModule> GetTrainingModuleAsync(int trainingModuleId)
        {
            IQueryable<TrainingModule> query = _pedalProDbContext.TrainingModules.Where(c => c.TrainingModuleId == trainingModuleId);
            return await query.FirstOrDefaultAsync();
        }



        //Video types
        public async Task<VideoType[]> GetAllVideoTypeAsync()
        {
            IQueryable<VideoType> query = _pedalProDbContext.VideoTypes;
            return await query.ToArrayAsync();
        }



        //Packages
        public async Task<Package[]> GetAllPackageAsync()
        {
            IQueryable<Package> query = _pedalProDbContext.Packages;
            return await query.ToArrayAsync();
        }
        public async Task<Package> GetPackageAsync(int packageId)
        {
            IQueryable<Package> query = _pedalProDbContext.Packages.Where(c => c.PackageId == packageId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PackagePrice> GetPackageAssocAsync(int packageId)
        {
            IQueryable<PackagePrice> query = _pedalProDbContext.PackagePrices.Where(c => c.PackageId == packageId);
            return await query.FirstOrDefaultAsync();
        }


        //Price
        public async Task<Price> GetPriceAsync(int priceId)
        {
            IQueryable<Price> query = _pedalProDbContext.Prices.Where(c => c.PriceId == priceId);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<PackagePrice[]> GetAllPackagePriceAsync()
        {

            IQueryable<PackagePrice> query = _pedalProDbContext.PackagePrices;
            return await query.ToArrayAsync();
        }

        public async Task<Price[]> GetPrices()
        {

            IQueryable<Price> query = _pedalProDbContext.Prices;
            return await query.ToArrayAsync();
        }


        //Video Links
        public async Task<VideoLink> GetVideoLinkAsync(int videoLinkId)
        {
            IQueryable<VideoLink> query = _pedalProDbContext.VideoLinks.Where(c => c.VideoLinkId == videoLinkId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<VideoLink[]> GetTrainingMateVidAsync(int videolinkId)
        {
            IQueryable<VideoLink> query = _pedalProDbContext.VideoLinks.Where(c => c.VideoLinkId == videolinkId);
            return await query.ToArrayAsync();
        }

        public async Task<BrandImage[]> GetImgBrandVidAsync(int brandImageId)
        {
            IQueryable<BrandImage> query = _pedalProDbContext.BrandImages.Where(c => c.BrandImageId == brandImageId);
            return await query.ToArrayAsync();
        }


        


        //Bicycle part
        public async Task<BicyclePart> GetBicyclePartAsync(int bicyclePartId)
        {
            IQueryable<BicyclePart> query = _pedalProDbContext.BicycleParts.Where(c => c.BicyclePartId == bicyclePartId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BicyclePart[]> GetAllBicyclePartAsync()
        {
            IQueryable<BicyclePart> query = _pedalProDbContext.BicycleParts;
            return await query.ToArrayAsync();
        }



        //Client Types
        public async Task<ClientType[]> GetAllClientTypeAsync()
        {
            IQueryable<ClientType> query = _pedalProDbContext.ClientTypes;
            return await query.ToArrayAsync();
        }

        public async Task<ClientType> GetClientTypeAsync(int clientTypeId)
        {
            IQueryable<ClientType> query = _pedalProDbContext.ClientTypes.Where(c => c.ClientTypeId == clientTypeId);
            return await query.FirstOrDefaultAsync();
        }


        //Bicycle category
        public async Task<BicycleCategory[]> GetAllBicycleCategoryAsync()
        {
            IQueryable<BicycleCategory> query = _pedalProDbContext.BicycleCategories;
            return await query.ToArrayAsync();
        }


        public async Task<BicycleCategory> GetBicycleCategoryAsync(int bicycleCategoryId)
        {
            IQueryable<BicycleCategory> query = _pedalProDbContext.BicycleCategories.Where(c => c.BicycleCategoryId == bicycleCategoryId);
            return await query.FirstOrDefaultAsync();
        }

        //BicycleBrand
        public async Task<BicycleBrand[]> GetAllBicycleBrandAsync()
        {
            IQueryable<BicycleBrand> query = _pedalProDbContext.BicycleBrands;
            return await query.ToArrayAsync();
        }


        public async Task<BicycleBrand> GetBicycleBrandAsync(int bicycleBrandId)
        {
            IQueryable<BicycleBrand> query = _pedalProDbContext.BicycleBrands.Where(c => c.BicycleBrandId == bicycleBrandId);
            return await query.FirstOrDefaultAsync();
        }


        //BrandImage
        public async Task<BrandImage[]> GetAllBrandImageAsync()
        {
            IQueryable<BrandImage> query = _pedalProDbContext.BrandImages;
            return await query.ToArrayAsync();
        }


        public async Task<BrandImage> GetBrandImageAsync(int brandImageId)
        {
            IQueryable<BrandImage> query = _pedalProDbContext.BrandImages.Where(c => c.BrandImageId == brandImageId);
            return await query.FirstOrDefaultAsync();
        }


        //ImageType
        public async Task<ImageType[]> GetAllImageTypeAsync()
        {
            IQueryable<ImageType> query = _pedalProDbContext.ImageTypes;
            return await query.ToArrayAsync();
        }


        public async Task<ImageType> GetImageTypeAsync(int imageTypeId)
        {
            IQueryable<ImageType> query = _pedalProDbContext.ImageTypes.Where(c => c.ImageTypeId == imageTypeId);
            return await query.FirstOrDefaultAsync();
        }


        //Bicycle
        public async Task<Bicycle[]> GetAllBicycleAsync()
        {
            IQueryable<Bicycle> query = _pedalProDbContext.Bicycles;
            return await query.ToArrayAsync();
        }

        public async Task<Bicycle[]> GetAllBicyclesAsyncTwo(int clientId)
        {
            IQueryable<Bicycle> query = _pedalProDbContext.Bicycles.Where(b => b.ClientId == clientId);
            return await query.ToArrayAsync();
        }


        public async Task<Bicycle> GetBicycleAsync(int bicycleId)
        {
            IQueryable<Bicycle> query = _pedalProDbContext.Bicycles.Where(c => c.BicycleId == bicycleId);
            return await query.FirstOrDefaultAsync();
        }


        //Workout
        public async Task<Workout[]> GetAllWorkoutsAsync(int clientId)
        {
            IQueryable<Workout> query = _pedalProDbContext.Workouts.Where(b => b.ClientId == clientId);
            return await query.ToArrayAsync();
        }

        public async Task<Refund[]> GetAllRefunds()
        {
            IQueryable<Refund> query = _pedalProDbContext.Refunds;
            return await query.ToArrayAsync();
        }

        //ClientGet
        public async Task<Client> GetClient(string userId)
        {
            IQueryable<Client> query = _pedalProDbContext.Clients.Where(c=>c.UserId==userId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployee(string userId)
        {
            IQueryable<Employee> query = _pedalProDbContext.Employees.Where(c => c.UserId == userId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Administrator> GetAdmin(string userId)
        {
            IQueryable<Administrator> query = _pedalProDbContext.Administrators.Where(c => c.UserId == userId);
            return await query.FirstOrDefaultAsync();
        }

        //ScheduleGet
        public async Task<Schedule> GetSchedule(int scheduleId)
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules.Where(c => c.ScheduleId == scheduleId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<DateSlot> getDateBooking(int dateslotId)
        {
            IQueryable<DateSlot> query = _pedalProDbContext.DateSlots.Where(c => c.DateSlotId == dateslotId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<Schedule> GetScheduleSecond(int dateslotId)
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules.Where(c => c.DateslotId == dateslotId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Schedule> GetScheduleThird(int ScheduleId)
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules.Where(c => c.ScheduleId == ScheduleId);
            return await query.FirstOrDefaultAsync();
        }




        //BookingType
        public async Task<BookingType> GetBookingTypetwo(int bookingTypeId)
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes.Where(c => c.BookingTypeId == bookingTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Setup
        public async Task<Setup> GetSetup(int setupId)
        {
            IQueryable<Setup> query = _pedalProDbContext.Setups.Where(c => c.SetupId == setupId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Models.Service> GetService(int serviceid)
        {
            IQueryable<Models.Service> query = _pedalProDbContext.Services.Where(c => c.ServiceId == serviceid);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TrainingSession> GettrainingSession(int sessionId)
        {
            IQueryable<TrainingSession> query = _pedalProDbContext.TrainingSessions.Where(c => c.TrainingSessionId == sessionId);
            return await query.FirstOrDefaultAsync();
        }

        //sa
        public async Task<Booking[]> GetAllBookingAsyncTwo(int clientId)
        {
            IQueryable<Booking> query = _pedalProDbContext.Bookings.Where(b => b.ClientId == clientId && b.BookingStatusId==3);
            return await query.ToArrayAsync();
        }


        public async Task<Payment[]> GetClientPayments(int clientId)
        {
            IQueryable<Payment> query = _pedalProDbContext.Payments.Where(b => b.ClientId == clientId);
            return await query.ToArrayAsync();
        }

        public async Task<Payment[]> GetPayments()
        {
            IQueryable<Payment> query = _pedalProDbContext.Payments;
            return await query.ToArrayAsync();
        }

        public async Task<Payment> GetPayment(int paymentId)
        {
            IQueryable<Payment> query = _pedalProDbContext.Payments.Where(c => c.PaymentId == paymentId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Checkout> GetPaymentCheckout(int paymentId)
        {
            IQueryable<Checkout> query = _pedalProDbContext.Checkouts.Where(c => c.PaymentId == paymentId);
            return await query.FirstOrDefaultAsync();
        }


        //dateslot for booking

        public async Task<DateSlot> GetDateslotFour(int timeslotId)
        {
            IQueryable<DateSlot> query = _pedalProDbContext.DateSlots.Where(c => c.TimeslotId == timeslotId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Date> GetDateFive(int dateId)
        {
            IQueryable<Date> query = _pedalProDbContext.Dates.Where(c => c.DateId == dateId);
            return await query.FirstOrDefaultAsync();
        }

        

        


        //EmpUser
        public async Task<PedalProUser> GetUserFromEmp(string userId)
        {
            IQueryable<PedalProUser> query = _pedalProDbContext.PedalProUsers.Where(c => c.Id == userId);
            return await query.FirstOrDefaultAsync();
        }


        //Workout types
        public async Task<WorkoutType> GetWorkoutType(int workoutTypeId)
        {
            IQueryable<WorkoutType> query = _pedalProDbContext.WorkoutTypes.Where(c => c.WorkoutTypeId == workoutTypeId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<WorkoutType[]> GetAllWorkoutTypes()
        {
            IQueryable<WorkoutType> query = _pedalProDbContext.WorkoutTypes;
            return await query.ToArrayAsync();
        }

        public async Task<Workout> GetWorkout(int workoutId)
        {
            IQueryable<Workout> query = _pedalProDbContext.Workouts.Where(c => c.WorkoutId == workoutId);
            return await query.FirstOrDefaultAsync();
        }



        //Booking reminder
        public async Task<Booking[]> GetBookingsReminder(int clientId)
        {
            IQueryable<Booking> query = _pedalProDbContext.Bookings.Where(c=>c.ClientId==clientId);
            return await query.ToArrayAsync();
        }


        public async Task<Booking> GetEmployeeBooking(int scheduleId)
        {
            IQueryable<Booking> query = _pedalProDbContext.Bookings.Where(c => c.ScheduleId == scheduleId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<Schedule[]> GetEmployeeBookingTwo(int employeeId)
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules.Where(c => c.EmployeeId == employeeId);
            return await query.ToArrayAsync();
        }


        public async Task<Client> GetClientClient(int clientId)
        {
            IQueryable<Client> query = _pedalProDbContext.Clients.Where(c => c.ClientId == clientId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<BookingStatus> GetBookingStatus(int statusId)
        {
            IQueryable<BookingStatus> query = _pedalProDbContext.BookingStatuses.Where(c => c.BookingStatusId == statusId);
            return await query.FirstOrDefaultAsync();
        }



        //BookingType
        public async Task<BookingType[]> GetAllBookingTypeAsync()
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes;
            return await query.ToArrayAsync();
        }

        public async Task<BookingType> GetBookingTypeAsync(int bookingTypeId)
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes.Where(c => c.BookingTypeId == bookingTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Help Category
        public async Task<HelpCategory[]> GetAllHelpCategoriesAsync()
        {
            IQueryable<HelpCategory> query = _pedalProDbContext.Helpcategories;
            return await query.ToArrayAsync();
        }
        public async Task<HelpCategory> GetHelpCategoryAsync(int helpCategoryId)
        {
            IQueryable<HelpCategory> query = _pedalProDbContext.Helpcategories.Where(c => c.HelpCategoryId == helpCategoryId);
            return await query.FirstOrDefaultAsync();
        }

        //Help
        public async Task<Help[]> GetAllHelpAsync()
        {
            IQueryable<Help> query = _pedalProDbContext.Helps;
            return await query.ToArrayAsync();
        }

        public async Task<Help> GetHelpAsync(int helpId)
        {
            IQueryable<Help> query = _pedalProDbContext.Helps.Where(c => c.HelpId == helpId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<Timeslot> GetTimeslotAsync(int timeslotId)
        {
            IQueryable<Timeslot> query = _pedalProDbContext.Timeslots.Where(c => c.TimeslotId == timeslotId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<DatabaseTimer> GetDatabaseTimer(int timerId)
        {
            IQueryable<DatabaseTimer> query = _pedalProDbContext.DatabaseTimers.Where(c => c.DatabaseTimerId == timerId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<DatabaseTimer> GetDatabaseTimerTwo(int timerId)
        {
            IQueryable<DatabaseTimer> query = _pedalProDbContext.DatabaseTimers.Where(c => c.DatabaseTimerId == timerId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<VAT> GetVat(int vatId)
        {
            IQueryable<VAT> query = _pedalProDbContext.Vats.Where(c => c.VatId == vatId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<IndemnityForm> GetLatestDocument()
        {
            var latestDocument = await _pedalProDbContext.IndemnityForms.OrderByDescending(form => form.DateUploaded).FirstOrDefaultAsync();
            return latestDocument;
        }


        public async Task<ClientIndemnityForm> GetUplaodedPdfDocument()
        {
            var latestDocument = await _pedalProDbContext.ClientIndemnityForms.OrderByDescending(form => form.DateUploaded).FirstOrDefaultAsync();
            return latestDocument;
        }



        public async Task<TrainingMaterial[]> GetTrainingMaterialsVid(int moduleId)
        {
            IQueryable<TrainingMaterial> query = _pedalProDbContext.TrainingMaterials.Where(b => b.TrainingModuleId == moduleId);
            return await query.ToArrayAsync();
        }


        public async Task<Feedback[]> GetAllFeedbackAsync()
        {
            IQueryable<Feedback> query = _pedalProDbContext.Feedbacks;
            return await query.ToArrayAsync();
        }
        
        public async Task<Feedback> GetFeedbackAsync(int feedbackId)
        {
            IQueryable<Feedback> query = _pedalProDbContext.Feedbacks.Where(c => c.FeedbackId == feedbackId);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<FeedbackCategory[]> GetAllFeedbackCategoriesAsync()
        {
            IQueryable<FeedbackCategory> query = _pedalProDbContext.FeedbackCategories;
            return await query.ToArrayAsync();
        }

        public async Task<FeedbackCategory> GetFeedbackCategoryAsync(int feedbackcategoryId)
        {
            IQueryable<FeedbackCategory> query = _pedalProDbContext.FeedbackCategories.Where(c => c.FeedbackCategoryId == feedbackcategoryId);
            return await query.FirstOrDefaultAsync();
        }


        //FeedbackCategory



        //Cart
        public async Task<Cart> GetCartOne(int cartId)
        {
            IQueryable<Cart> query = _pedalProDbContext.Carts.Where(c => c.CartId == cartId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Cart> GetCartWithPackages(int cartId)
        {
            IQueryable<Cart> query = _pedalProDbContext.Carts
                .Include(c => c.Packages)
                .Where(c => c.CartId == cartId);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<List<Workout>> GetWorkoutDataBetweenDates(DateTime startDate, DateTime endDate, int clientId)
        {
            return await _pedalProDbContext.Workouts
                .Where(workout => workout.WorkoutDate >= startDate && workout.WorkoutDate <= endDate && workout.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Checkout[]> GetAllCheckouts()
        {
            IQueryable<Checkout> query = _pedalProDbContext.Checkouts;
            return await query.ToArrayAsync();
        }

        public async Task<PackageRevenue> GetPackageRevenue(string name)
        {
            IQueryable<PackageRevenue> query = _pedalProDbContext.PackageRevenues.Where(c => c.PackageName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PackageRevenue[]> GetAllPackageRevenuesAsync()
        {
            IQueryable<PackageRevenue> query = _pedalProDbContext.PackageRevenues;
            return await query.ToArrayAsync();
        }


        public async Task<BookingRevenue> GetBookingRevenue(string name)
        {
            IQueryable<BookingRevenue> query = _pedalProDbContext.BookingRevenues.Where(c => c.BookingType == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Package> GetPackageName(string name)
        {
            IQueryable<Package> query = _pedalProDbContext.Packages.Where(c => c.PackageName == name);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<Schedule[]> GetAllSchedulesAsync()
        {
            IQueryable<Schedule> query = _pedalProDbContext.Schedules;
            return await query.ToArrayAsync();
        }

        public async Task<BookingRevenue[]> GetAllBookingrevenue()
        {
            IQueryable<BookingRevenue> query = _pedalProDbContext.BookingRevenues;
            return await query.ToArrayAsync();
        }

        public async Task<BookingType> GetBookingTypeName(string name)
        {
            IQueryable<BookingType> query = _pedalProDbContext.BookingTypes.Where(c => c.BookingTypeName == name);
            return await query.FirstOrDefaultAsync();
        }

    }
}
