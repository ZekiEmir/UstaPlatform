// UstaPlatform.Domain/Entities/Schedule.cs
using System;
using System.Collections.Generic;
using System.Linq;
// using System.Threading.Tasks; ve using System.Text; gerekmediği için silebilirsiniz.

namespace UstaPlatform.Domain.Entities
{
    // Çizelge (Schedule): Ustaların iş emri takvimi.
    public class Schedule
    {
        private readonly Dictionary<DateOnly, List<WorkOrder>> _dailyOrders = new();

        // Dizinleyici (Indexer): Schedule[DateOnly gün]
        public List<WorkOrder> this[DateOnly gun]
        {
            get
            {
                if (!_dailyOrders.ContainsKey(gun))
                {
                    return new List<WorkOrder>();
                }
                return _dailyOrders[gun];
            }
        }

        public Schedule() { }

        // Bu metot, iş emrini eklerken İLGİLİ TALEP ZAMANINI kullanır.
        public void AddWorkOrder(WorkOrder order)
        {
            // Planlanan günü, Talep'teki İstenenZaman'dan alıyoruz.
            // Bu, Cumartesi gününe ekleme yapılmasını garanti eder.
            DateOnly planlananGun = DateOnly.FromDateTime(order.IlgiliTalep.IstenenZaman.Date);

            if (!_dailyOrders.ContainsKey(planlananGun))
            {
                _dailyOrders[planlananGun] = new List<WorkOrder>();
            }
            _dailyOrders[planlananGun].Add(order);
        }
    }
}