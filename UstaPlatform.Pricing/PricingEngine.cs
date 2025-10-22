// UstaPlatform.Pricing/PricingEngine.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing
{
    // Hatanın çözümü için sınıfı public yapıyoruz (CS0122)
    public class PricingEngine
    {
        private readonly List<IPricingRule> _rules = new();

        // DLL'leri yükler ve IPricingRule'ları bulur (Plug-in Mimarisi)
        public void LoadRules(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
            {
                Console.WriteLine($"Kural klasörü bulunamadı: {pluginDirectory}");
                return;
            }

            foreach (var dllPath in Directory.GetFiles(pluginDirectory, "*.dll"))
            {
                try
                {
                    // Assembly'yi yükle
                    Assembly assembly = Assembly.LoadFrom(dllPath);

                    // IPricingRule arayüzünü uygulayan tipleri bul
                    var ruleTypes = assembly.GetTypes()
                        .Where(t => typeof(IPricingRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in ruleTypes)
                    {
                        // Kuralın bir örneğini oluştur ve listeye ekle (DIP'ye uygun)
                        var rule = Activator.CreateInstance(type) as IPricingRule;
                        if (rule != null)
                        {
                            _rules.Add(rule);
                            Console.WriteLine($"[Kural Yüklendi]: {rule.RuleName} ({Path.GetFileName(dllPath)})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {Path.GetFileName(dllPath)} yüklenemedi. Detay: {ex.Message}");
                }
            }
        }

        // Fiyat Hesaplama Kompozisyonu: Kuralların ardışık uygulanması.
        public decimal CalculatePrice(WorkOrder order, decimal basePrice)
        {
            decimal currentPrice = basePrice;

            Console.WriteLine($"Başlangıç Ücreti: {currentPrice:C}");

            // Yüklenen tüm kuralları ardışık olarak uygula.
            foreach (var rule in _rules)
            {
                decimal newPrice = rule.Apply(currentPrice, order);
                if (newPrice != currentPrice)
                {
                    Console.WriteLine($"  -> Kural Uygulandı: {rule.RuleName}. ({currentPrice:C} -> {newPrice:C})");
                    currentPrice = newPrice;
                }
            }

            return currentPrice;
        }
    }
}