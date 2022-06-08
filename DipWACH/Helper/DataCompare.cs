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
        private static Dictionary<string, string> _dict;

        public static string CompareEmployeeData(string login, string password)
        {
            _dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {

                using (ModelBD model = new ModelBD())
                {

                    var employees = from e in model.Employee.AsParallel()
                                    select e;

                    string hashPass = CryptoPass.GetHashPassword(password);
                    Employee employee = employees.Where(p => p.Login.Equals(login) && p.Password.Equals(hashPass)).FirstOrDefault();

                    if (employee != null)
                    {
                        //_dict.Add("fio", employee.FIO);
                        //_dict.Add("flag", "true");

                        return employee.FIO;
                    }
                }

            }

            return null;
        }
    }
}
