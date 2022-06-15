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
        private static List<Region> _regions;
        private static Region _region;

        private static List<NewEmployee> _employees;
        private static NewEmployee _newEmployee;

        private static List<NewRegion> _newRegions;
        private static NewRegion _newRegion;

        private static List<NewRatePage> _newRates;
        private static NewRatePage _newRate;

        public static List<Region> GetRegions()
        {
            _regions = new List<Region>();

            using (ModelBD model = new ModelBD())
            {

                var subscribers = from s in model.Region
                                  select s;

                if (subscribers != null)
                {

                    foreach (var item in subscribers)
                    {

                        _region = new Region
                        {
                            Name = item.Name,
                            Settlements = item.Settlements
                        };

                        _regions.Add(_region);

                    }

                    return _regions;

                }

            }

            return null;

        }

        public static List<NewRegion> GetNewRegion()
        {

            try
            {
                _newRegions = new List<NewRegion>();

                using (ModelBD model = new ModelBD())
                {

                    var regions = from r in model.Region
                                  select r;

                    var qtyPrivateBuilding = from r in model.Region
                                             join b in model.Building on r.ID equals b.IDRegion
                                             where b.IsPrivate == true
                                             select new
                                             {
                                                 IDRegion = r.ID
                                             };

                    var qtyBuilding = from r in model.Region
                                      join b in model.Building on r.ID equals b.IDRegion
                                      select new
                                      {
                                          IDRegion = r.ID
                                      };

                    var qtyApartment = from r in model.Region
                                       join b in model.Building on r.ID equals b.IDRegion
                                       join a in model.Apartment on b.ID equals a.IDBuilding
                                       where b.IsPrivate == false
                                       select new
                                       {
                                           IDRegion = r.ID
                                       };

                    foreach (var region in regions.ToList())
                    {

                        _newRegion = new NewRegion
                        {
                            IDRegion = region.ID,
                            Region = region.Name,
                            Settlements = region.Settlements,
                            QtyPrivateBuilding = qtyPrivateBuilding.Where(p => p.IDRegion == region.ID).ToList().Count,
                            QtyBuilding = qtyBuilding.Where(p => p.IDRegion == region.ID).ToList().Count,
                            QtyApartment = qtyApartment.Where(p => p.IDRegion == region.ID).ToList().Count
                        };

                        _newRegions.Add(_newRegion);

                    }

                    return _newRegions;

                }
            }
            catch
            {
                return null;
            }

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

        public static List<NewRate> GetNewRate()
        {
            List<NewRate> newRates = new List<NewRate>();

            using (ModelBD model = new ModelBD())
            {

                var rates = from r in model.Rate
                           join reg in model.Region on r.ID equals reg.IDRate
                           join b in model.Building on reg.ID equals b.IDRegion
                           select new
                           {
                               IDBuilding = b.ID,
                               RateWInto = r.PriceWInto,
                               RateWOut = r.PriceWOut
                           };
                
                foreach (var item in rates)
                {

                    NewRate newRate = new NewRate
                    {
                        IDBuilding = item.IDBuilding,
                        RateWInto = item.RateWInto,
                        RateWOut = item.RateWOut
                    };

                    newRates.Add(newRate);

                }

                return newRates;

            }

        }

        public static List<Rate> GetRate()
        {
            using (ModelBD model = new ModelBD())
            {

                var rates = from r in model.Rate
                            select r;

                return rates.ToList();

            }
        }

        public static List<NewRatePage> GetRatePage()
        {
            _newRates = new List<NewRatePage>();

            using (ModelBD model = new ModelBD())
            {

                var rates = from r in model.Rate
                            join reg in model.Region on r.ID equals reg.IDRate
                            select new
                            {
                                ID = r.ID,
                                PeriodStart = r.PeriodStart,
                                PeriodEnd = r.PeriodEnd,
                                PriceWIn = r.PriceWInto,
                                PriceWOut = r.PriceWOut,
                                RegionName = reg.Name
                            };

                foreach (var item in rates)
                {
                    _newRate = new NewRatePage
                    {
                        ID = item.ID,
                        PeriodStart = item.PeriodStart,
                        PeriodEnd = item.PeriodEnd,
                        PriceWIn = item.PriceWIn,
                        PriceWOut = item.PriceWOut,
                        RegionName = item.RegionName
                    };

                    _newRates.Add(_newRate);
                }

                return _newRates;

            }
        }

        public static List<Apartment> GetApartments(int idRegion)
        {
            using (ModelBD model = new ModelBD())
            {

                var apartments = from b in model.Building
                                 join a in model.Apartment on b.ID equals a.IDBuilding
                                 where b.IDRegion == idRegion
                                 select a;

                var list = apartments.ToList();

                return list;

            }
        }

        public static List<Apartment> GetApartments()
        {
            using (ModelBD model = new ModelBD())
            {

                var apartments = from a in model.Apartment
                                 select a;

                var list = apartments.ToList();

                return list;

            }
        }

        public static List<Building> GetBuildings(int idRegion)
        {

            using (ModelBD model = new ModelBD())
            {

                var buildings = from b in model.Building
                                where b.IDRegion == idRegion && b.IsPrivate == true
                                select b;

                var list = buildings.ToList();

                return list;

            }

        }

        public static List<Building> GetBuildings()
        {
            using (ModelBD model = new ModelBD())
            {

                var buildings = from b in model.Building
                                where b.IsPrivate == true
                                select b;

                var list = buildings.ToList();

                return list;

            }
        }
    }
}