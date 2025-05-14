using Api.Application.Dependents.Payload;
using Api.Application.Employees.Payload;
using Api.Domain.Employees;

namespace Api.Data;

public static class DatabaseInitializer
{
    private static readonly List<GetEmployeeDto> EmployeesTestData = new List<GetEmployeeDto>
    {
        new()
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        },
        new()
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3)
                },
                new()
                {
                    Id = 2,
                    FirstName = "Child1",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2020, 6, 23)
                },
                new()
                {
                    Id = 3,
                    FirstName = "Child2",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2021, 5, 18)
                }
            }
        },
        new()
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 4,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2)
                }
            }
        }
    };

    public static void Initialize(BenefitsContext context)
    {
        // Recreate the database each time.
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed if there are no employees.
        if (!context.Employees.Any())
        {
            List<Employee> employees = EmployeesTestData.Select(dto =>
            {
                var employee = new Employee(
                    dto.FirstName,
                    dto.LastName,
                    dto.Salary,
                    dto.DateOfBirth);
                if (dto.Dependents != null)
                {
                    foreach (var dependent in dto.Dependents)
                    {
                        employee.AddDependent(
                            new Dependent(
                                dependent.FirstName,
                                dependent.LastName,
                                dependent.DateOfBirth,
                                dependent.Relationship));
                    }
                }

                return employee;
            }).ToList();

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
    }
}