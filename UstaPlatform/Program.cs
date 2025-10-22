// UstaPlatform.App/Program.cs

// using ifadeleri, diğer projelere ve C# kütüphanelerine erişimi sağlar.
using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Pricing;// IPricingRule arayüzüne erişim için 
using UstaPlatform.Pricing; // PricingEngine sınıfına erişim için
using System;
using System.Linq;// FirstOrDefault ve First gibi metotlar için 
using System.Collections.Generic;

// Uygulamanın giriş noktası olan namespace (Çözümün ana adı)
namespace UstaPlatform.App
{
    class Program
    {
        // Uygulamanın giriş noktası (Ders 1: Main fonksiyonu)
        static void Main(string[] args)
        {
            // const: Değeri sabit olan ve derleme zamanında belirlenen değişken.
            const string PluginKlasoru = "Plugins";

            Console.WriteLine("==================================================");
            Console.WriteLine("=== UstaPlatform Uygulaması Başlatılıyor (NYP Projesi) ===");
            Console.WriteLine("==================================================");

            // 1. Fiyatlama Motorunu Hazırla ve Kuralları Yükle
            var pricingEngine = new PricingEngine(); // Fiyat motoru nesnesi oluşturuluyor.

            // Plug-in Mimarisi: Belirtilen klasördeki DLL'leri dinamik olarak yükler (OCP'yi kanıtlar) 
            Console.WriteLine("\n[1] Fiyatlama Kuralları Yükleniyor...");
           // LoadRules metodu, Reflection kullanarak klasörü tarar ve IPricingRule uygulayan sınıfları yükler. 
            pricingEngine.LoadRules(PluginKlasoru);

            // 2. Demo Verisi Oluştur (Nesne ve Koleksiyon Başlatıcıları Kullanımı)

            // Koleksiyon Başlatıcıları (Ders 3/Ders 5) ile Usta listesi oluşturma 
            var ustaList = new List<Usta>
            {
                // Nesne Başlatıcıları (Object Initializers) kullanımı (Ders 5) 
                new Usta { Ad = "Ahmet Usta", UzmanlikAlani = "Tesisatçı", Puan = 4.8m, Seviye = "Deneyimli" },
                new Usta { Ad = "Burak Usta", UzmanlikAlani = "Elektrikçi", Puan = 4.5m, Seviye = "Normal" }
            };
            // Vatandaş nesnesinin oluşturulması 
            var vatandas = new Vatandas { Ad = "Fatma Hanım", Adres = "Arcadia, Merkez Mah." };

            // Haftasonu Kuralını tetiklemek için bir sonraki Cumartesi gününü bulma
            DateTime cumartesi = GetNextDay(DayOfWeek.Saturday).AddHours(14);

             var talep = new Talep // Talep nesnesi oluşturuluyor 
            {
                Aciklama = "Acil su sızıntısı tamiri gerekiyor.",
                LokasyonX = 15,
                LokasyonY = 30,
                IstenenZaman = cumartesi // İşi ve zamanını tanımlama 
            };

            Console.WriteLine("\n[2] Yeni İş Akışı Başlatıldı (Tarih: {0})", talep.IstenenZaman.ToShortDateString());
            Console.WriteLine($"Talep Eden: {vatandas.Ad} | Talep: {talep.Aciklama}");

            // Basit Eşleştirme: İlk uygun ustayı seç (Açık uçlu kısım için basit çözüm) 
            var atananUsta = ustaList.FirstOrDefault(u => u.UzmanlikAlani == "Tesisatçı");

            if (atananUsta == null)
            {
                Console.WriteLine("Hata: Uygun usta bulunamadı.");
                return;
            }

            // İş Emri Oluşturma (WorkOrder) 
            // WorkOrder sınıfındaki Id, init-only özelliği ile korunur. 
            var workOrder = new WorkOrder
            {
                TalepEden = vatandas,
                AtananUsta = atananUsta,
                IlgiliTalep = talep,
                TahminiFiyat = 100.0m // Başlangıç/Temel fiyat
            };
            Console.WriteLine($"Atanan Usta: {atananUsta.Ad}");

            // Rota oluşturma (Özel IEnumerable<T> koleksiyon başlatıcıları ile - Ders 3/5) 
            workOrder.RotaDuraklari = new Route
            {
                { 10, 20 }, // Ustanın başlangıç konumu
                { talep.LokasyonX, talep.LokasyonY } // Talep adresi
            };
            Console.WriteLine($"Rota Durakları Oluşturuldu (2 durak): (10, 20) -> ({talep.LokasyonX}, {talep.LokasyonY})");


            // Fiyatı Hesapla
            Console.WriteLine("\n[3] Dinamik Fiyat Hesaplaması Başlıyor (Plug-in Mimarisi Testi)...");
            // Motor çağırılır. Motor, yüklediği kuralı Polimorfizm/DIP ile uygular.
            workOrder.TahminiFiyat = pricingEngine.CalculatePrice(workOrder, 100.0m);


            // 4. İş Emrini Çizelgeye Yerleştirme

            Console.WriteLine("\n[4] İş Emri Çizelgeye Yerleştiriliyor...");

            // İş emri, usta nesnesinin içindeki Schedule koleksiyonuna AddWorkOrder metoduyla eklenir. 
            atananUsta.CalismaCizelgesi.AddWorkOrder(workOrder);

            // DateOnly kullanımı (NET 6+ özelliği)
            DateOnly talepGunu = DateOnly.FromDateTime(workOrder.IlgiliTalep.IstenenZaman.Date);

            // Dizinleyici (Indexer) kullanımı: Schedule[DateOnly gün] ile işler geri alınır. 
            var oGuneAitIsler = atananUsta.CalismaCizelgesi[talepGunu];

            Console.WriteLine($"'{atananUsta.Ad}' ustasının {talepGunu} günündeki iş sayısı: {oGuneAitIsler.Count}");
            // Listenin ilk elemanının fiyatı kontrol edilir (Indexer'ın ve Add işleminin doğruluğu)
            Console.WriteLine($"Kontrol Edilen İş Emri Fiyatı: {oGuneAitIsler.First().TahminiFiyat:C}");

            Console.WriteLine("\n==================================================");
            Console.WriteLine("=== Demo Senaryosu Tamamlandı ===");
            Console.WriteLine("==================================================");
        }

        // Yardımcı metot: Bir sonraki belirli günü bulur (Main metodu dışında tanımlanmıştır)
        static DateTime GetNextDay(DayOfWeek day)
        {
            DateTime start = DateTime.Today;
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }
    }
}