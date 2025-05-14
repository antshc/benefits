using Api.SharedKernel.Domain;

namespace Api.Domain.Employees;

// why? There are no requirements that justify modeling Dependent as a separate aggregate root.
// In my view, it is more appropriately treated as part of the Employee aggregate, store in separate database table to avoid bloating of employee table.
public class Dependent : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Relationship Relationship { get; private set; }
    public int EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }

    // EF Core requires a parameterless constructor
    private Dependent()
    {
    }

    public Dependent(string firstName, string lastName, DateTime dateOfBirth, Relationship relationship, int employeeId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Relationship = relationship;
        EmployeeId = employeeId;
    }

    // why? Consider using polymorphic behavior for the relationship, especially if future requirements may introduce logic specific to particular types of dependents.
    public bool IsSpouseOrDomesticPartner()
    {
        return Relationship == Relationship.Spouse || Relationship == Relationship.DomesticPartner;
    }
}