using System;
using Api.Domain.Employees;
using Xunit;

namespace ApiTests.UnitTests.Emloyees;

public class EmployeeUnitTests
{
    [Fact]
    public void WhenAddMoreThenOneSpouse_Throws()
    {
        var emp = new Employee("LeBron", "James", 78000m, new DateTime(1984, 12, 30));
        Assert.Throws<ApplicationException>(() =>
        {
            emp.AddDependent(new Dependent("Spouse", "LeBron", new DateTime(2015, 5, 5), Relationship.Spouse));
            emp.AddDependent(new Dependent("Spouse2", "LeBron", new DateTime(2015, 5, 5), Relationship.Spouse));
        });
    }

    [Fact]
    public void WhenAddMSpouseAndPartner_Throws()
    {
        var emp = new Employee("LeBron", "James", 78000m, new DateTime(1984, 12, 30));
        Assert.Throws<ApplicationException>(() =>
        {
            emp.AddDependent(new Dependent("Spouse", "LeBron", new DateTime(2015, 5, 5), Relationship.Spouse));
            emp.AddDependent(new Dependent("Spouse2", "LeBron", new DateTime(2015, 5, 5), Relationship.DomesticPartner));
        });
    }
}