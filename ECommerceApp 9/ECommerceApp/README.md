# ECommerceApp — Yazılım Test ve Kalitesi Final Projesi

Bu proje, MTH2005 Yazılım Test ve Kalitesi dersi final ödevi kapsamında geliştirilmiştir.
Vize projesindeki e-ticaret sistemi, final gereksinimleriyle genişletilmiş ve NUnit testleriyle kapsamlı biçimde test edilmiştir.

## Kullanıcı Akışı

1. Kullanıcı ürün seçer.
2. Ürünü sepete ekler.
3. Sipariş verir.
4. Ödeme yapar.

## Final Kapsamında Eklenen Senaryolar

- **Stok Kontrolü** — Stokta olmayan ürün sipariş edilememeli.
- **İndirim Uygulaması** — Yüzde veya sabit tutarlı indirim uygulanabilmeli.
- **Minimum Sipariş Tutarı** — Sepet tutarı 50 TL'nin altındaysa sipariş reddedilmeli.

> ⚠️ Sistem içerisinde bilerek hatalar (bug) bırakılmıştır. Bu hatalar testlerle tespit edilmektedir.

---

## Proje Dosya Yapısı

```
ECommerceApp/
 ├── Core/
 │    ├── Product.cs
 │    ├── Cart.cs
 │    ├── Discount.cs          ← Final: indirim türleri
 │    └── OrderService.cs
 │
 ├── Tests/
 │    ├── UnitTests/
 │    │    └── ECommerceTests.cs   (White Box, Black Box, Gray Box)
 │    └── IntegrationTests/
 │         └── IntegrationTests.cs
 │
 ├── Program.cs
 ├── Report.md                 ← STLC, bug listesi, test özeti
 ├── TEST_CASES.md             ← 28 test case, EP ve BVA teknikleri
 └── README.md
```

---

## Uygulamayı Çalıştırma

```bash
cd ECommerceApp
dotnet run
```

---

## Testleri Çalıştırma

```bash
dotnet test
```

Detaylı çıktı için:

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Beklenen Test Özeti

| Durum | Sayı |
|---|---:|
| Toplam test | 28 |
| Geçti (Passed) | 13 |
| Başarısız (Failed) | 15 |

> Başarısız testler, sistemde bilerek bırakılan hataları göstermek için tasarlanmıştır.

---

## Bilerek Bırakılan Bug'lar

| Bug ID | Bileşen | Açıklama |
|---|---|---|
| BUG-01 | Product | Negatif fiyata izin veriliyor. |
| BUG-02 | Product | Stok negatife düşebiliyor. |
| BUG-03 | Cart | Aynı ürün tekrar eklenince miktar birleştirilmiyor. |
| BUG-04 | Cart | Sepete ekleme sırasında stok kontrolü yok. |
| BUG-05 | Cart | GetTotal indirim ve vergileri hesaba katmıyor. |
| BUG-06 | Cart | GetItemCount toplam miktar yerine satır sayısı döndürüyor. |
| BUG-07 | OrderService | Boş sepete sipariş verilebiliyor. |
| BUG-08 | OrderService | Eksik ödeme tutarı onaylanıyor. |
| BUG-09 | OrderService | İptal edilmiş sipariş tekrar iptal edilebiliyor. |
| BUG-10 | OrderService | Ödeme yapılmadan sipariş kargoya verilebiliyor. |
| BUG-11 | OrderService | Stok 0 olduğunda stok kontrolü bypass ediliyor. |
| BUG-12 | OrderService | Her indirimde sabit %50 uygulanıyor. |
| BUG-13 | OrderService | Minimum sipariş limiti 50 TL yerine 10 TL olarak uygulanıyor. |

---

## Kullanılan Teknolojiler

- **Dil:** C# (.NET 9.0)
- **Test Kütüphanesi:** NUnit
- **Test Türleri:** Unit (White Box), Black Box, Gray Box, Integration
- **Test Tasarım Teknikleri:** Equivalence Partitioning (EP), Boundary Value Analysis (BVA)
