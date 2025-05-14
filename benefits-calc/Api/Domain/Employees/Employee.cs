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
    public Dependent? SpouseOrDomesticPartner => Dependents.SingleOrDefault(d => d.IsSpouseOrDomesticPartner());

    // EF Core requires a parameterless constructor
    private Employee()
    {
        _dependents = new List<Dependent>();
    }

    public Employee(string firstName, string lastName, decimal salary, DateTime dateOfBirth, IEnumerable<Dependent> dependents = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        DateOfBirth = dateOfBirth;
        _dependents = dependents?.ToList() ?? new List<Dependent>();
    }

    public IReadOnlyCollection<Dependent> GetDependents()
    {
        return Dependents;
    }

    public void AddChild(string firstName, string lastName, DateTime dateOfBirth)
    {
        AddDependent(firstName, lastName, dateOfBirth, Relationship.Child);
    }

    public void AddSpouse(string firstName, string lastName, DateTime dateOfBirth)
    {
        CheckRule(new SingleSpouseOrDomesticPartnerRule(Dependents));
        AddDependent(firstName, lastName, dateOfBirth, Relationship.Spouse);
    }

    public void AddDomesticPartner(string firstName, string lastName, DateTime dateOfBirth)
    {
        CheckRule(new SingleSpouseOrDomesticPartnerRule(Dependents));
        AddDependent(firstName, lastName, dateOfBirth, Relationship.DomesticPartner);
    }

    public void AddDependent(string firstName, string lastName, DateTime dateOfBirth, Relationship relationship)
    {
        _dependents.Add(new Dependent(firstName, lastName, dateOfBirth, relationship, Id));
    }
}