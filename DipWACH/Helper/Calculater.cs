using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Helper
{
    public class Calculater
    {
        private List<NewRate> _newRates;
        private List<string> _summs;
        private double? _priceWIn;
        private double? _priceWOut;

        public Calculater()
        {
            _newRates = GetData.GetNewRate();
        }

        public Calculater(bool test) { }

        private double BSSumm(double? rate, double norm, int? qtyPeople)
        {
            return (double)(rate * norm * qtyPeople);
        }

        private double Summ(double? rate, double? wMeter)
        {
            return (double)(rate * wMeter);
        }

        private void GetRatePrice(int id)
        {
            var priceWIn = _newRates.Where(p => p.IDBuilding == id).FirstOrDefault();
            var priceWOut = _newRates.Where(p => p.IDBuilding == id).FirstOrDefault();

            if (priceWIn.RateWInto != null)
                _priceWIn = priceWIn.RateWInto;
            else
                _priceWIn = 0;

            if (priceWOut.RateWOut != null)
                _priceWOut = priceWOut.RateWOut;
            else
                _priceWOut = 0;

        }

        public double CalculateApartmentSumm(List<Apartment> apartments)
        {
            double summ = 0;

            if (apartments != null)
            {
                foreach (var item in apartments)
                {
                    if (item.IsWMeter == true)
                    {
                        GetRatePrice(item.IDBuilding);

                        summ += Summ(_priceWIn, item.WMeter) + Summ(_priceWOut, item.WMeter);
                    }
                    else
                    {
                        GetRatePrice(item.IDBuilding);

                        summ += BSSumm(_priceWIn, 8.8, item.QtyPeople) + BSSumm(_priceWOut, 8.8, item.QtyPeople);
                    }

                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

        public double CalculateApartmentSummT(List<Apartment> apartments, double priceWIn, double priceWOut)
        {
            double summ = 0;

            if (apartments != null)
            {
                foreach (var item in apartments)
                {
                    if (item.IsWMeter == true)
                    {
                        summ += Summ(priceWIn, item.WMeter) + Summ(priceWOut, item.WMeter);
                    }
                    else
                    {
                        summ += BSSumm(priceWIn, 8.8, item.QtyPeople) + BSSumm(priceWOut, 8.8, item.QtyPeople);
                    }

                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

        public double CalculateBuildingSumm(List<Building> buildings)
        {
            double summ = 0;

            if (buildings != null)
            {
                foreach (var item in buildings)
                {
                    GetRatePrice(item.ID);

                    summ += BSSumm(_priceWIn, 8.8, item.QtyPeople) + BSSumm(_priceWOut, 8.8, item.QtyPeople);

                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

        public double CalculateBuildingSummT(List<Building> buildings, double priceWIn, double priceWOut)
        {
            double summ = 0;

            if (buildings != null)
            {
                foreach (var item in buildings)
                {
                    summ += BSSumm(priceWIn, 8.8, item.QtyPeople) + BSSumm(priceWOut, 8.8, item.QtyPeople);
                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

        public double AutoCalculate()
        {
            double summ = 0;

            var apartments = GetData.GetApartments();
            var buildings = GetData.GetBuildings();

            if (apartments != null && buildings != null)
            {
                var apSumm = CalculateApartmentSumm(apartments);
                var buSumm = CalculateBuildingSumm(buildings);
                
                summ = apSumm + buSumm;
            }

            return summ;

        }

        public List<string> AutoCalculateForPDF()
        {
            double summ = 0;
            _summs = new List<string>();

            var regions = GetData.GetRegions();

            foreach (var item in regions)
            {
                var apartments = GetData.GetApartments(item.ID);
                var buildings = GetData.GetBuildings(item.ID);

                var apSumm = CalculateApartmentSumm(apartments);
                var buSumm = CalculateBuildingSumm(buildings);

                summ = apSumm + buSumm;

                _summs.Add($"{item.Name}-{summ}");
            }

            return _summs;

        }

        public double AutoCalculateT(List<Apartment> apartments, List<Building> buildings, double priceWIn, double priceWOut)
        {
            double summ = 0;

            if (apartments != null && buildings != null)
            {
                var apSumm = CalculateApartmentSummT(apartments, priceWIn, priceWOut);
                var buSumm = CalculateBuildingSummT(buildings, priceWIn, priceWOut);

                summ = apSumm + buSumm;
            }

            return summ;

        }

        public double CalculateSpecialSummWIn(NewApartment newApartment)
        {
            double summ = 0;

            if (newApartment != null)
            {
                if (newApartment.RatePriceWIn != null)
                {
                    if (newApartment.IsWMeter == true)
                    {
                        summ = Summ(newApartment.RatePriceWIn, newApartment.WMeter);
                    }
                    else
                    {
                        summ = BSSumm(newApartment.RatePriceWIn, 8.8, newApartment.QtyPeople);
                    }
                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

        public double CalculateSpecialSummWOut(NewApartment newApartment)
        {
            double summ = 0;

            if (newApartment != null)
            {
                if (newApartment.RatePriceWOut != null)
                {
                    if (newApartment.IsWMeter == true)
                    {
                        summ = Summ(newApartment.RatePriceWOut, newApartment.WMeter);
                    }
                    else
                    {
                        summ = BSSumm(newApartment.RatePriceWOut, 8.8, newApartment.QtyPeople);
                    }
                }

                return Math.Round(summ, 2);
            }

            return 0;
        }

    }
}