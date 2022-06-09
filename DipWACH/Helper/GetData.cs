using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Helper
{
    public static class GetData
    {
        private static List<NewSubscriber> _subscribers;
        private static NewSubscriber _newSubscriber;

        private static List<NewEmployee> _employees;
        private static NewEmployee _newEmployee;

        public static List<NewSubscriber> GetSubscribers()
        {
            _subscribers = new List<NewSubscriber>();

            using (ModelBD model = new ModelBD())
            {

                var subscribers = from s in model.Subscriber
                                  select s;

                if (subscribers != null)
                {

                    foreach (var item in subscribers)
                    {

                        _newSubscriber = new NewSubscriber
                        {
                            Name = item.FIO.Split(' ')[1].Split(' ')[0],
                            Sername = item.FIO.Split(' ')[0],
                            Serial = item.PassportData.Split(' ')[0],
                            Number = item.PassportData.Split(' ')[1],
                            INN = item.INN
                        };

                        _subscribers.Add(_newSubscriber);

                    }

                    return _subscribers;

                }

            }

            return null;

        }

        public static List<string> GetEmployeeTypes()
        {

            List<string> types = new List<string>();

            using (ModelBD model = new ModelBD())
            {

                var emplTypes = from et in model.TypeEmployee
                                select et;

                if (emplTypes != null)
                {

                    //types = new string[emplTypes.ToList().Count];
                    var count = 0;

                    foreach (var item in emplTypes)
                    {
                        types.Add(item.ID + "-" + item.Type);
                        count++;
                    }

                    return types;

                }

                return null;

            }

        }

        public static List<NewEmployee> GetEmployees()
        {

            _employees = new List<NewEmployee>();

            using (ModelBD model = new ModelBD())
            {

                var employees = from e in model.Employee
                                join t in model.TypeEmployee on e.IDTypeEmployee equals t.ID
                                select new
                                {
                                    ID = e.ID,
                                    FIO = e.FIO,
                                    Login = e.Login,
                                    Password = e.Password,
                                    Type = t.Type
                                };

                if (employees != null)
                {

                    foreach (var item in employees)
                    {

                        _newEmployee = new NewEmployee
                        {
                            ID = item.ID,
                            Name = item.FIO.Split(' ')[1].Split(' ')[0],
                            Sername = item.FIO.Split(' ')[0],
                            MiddleName = item.FIO.Split(' ')[2],
                            Login = item.Login,
                            Password = item.Password,
                            Type = item.Type
                        };

                        _employees.Add(_newEmployee);

                    }

                    return _employees;

                }

            }

            return null;

        }
    }
}
