using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Helper
{
    public class FillSubData
    {
        private string[] _names = new string[7] { "Андрей", "Даниил", "Дмитрий", "Сергей", "Антон", "Олег", "Владимир" };
        private string[] _sernames = new string[7] { "Иванов", "Сидоров", "Смирнов", "Попов", "Петров", "Соколов", "Михайлов" };
        private string[] _middlenames = new string[7] { "Александрович", "Владимирович", "Никитич", "Андреевич", "Сергеевич", "Дмитриевич", "Игоревич" };

        private string[] _fio = new string[20];
        private string[] _passdata = new string[20];

        private Random _random;

        private void GetFIO()
        {
            _random = new Random();

            var FIO = "";

            for (int i = 0; i < _fio.Length; i++)
            {

                FIO = _sernames[_random.Next(7)] + " " + _names[_random.Next(7)] + " " + _middlenames[_random.Next(7)];
                _fio[i] = FIO;

            }

        }

        private void GetPassData()
        {
            _random = new Random();

            var Serial = 1111;
            var Number = 111111;
            var PassData = "";

            for (int i = 0; i < _passdata.Length; i++)
            {

                Serial = _random.Next(9999);
                Number = _random.Next(999999);

                PassData = Serial + " " + Number;

                _passdata[i] = PassData;

            }

        }

        public void Fill(int amount)
        {
            GetFIO();
            GetPassData();

            //using (ModelBD model = new ModelBD())
            //{

            //    Subscriber subscriber;
            //    _random = new Random();

            //    if (_fio != null && _passdata != null)
            //    {

            //        for (int i = 0; i < amount; i++)
            //        {
            //            subscriber = new Subscriber
            //            {
            //                FIO = _fio[_random.Next(20)],
            //                IDProperty = 1,
            //                PassportData = _passdata[_random.Next(20)],
            //                INN = _random.Next(11111, 99999).ToString() + _random.Next(111111, 999999).ToString()
            //            };

            //            model.Subscriber.Add(subscriber);
            //        }

            //    }

            //    model.SaveChanges();

            //}
        }

    }
}
