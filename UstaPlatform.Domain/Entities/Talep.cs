
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Nesne Yönelimli Programlama (NYP) gereği varlıklarımızı bir namespace altında topluyoruz.
namespace UstaPlatform.Domain.Entities
{
    // Talep sınıfı, vatandaşın açtığı iş talebini (Sızıntı tamiri, bilgisayar sorunu vb.) tanımlar.
    // Tek Sorumluluk Prensibi (SRP): Bu sınıfın tek sorumluluğu, işi ve zamanını tanımlamaktır
    public class Talep
    {
        // init-only özelliği: Id (Kimlik) alanı, nesne oluşturulduktan sonra değiştirilemez (Veri bütünlüğü).
        public Guid Id { get; init; } = Guid.NewGuid();

        // İşin ne olduğunu açıklayan metin. Null güvenliği için string.Empty ile başlatıldı.
        public string Aciklama { get; set; } = string.Empty;

        // İşin coğrafi konumu (X koordinatı).
        public int LokasyonX { get; set; }

        // İşin coğrafi konumu (Y koordinatı).
        public int LokasyonY { get; set; }

        // Vatandaşın hizmeti istediği zaman. İşin planlanması için kritik alandır.
        public DateTime IstenenZaman { get; set; }
    }
}