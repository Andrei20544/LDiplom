using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Helper
{
    public static class DataCompare
    {
        public static string CompareEmployeeData(string login, string password)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {

                using (ModelBD model = new ModelBD())
                {

                    var employees = from e in model.Employee.AsParallel()
                                    select e;

                    var employeeTypes = from et in model.TypeEmployee
                                        select et;

                    string hashPass = CryptoPass.GetHashPassword(password);

                    Employee employee = employees.Where(p => p.Login.Equals(login) && p.Password.Equals(hashPass)).FirstOrDefault();                 

                    if (employee != null)
                    {
                        TypeEmployee typeEmployee = employeeTypes.Where(p => p.ID == employee.IDTypeEmployee).FirstOrDefault();
                        return employee.FIO + "-" + typeEmployee.Type;
                    }
                }

            }

            return null;
        }
    }
}
