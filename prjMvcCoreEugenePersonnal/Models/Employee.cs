using System;
using System.Collections.Generic;

namespace prjMvcCoreEugenePersonnal.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeePassword { get; set; } = null!;

    public string EmployeeEmail { get; set; } = null!;

    public bool EmployeeCancel { get; set; }
}
