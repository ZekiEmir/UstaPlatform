// UstaPlatform.Rules.Weekend/WeekendSurchargeRule.cs
using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using System; // DayOfWeek kullanmak için gereklidir

namespace UstaPlatform.Rules.Weekend
{
    // Bu sınıf, IPricingRule arayüzünü uygulayarak Plug-in mimarisine dahil olur.
    public class WeekendSurchargeRule : IPricingRule
    {
        public string RuleName => "Haftasonu Ek Ücreti Kuralı";

        // Fiyatlandırmayı uygular.
        public decimal Apply(decimal basePrice, WorkOrder order)
        {
            // Haftasonu kuralı, iş emrinin TALEP EDİLEN ZAMANI kullanmalıdır.

            // İş Emri'nin planlanan zamanı Cumartesi veya Pazar mı kontrol edilir.
            if (order.IlgiliTalep.IstenenZaman.DayOfWeek == DayOfWeek.Saturday ||
               order.IlgiliTalep.IstenenZaman.DayOfWeek == DayOfWeek.Sunday)
            {
                // %20 ek ücret uygula
                return basePrice * 1.20m;
            }
            return basePrice; // Haftaiçi ise fiyatı değiştirme.
        }
    }
}