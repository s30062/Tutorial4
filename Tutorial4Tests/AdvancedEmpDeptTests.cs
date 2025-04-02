using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tutorial4Tests;

public class AdvancedEmpDeptTests
{
    [Fact]
    public void ShouldReturnMaxSalary()
    {
        var emps = Database.GetEmps();
        decimal? maxSalary = emps.Max(e => e.Sal);
        Assert.Equal(5000, maxSalary);
    }

    [Fact]
    public void ShouldReturnMinSalaryInDept30()
    {
        var emps = Database.GetEmps();
        decimal? minSalary = emps.Where(e => e.DeptNo == 30).Min(e => e.Sal);
        Assert.Equal(1250, minSalary);
    }

    [Fact]
    public void ShouldReturnFirstTwoHiredEmployees()
    {
        var emps = Database.GetEmps();
        var firstTwo = emps.OrderBy(e => e.HireDate).Take(2).ToList();
        Assert.Equal(2, firstTwo.Count);
        Assert.True(firstTwo[0].HireDate <= firstTwo[1].HireDate);
    }

    [Fact]
    public void ShouldReturnDistinctJobTitles()
    {
        var emps = Database.GetEmps();
        var jobs = emps.Select(e => e.Job).Distinct().ToList();
        Assert.Contains("PRESIDENT", jobs);
        Assert.Contains("SALESMAN", jobs);
    }

    [Fact]
    public void ShouldReturnEmployeesWithManagers()
    {
        var emps = Database.GetEmps();
        var withMgr = emps.Where(e => e.Mgr.HasValue).ToList();
        Assert.All(withMgr, e => Assert.NotNull(e.Mgr));
    }

    [Fact]
    public void AllEmployeesShouldEarnMoreThan500()
    {
        var emps = Database.GetEmps();
        var result = emps.All(e => e.Sal > 500);
        Assert.True(result);
    }

    [Fact]
    public void ShouldFindAnyWithCommissionOver400()
    {
        var emps = Database.GetEmps();
        var result = emps.Any(e => e.Comm > 400);
        Assert.True(result);
    }

    [Fact]
    public void ShouldReturnEmployeeManagerPairs()
    {
        var emps = Database.GetEmps();
        var result = emps.Join(emps, e1 => e1.Mgr, e2 => e2.EmpNo, (e1, e2) => new { Employee = e1.EName, Manager = e2.EName }).ToList();
        Assert.Contains(result, r => r.Employee == "SMITH" && r.Manager == "FORD");
    }
}

public class EmpDeptSalgradeTests
{
    [Fact]
    public void ShouldReturnAllSalesmen()
    {
        var emps = Database.GetEmps();
        var result = emps.Where(e => e.Job == "SALESMAN").ToList();
        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.Equal("SALESMAN", e.Job));
    }

    [Fact]
    public void ShouldReturnDept30EmpsOrderedBySalaryDesc()
    {
        var emps = Database.GetEmps();
        var result = emps.Where(e => e.DeptNo == 30).OrderByDescending(e => e.Sal).ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result[0].Sal >= result[1].Sal);
    }

    [Fact]
    public void ShouldReturnEmployeesFromChicago()
    {
        var emps = Database.GetEmps();
        var depts = Database.GetDepts();
        var result = emps.Where(e => depts.Any(d => d.Loc == "CHICAGO" && d.DeptNo == e.DeptNo)).ToList();
        Assert.All(result, e => Assert.Equal(30, e.DeptNo));
    }
}
