using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatform.Domain.Entities
{
    public class WorkOrder
    {
        // init-only: Nesne oluşturulduktan sonra değiştirilemez (Gereksinim)
        public Guid Id { get; init; } = Guid.NewGuid();

        // CS0122 hatasını çözmek için kurucuyu public yapın
        public WorkOrder()
        {
            // Nesne başlatıcıları (new WorkOrder { ... }) kullanımını destekler.
            TalepEden = new Vatandas();
            AtananUsta = new Usta();
            IlgiliTalep = new Talep();
            RotaDuraklari = new Route();
        }

        public Vatandas TalepEden { get; set; }
        public Usta AtananUsta { get; set; }
        public Talep IlgiliTalep { get; set; }
        public decimal TahminiFiyat { get; set; }

        // Rota durak bilgisi.
        public Route RotaDuraklari { get; set; }
    }
}
