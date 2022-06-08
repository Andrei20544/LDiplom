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
    }
}
