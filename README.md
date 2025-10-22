Bu proje, Arcadia şehrinin talebi üzerine genişletilebilir ve bakımı kolay bir yazılım platformu olarak geliştirilmiştir.
1. Mimarinin Kalbi: Plug-in Sistemi ve SOLID:
2. Projenin en önemli kısmı, yeni bir mahalle kuralı eklendiğinde ana kodu değiştirmeme zorunluluğuydu. Bu, yazılım tasarımında birincil öncelikti.
3. A. Açık/Kapalı Prensibi (OCP) Uygulaması
4. Amaç: Sistemin yeni özelliklere açık, ancak ana kodda değişikliğe kapalı olmasını sağlamak
5. Çözüm: PricingEngine sınıfı, somut bir kurala bağımlı olmak yerine, sadece IPricingRule arayüzünü tanır.
6. Çalışma Anı: Uygulama başlarken, motor Plugins klasörünü tarar
UstaPlatform.Rules.Weekend.dll dosyasını bulur ve Yansıma (Reflection) kullanarak kuralı dinamik olarak sisteme ekler.
Kanıt: Konsol çıktısındaki [Kural Yüklendi]: Haftasonu Ek Ücreti Kuralı satırı, bu mimarinin çalıştığını kanıtlar.
B. Bağımlılıkların Tersine Çevrilmesi (DIP):
PricingEngine gibi üst katman modülleri, somut sınıflara değil, IPricingRule gibi arayüzlere bağımlıdır.
2. İleri C# ve Varlık Yönetimi:
3. Özellik,Kullanım Amacı,Örnek
init-only,,"Id ve KayitZamani  gibi alanların, nesne oluşturulduktan sonra yanlışlıkla değiştirilmesini engeller. Bu, veri bütünlüğünü sağlar.",public Guid Id { get; init; } 
Dizinleyici (Indexer),,"Schedule sınıfında (Schedule[DateOnly gün]), o güne ait iş emirlerine kolay erişim sağlar.",,public List<WorkOrder> this[DateOnly gun] 
Özel Koleksiyon,,"Route sınıfı, koleksiyon başlatıcıları desteklemek için IEnumerable arayüzünü uygulamalı ve Add metoduna sahip olmalıdır.",,"new Route { {10, 20} } "
