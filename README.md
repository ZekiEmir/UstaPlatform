# UstaPlatform Projesi: Kısa Özet ve Teknik Odak Noktaları
Bu proje, Arcadia şehrinin talebi üzerine **genişletilebilirlik** ve **SOLID prensiplerine**  dayalı bir yazılım platformu olarak geliştirilmiştir.
Projenin ana amacı, dinamik fiyatlandırma yapabilmek ve **genişletilebilir**  bir yapı kurmaktı.

## 1\. Mimarinin Kalbi: Plug-in Sistemi ve OCP Kanıtı

Projenin en önemli kısmı, yeni bir kural eklendiğinde ana kodu değiştirmeme zorunluluğuydu

**Açık/Kapalı Prensibi (OCP) Uygulaması:** Yeni bir fiyat kuralı (Örn: Hafta sonu ücreti) eklendiğinde, ana uygulama kodunu yeniden derlemek yerine, sadece yeni bir sınıf veya DLL bırakılarak sisteme entegre edilebilmelidir
**Çözüm:** **`PricingEngine`** sınıfı, `Plugins` klasörünü tarar[cite: 33].Yansıma (Reflection) kullanarak , **`IPricingRule`** arayüzünü uygulayan  kuralları dinamik olarak yükler.
**Bağımlılıkların Tersine Çevrilmesi (DIP):** Fiyat motoru, somut sınıflara değil, **`IPricingRule`** gibi arayüzlere bağımlıdır
  * **Kanıt:** Konsol çıktısındaki `[Kural Yüklendi]: Haftasonu Ek Ücreti Kuralı` satırı, OCP'nin başarıyla uygulandığını gösterir.

## 2\. İleri C\# ve Varlık Yönetimi

Projedeki temel varlıklar, İleri C\# özellikleri kullanılarak tasarlanmıştır.

  * **`init-only` Özelliği:** `Id` (Kimlik) gibi alanlar, nesne oluşturulduktan sonra değiştirilemesin diye sadece başlatma sırasında atanabilir
  * **Dizinleyici (Indexer):** `Schedule` sınıfında, `Schedule[DateOnly gün]` yapısı kullanılarak o güne ait iş emirleri listesine kolay erişim sağlanır
  * **Özel Koleksiyon:** `Route` sınıfı, `IEnumerable<(int X, int Y)>` arayüzünü uygulamalıdır
  * **Nesne Başlatıcılar:** Domain nesneleri oluşturulurken okunaklılık için yoğun olarak kullanılmıştır

## 3\. Demo Çıktısı Özeti

Çıktı, hafta sonu talebinde, dinamik olarak yüklenen kuralın fiyatı $100.00'dan $120.00'a (%20) çıkardığını kanıtlamaktadır:

**Senaryo:** Yeni bir kural DLL'i (`LoyaltyDiscountRule.dll` gibi) projeye bırakılır ve uygulama çalıştırıldığında, bu yeni kuralın fiyat hesaplamasına otomatik olarak dahil olduğu gösterilir


=== Demo Senaryosu Tamamlandı ===
```
