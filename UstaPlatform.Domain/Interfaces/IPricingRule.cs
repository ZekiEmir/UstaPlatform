using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaPlatform.Domain.Entities;

// UstaPlatform.Domain/Interfaces/IPricingRule.cs
namespace UstaPlatform.Domain.Interfaces
{
    // Fiyatı dinamik olarak değiştiren kuralı temsil eder.
    public interface IPricingRule
    {
        // Kuralın adını döndürür.
        string RuleName { get; }

        // Fiyatlandırmayı uygular ve güncel fiyatı döndürür.
        decimal Apply(decimal basePrice, WorkOrder order);
    }
}