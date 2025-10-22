using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatform.Domain.Entities
{
    public class Usta
    {
        // init-only: Nesne oluşturulduktan sonra değiştirilemez (Gereksinim)
        public Guid Id { get; init; } = Guid.NewGuid();

        // CS8618 hatasını çözmek için = string.Empty atıyoruz.
        public string Ad { get; set; } = string.Empty;
        public string UzmanlikAlani { get; set; } = string.Empty;
        public decimal Puan { get; set; }
        public string Seviye { get; set; } = "Normal";

        // Ustanın takvimi
        public Schedule CalismaCizelgesi { get; set; } = new Schedule();
    }
}