namespace IntroToEntityFramework
{
    using Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            var ctx = new SoftUniContext();
            using (ctx)
            {
                var employees = ctx.Employees;

                //03. Employees full information
                /*foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FirstName} " +
                                        $"{employee.LastName} " + 
                                        $"{employee.MiddleName} " + 
                                        $"{employee.JobTitle} " +
                                        $"{employee.Salary}");
                }*/

                //////////////////////////////////////////////////////

                //04. Employees with Salary Over 50 000
                //var employeesWithSalaryOver50000 = employees
                //                                    .Where(e => e.Salary > 50000)
                //                                    .Select(e => e.FirstName);

                //foreach (var employee in employeesWithSalaryOver50000)
                //{
                //    Console.WriteLine(employee);
                //}

                //////////////////////////////////////////////////////

                //05. Employees from Seattle
                //var employeesFromSeattle = employees
                //                            .Where(e => e.Department.Name == "Research and Development")
                //                            .OrderBy(e => e.Salary)
                //                            .ThenByDescending(e => e.FirstName);

                //foreach (var emp in employeesFromSeattle)
                //{
                //    Console.WriteLine($"{emp.FirstName} {emp.LastName} " +
                //                      $"from {emp.Department.Name} - ${emp.Salary:F2}");
                //}

                //////////////////////////////////////////////////////

                //06. Adding a New Address and Updating Employee
                //var address = new Address();
                //address.AddressText = "Vitoshka 15";
                //address.TownID = 4;

                //var employeeNakov = ctx.Employees.FirstOrDefault(e => e.LastName == "Nakov");
                //employeeNakov.Address = address;

                //ctx.SaveChanges();

                //var employeesAddresses = ctx.Addresses
                //                            .OrderByDescending(e => e.AddressID)
                //                            .Take(10)
                //                            .Select(e => e.AddressText);

                //foreach (var empAddress in employeesAddresses)
                //{
                //    Console.WriteLine(empAddress);
                //}

                ///////////////////////////////////////////////////////////

                //07. Delete Project by Id
                //var project = ctx.Projects.Find(2);

                //foreach (var emp in project.Employees)
                //{
                //    var currentEmp = ctx.Employees.FirstOrDefault(e => e.EmployeeID == emp.EmployeeID);
                //    currentEmp.Projects.Remove(project);
                //}

                //ctx.Projects.Remove(project);

                //ctx.SaveChanges();

                //foreach (var name in ctx.Projects.Take(10).Select(p => p.Name))
                //{
                //    Console.WriteLine(name);
                //}

                /////////////////////////////////////////////////////////

                //08. Find employees in period
                //var employeesInPeriod = ctx.Projects
                //                            .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                //                            .SelectMany(p => p.Employees)
                //                            .Distinct()
                //                            .Take(30);

                //foreach (var emp in employeesInPeriod)
                //{
                //    Console.WriteLine($"{emp.FirstName} {emp.LastName} {emp.Manager.FirstName}");
                //    foreach (var proj in emp.Projects)
                //    {
                //        Console.WriteLine($"--{proj.Name} {proj.StartDate} {proj.EndDate}");
                //    }
                //}

                ///////////////////////////////////////////////////////////

                //09. Addresses by town name 
                var addresses = ctx.Addresses
                                    .OrderByDescending(a => a.Employees.Count())
                                    .ThenBy(a => a.Town.Name)
                                    .Take(10);
                foreach (var addr in addresses)
                {
                    Console.WriteLine($"{addr.AddressText}, {addr.Town.Name} - {addr.Employees.Count()} employees");
                }
            }
        }
    }
}
