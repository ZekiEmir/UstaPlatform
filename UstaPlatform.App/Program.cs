// UstaPlatform.App/Program.cs
using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Pricing;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UstaPlatform.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const string PluginKlasoru = "Plugins";

            Console.WriteLine("==================================================");
            Console.WriteLine("=== UstaPlatform Uygulaması Başlatılıyor (NYP Projesi) ===");
            Console.WriteLine("==================================================");

            // 1. Fiyatlama Motorunu Hazırla ve Kuralları Yükle
            var pricingEngine = new PricingEngine();

            // Plug-in (Eklenti) Mimarisi: Belirtilen klasördeki DLL'leri dinamik olarak yükler (OCP)
            Console.WriteLine("\n[1] Fiyatlama Kuralları Yükleniyor...");
            pricingEngine.LoadRules(PluginKlasoru);

            // 2. Demo Verisi Oluştur (Nesne ve Koleksiyon Başlatıcıları Kullanımı)

            // Koleksiyon Başlatıcıları ile Usta listesi oluşturma
            var ustaList = new List<Usta>
        {
            // Nesne Başlatıcıları kullanımı
            new Usta { Ad = "Ahmet Usta", UzmanlikAlani = "Tesisatçı", Puan = 4.8m, Seviye = "Deneyimli" },
            new Usta { Ad = "Burak Usta", UzmanlikAlani = "Elektrikçi", Puan = 4.5m, Seviye = "Normal" }
        };
            var vatandas = new Vatandas { Ad = "Fatma Hanım", Adres = "Arcadia, Merkez Mah." };

            DateTime cumartesi = GetNextDay(DayOfWeek.Saturday).AddHours(14);

            var talep = new Talep
            {
                Aciklama = "Acil su sızıntısı tamiri gerekiyor.",
                LokasyonX = 15,
                LokasyonY = 30,
                IstenenZaman = cumartesi
            };

            Console.WriteLine("\n[2] Yeni İş Akışı Başlatıldı (Tarih: {0})", talep.IstenenZaman.ToShortDateString());
            Console.WriteLine($"Talep Eden: {vatandas.Ad} | Talep: {talep.Aciklama}");

            var atananUsta = ustaList.FirstOrDefault(u => u.UzmanlikAlani == "Tesisatçı");

            if (atananUsta == null)
            {
                Console.WriteLine("Hata: Uygun usta bulunamadı.");
                return;
            }

            var workOrder = new WorkOrder
            {
                TalepEden = vatandas,
                AtananUsta = atananUsta,
                IlgiliTalep = talep,
                TahminiFiyat = 100.0m
            };
            Console.WriteLine($"Atanan Usta: {atananUsta.Ad}");

            workOrder.RotaDuraklari = new Route
        {
            { 10, 20 },
            { talep.LokasyonX, talep.LokasyonY }
        };
            Console.WriteLine($"Rota Durakları Oluşturuldu (2 durak): (10, 20) -> ({talep.LokasyonX}, {talep.LokasyonY})");


            Console.WriteLine("\n[3] Dinamik Fiyat Hesaplaması Başlıyor (Plug-in Mimarisi Testi)...");
            workOrder.TahminiFiyat = pricingEngine.CalculatePrice(workOrder, 100.0m);


            // 4. İş Emrini Çizelgeye Yerleştirme

            Console.WriteLine("\n[4] İş Emri Çizelgeye Yerleştiriliyor...");

            atananUsta.CalismaCizelgesi.AddWorkOrder(workOrder);

            DateOnly talepGunu = DateOnly.FromDateTime(workOrder.IlgiliTalep.IstenenZaman.Date);

            var oGuneAitIsler = atananUsta.CalismaCizelgesi[talepGunu];

            Console.WriteLine($"'{atananUsta.Ad}' ustasının {talepGunu} günündeki iş sayısı: {oGuneAitIsler.Count}");
            Console.WriteLine($"Kontrol Edilen İş Emri Fiyatı: {oGuneAitIsler.First().TahminiFiyat:C}");

            Console.WriteLine("\n==================================================");
            Console.WriteLine("=== Demo Senaryosu Tamamlandı ===");
            Console.WriteLine("==================================================");
        }

        static DateTime GetNextDay(DayOfWeek day)
        {
            DateTime start = DateTime.Today;
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }
    }
}