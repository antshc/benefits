using Api.SharedKernel.Domain;

namespace Api.Domain.Employees.Rules;

// why? Extracting business rules into separate classes keeps the aggregate focused and easier to maintain.
// It promotes separation of concerns, improves readability, and allows reuse of common validation or policy logic.
internal class SingleSpouseOrDomesticPartnerRule: IBusinessRule
{
    private readonly IEnumerable<Dependent> _dependents;

    public SingleSpouseOrDomesticPartnerRule(IEnumerable<Dependent> dependents)
    {
        _dependents = dependents;
    }

    public void Check()
    {
        // why? Better to throw specific business exception instead of generic one. It makes code more readable and exception handling easier
        if (_dependents.Any(IsSpouseOrDomesticPartner)) throw new ApplicationException("An employee can only have one spouse or domestic partner.");
    }

    private bool IsSpouseOrDomesticPartner(Dependent dependent)
    {
        return dependent.IsSpouseOrDomesticPartner();
    }
}