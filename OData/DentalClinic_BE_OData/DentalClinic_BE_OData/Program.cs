using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Models;
using Repositories.Implement;
using Repositories.Interface;
using Services.Implement;
using Services.Interface;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Dentist>("Dentists");
modelBuilder.EntitySet<Treatment>("Treatments");
modelBuilder.EntitySet<Patient>("Patients");
var edmModel = modelBuilder.GetEdmModel();

builder.Services.AddControllers().AddOData(options =>
{
    options.EnableQueryFeatures(); 
    options.AddRouteComponents("odata", edmModel); 
});



builder.Services.AddScoped<IRepositoryBase<Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepositoryBase<AppointmentDetail>, AppointmentDetailRepository>();
builder.Services.AddScoped<IRepositoryBase<Dentist>, DentistRepository>();
builder.Services.AddScoped<IRepositoryBase<Profession>, ProfessionRepository>();
builder.Services.AddScoped<IRepositoryBase<MedicalRecord>, MedicalRecordRepository>();
builder.Services.AddScoped<IRepositoryBase<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepositoryBase<Patient>, PatientRepository>();
builder.Services.AddScoped<IRepositoryBase<Schedule>, ScheduleRepository>();
builder.Services.AddScoped<IRepositoryBase<Treatment>, TreatmentRepository>();
builder.Services.AddScoped<IRepositoryBase<Models.Transaction>, TransactionRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDentistService, DentistService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
