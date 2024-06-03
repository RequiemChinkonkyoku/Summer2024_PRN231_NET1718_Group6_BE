using Microsoft.Extensions.Options;
using Models;
using Repositories.Implement;
using Repositories.Interface;
using Services.Implement;
using Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services and configure a named policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyCorsPolicy",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
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

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDentistService, DentistService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".CCP.Session";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyCorsPolicy");

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
