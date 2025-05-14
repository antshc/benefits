using Api.Domain.Employees.Rules;
using Api.SharedKernel;
using Api.SharedKernel.Domain;

namespace Api.Domain.Employees;

public class Employee : Entity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public decimal Salary { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    private readonly List<Dependent> _dependents;
    public IReadOnlyCollection<Dependent> Dependents => _dependents.AsReadOnly();

    // EF Core requires a parameterless constructor
    private Employee()
    {
        _dependents = new List<Dependent>();
    }

    public Employee(string firstName, string lastName, decimal salary, DateTime dateOfBirth, IEnumerable<Dependent> dependents = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        if (salary < 0)
            throw new ApplicationException("Salary cannot be negative.");

        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        DateOfBirth = dateOfBirth;
        _dependents = dependents?.ToList() ?? new List<Dependent>();
    }

    // why? The dependent may be easy added from the API POST /employees/{id}/dependents
    // Add rules to validate Child dependents are not allowed to be older than the employee.
    // May we use polymorphic behavior?
    public void AddDependent(Dependent dependent)
    {
        dependent.SetEmployeeId(Id);
        
        CheckRule(new SingleSpouseOrDomesticPartnerRule(Dependents, dependent));

        _dependents.Add(dependent);
    }
}