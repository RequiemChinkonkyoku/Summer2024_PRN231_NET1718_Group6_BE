using Models;
using Repositories.Implement;
using Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryBase<Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepositoryBase<AppointmentDetail>, AppointmentDetailRepository>();
builder.Services.AddScoped<IRepositoryBase<Dentist>, DentistRepository>();
builder.Services.AddScoped<IRepositoryBase<DentistMedicalService>, DentistServiceRepository>();
builder.Services.AddScoped<IRepositoryBase<MedicalRecord>, MedicalRecordRepository>();
builder.Services.AddScoped<IRepositoryBase<Patient>, PatientRepository>();
builder.Services.AddScoped<IRepositoryBase<Schedule>, ScheduleRepository>();
builder.Services.AddScoped<IRepositoryBase<MedicalService>, MedicalServiceRepository>();
builder.Services.AddScoped<IRepositoryBase<Models.Transaction>, TransactionRepository>();

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
