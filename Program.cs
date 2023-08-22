using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using HoneyRaesAPI.Models;

List<Customer> customers = new List<Customer> 
{
    new Customer()
    {
        Id = 1,
        Name = "Josh Baugh",
        Address = "1201 North Way"
    },
    new Customer()
    {
        Id = 2,
        Name = "Chris Mills",
        Address = "546 Vista Way"
    },
    new Customer()
    {
        Id = 3,
        Name = "Desmond Haynes",
        Address = "142 Pic Blvd"
    },
    new Customer()
    {
        Id = 4,
        Name = "Kenny Clayton",
        Address = "896 Ocean Ave"
    }
};
List<Employee> employees = new List<Employee> 
{
    new Employee()
    {
        Id = 1,
        Name = "Trenton Guffey",
        Specialty = "Computers"
    },
    new Employee()
    {
        Id = 2,
        Name = "Ryan Mathis",
        Specialty = "Mobile"
    }
};
List<ServiceTicket> serviceTickets = new List<ServiceTicket> 
{
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 2,
        Description = "Phone screen broken",
        Emergency = false
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 1,
        EmployeedId = 1,
        Description = "Laptop overheating",
        Emergency = false,
        DateCompleted = new DateTime(2023, 08, 20)
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 4,
        EmployeedId = 2,
        Description = "Phone keeps restarting",
        Emergency = true
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 3,
        EmployeedId = 1,
        Description = "Computer has a deadly virus",
        Emergency = true,
        DateCompleted = new DateTime(2023, 08, 14)
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 2,
        Description = "The internet turned off",
        Emergency = true
    }
};

var builder = WebApplication.CreateBuilder(args);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add services to the container.
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

#region Customer Endpoints
app.MapGet("/customers", () =>
{
    return customers;
});
app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    customer.ServiceTickets = serviceTickets.Where(st => st.CustomerId == id).ToList();
    return Results.Ok(customer);
});
#endregion

#region Employee Endpoints
app.MapGet("/employees", () =>
{
    return employees;
});
app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    employee.ServiceTickets = serviceTickets.Where(st => st.EmployeedId == id).ToList();
    return Results.Ok(employee);
});
#endregion

#region ServiceTicket Endpoints
app.MapGet("/servicetickets", () =>
{
   return serviceTickets; 
});
app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(sv => sv.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTicket.Customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeedId);
    return Results.Ok(serviceTicket);
});
#endregion

app.Run();