using DipWACH.Helper;
using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DipXTest
{
    public class UnitTest
    {
        private double _priceWIn = 15;
        private double _riceWOut = 12;

        [Fact]
        public void CalculateApartmentSummTest()
        {
            var calc = new Calculater(true);

            var apartments = new List<Apartment>() {
                new Apartment { ID = 1, IDBuilding = 1, IsWMeter = true, Number = 10, Penalties = 0, PeriodEnd = DateTime.Now, PeriodStart = DateTime.Now, QtyPeople = 3, WMeter = 16 },
                new Apartment { ID = 2, IDBuilding = 1, IsWMeter = false, Number = 10, Penalties = 0, PeriodEnd = DateTime.Now, PeriodStart = DateTime.Now, QtyPeople = 3, WMeter = 16 }
            };

            var firstAp = apartments.Where(p => p.ID == 1).FirstOrDefault();
            var secondAp = apartments.Where(p => p.ID == 2).FirstOrDefault();

            double MySumm = (double)((firstAp.WMeter * _priceWIn) + (firstAp.WMeter * _riceWOut) +
                         (secondAp.QtyPeople * _priceWIn * 8.8) + (secondAp.QtyPeople * _riceWOut * 8.8));

            var Summ = calc.CalculateApartmentSummT(apartments, _priceWIn, _riceWOut);

            Assert.Equal(Math.Round(MySumm, 2), Summ);

        }

        [Fact]
        public void CalculateBuildingSummTest()
        {
            var calc = new Calculater(true);

            var buildings = new List<Building>() {
                new Building { ID = 1, QtyFloor = 2, QtyApartment = 0, QtyPeople = 3, Address = "Some address", IDRegion = 1, IsPrivate = true, Penalties = 0 },
                new Building { ID = 2, QtyFloor = 2, QtyApartment = 0, QtyPeople = 4, Address = "Some address", IDRegion = 1, IsPrivate = true, Penalties = 0 }
            };

            var firstAp = buildings.Where(p => p.ID == 1).FirstOrDefault();
            var secondAp = buildings.Where(p => p.ID == 2).FirstOrDefault();

            double MySumm = (double)((firstAp.QtyPeople * _priceWIn * 8.8) + (firstAp.QtyPeople * _riceWOut * 8.8) +
                         (secondAp.QtyPeople * _priceWIn * 8.8) + (secondAp.QtyPeople * _riceWOut * 8.8));

            var Summ = calc.CalculateBuildingSummT(buildings, _priceWIn, _riceWOut);

            Assert.Equal(Math.Round(MySumm, 2), Summ);

        }

        [Fact]
        public void AutoCalculateSummTest()
        {
            var calc = new Calculater(true);

            var buildings = new List<Building>() {
                new Building { ID = 1, QtyFloor = 2, QtyApartment = 0, QtyPeople = 3, Address = "Some address", IDRegion = 1, IsPrivate = true, Penalties = 0 },
                new Building { ID = 2, QtyFloor = 2, QtyApartment = 0, QtyPeople = 4, Address = "Some address", IDRegion = 1, IsPrivate = true, Penalties = 0 }
            };

            var apartments = new List<Apartment>() {
                new Apartment { ID = 1, IDBuilding = 1, IsWMeter = true, Number = 10, Penalties = 0, PeriodEnd = DateTime.Now, PeriodStart = DateTime.Now, QtyPeople = 3, WMeter = 16 },
                new Apartment { ID = 2, IDBuilding = 1, IsWMeter = false, Number = 10, Penalties = 0, PeriodEnd = DateTime.Now, PeriodStart = DateTime.Now, QtyPeople = 3, WMeter = 16 }
            };

            var firstBu = buildings.Where(p => p.ID == 1).FirstOrDefault();
            var secondBu = buildings.Where(p => p.ID == 2).FirstOrDefault();

            var firstAp = apartments.Where(p => p.ID == 1).FirstOrDefault();
            var secondAp = apartments.Where(p => p.ID == 2).FirstOrDefault();

            var MySummAp = (firstAp.WMeter * _priceWIn) + (firstAp.WMeter * _riceWOut) +
                         (secondAp.QtyPeople * _priceWIn * 8.8) + (secondAp.QtyPeople * _riceWOut * 8.8);

            var MySummBu = (double)((firstBu.QtyPeople * _priceWIn * 8.8) + (firstBu.QtyPeople * _riceWOut * 8.8) +
                         (secondBu.QtyPeople * _priceWIn * 8.8) + (secondBu.QtyPeople * _riceWOut * 8.8));

            var Summ = calc.AutoCalculateT(apartments, buildings, _priceWIn, _riceWOut);

            Assert.Equal(Math.Round((double)(MySummAp + MySummBu), 2), Summ);

        }
    }
}
