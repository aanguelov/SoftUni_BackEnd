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
                //var employees = ctx.Employees;

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
                var address = new Address();
                address.AddressText = "Vitoshka 15";
                address.TownID = 4;

                var employeeNakov = ctx.Employees.FirstOrDefault(e => e.LastName == "Nakov");
                employeeNakov.Address = address;

                ctx.SaveChanges();

                var employeesAddresses = ctx.Employees
                                            .OrderByDescending(e => e.AddressID)
                                            .Take(10)
                                            .Select(e => e.Address.AddressText);

                foreach (var empAddress in employeesAddresses)
                {
                    Console.WriteLine(empAddress);
                }

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
                //var addresses = ctx.Addresses
                //                    .OrderByDescending(a => a.Employees.Count())
                //                    .ThenBy(a => a.Town.Name)
                //                    .Take(10);
                //foreach (var addr in addresses)
                //{
                //    Console.WriteLine($"{addr.AddressText}, {addr.Town.Name} - {addr.Employees.Count()} employees");
                //}

                //////////////////////////////////////////////////////////

                //10. Employee with id 147 sorted by project names 
                //var emp147 = ctx.Employees.Find(147);

                //Console.WriteLine($"{emp147.FirstName} {emp147.LastName} {emp147.JobTitle}");
                //var projects = emp147.Projects.OrderBy(p => p.Name);

                //foreach (var project in projects)
                //{
                //    Console.WriteLine($"{project.Name}");
                //}

                ////////////////////////////////////////////////////////////////////////////

                //11. Departments with more than 5 employees
                //var depWithMoreThan5Emps = ctx.Departments
                //                                .Where(d => d.Employees.Count > 5)
                //                                .OrderBy(d => d.Employees.Count);
                //foreach (var department in depWithMoreThan5Emps)
                //{
                //    Console.WriteLine($"{department.Name} {department.Manager.FirstName}");
                //    foreach (var emp in department.Employees)
                //    {
                //        Console.WriteLine($"{emp.FirstName} {emp.LastName} {emp.JobTitle}");
                //    }
                //}

                ////////////////////////////////////////////////////////////////////////

                //15. Find Latest 10 Projects
                //var latestStarted10Projects = ctx.Projects
                //                                    .OrderByDescending(p => p.StartDate)
                //                                    .Take(10)
                //                                    .OrderBy(p => p.Name);
                //foreach (var project in latestStarted10Projects)
                //{
                //    Console.WriteLine($"{project.Name} {project.Description} {project.StartDate} {project.EndDate}");
                //}

                /////////////////////////////////////////////////////////////////////

                //16. Increase Salaries
                //var employees = ctx.Employees
                //                    .Where(e => e.Department.Name == "Engineering" ||
                //                                e.Department.Name == "Tool Design" ||
                //                                e.Department.Name == "Marketing" ||
                //                                e.Department.Name == "Information Services");
                //foreach (var employee in employees)
                //{
                //    employee.Salary += employee.Salary * (decimal)0.12;
                //    Console.WriteLine($"{employee.FirstName} {employee.LastName} (${employee.Salary})");
                //}
                //ctx.SaveChanges();

                //18. Find Employees by First Name starting with ‘SA’
                //var employeesStartingWithSA = ctx.Employees.Where(e => e.FirstName.Substring(0, 2) == "Sa");

                //foreach (var employee in employeesStartingWithSA)
                //{
                //    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary})");
                //}
            }
        }
    }
}
