# ECommerceApp

Bu proje, Yazılım Test ve Kalitesi dersi kapsamında geliştirilen basit bir e-ticaret uygulamasıdır. Projede temel bir alışveriş süreci modellenmiş ve bu süreç farklı test türleriyle kontrol edilmiştir.

Uygulamada kullanıcı ürün seçer, ürünleri sepete ekler, sipariş oluşturur ve ödeme işlemini gerçekleştirir. Final çalışması kapsamında mevcut sisteme stok kontrolü, indirim uygulaması ve minimum sipariş tutarı kontrolü gibi ek senaryolar dahil edilmiştir.

## Projenin Amacı

Bu projenin amacı, bir e-ticaret sistemi üzerinden yazılım test süreçlerini uygulamalı olarak göstermektir. Projede yalnızca başarılı senaryolar değil, bilerek bırakılmış hatalı durumlar da bulunmaktadır. Bu hatalar testler aracılığıyla tespit edilmekte ve raporda açıklanmaktadır.

Bu kapsamda aşağıdaki konular ele alınmıştır:

* Unit test
* White box test
* Black box test
* Gray box test
* Integration test
* Equivalence Partitioning
* Boundary Value Analysis
* STLC süreci
* Hata kavramları
* Test raporlama

## Kullanılan Teknolojiler

* C#
* .NET 7
* NUnit
* Microsoft.NET.Test.Sdk
* NUnit3TestAdapter

## Proje Yapısı

```text
ECommerceApp/
├── Core/
│   ├── Product.cs
│   ├── Cart.cs
│   └── OrderService.cs
│
├── Tests/
│   ├── UnitTests/
│   └── IntegrationTests/
│
├── Program.cs
├── ECommerceApp.csproj
├── README.md
├── Report.md
└── TEST_CASES.md
```

## Uygulama Senaryosu

Uygulama temel olarak aşağıdaki alışveriş akışını içerir:

1. Ürünler tanımlanır.
2. Kullanıcı ürün seçer.
3. Ürünler sepete eklenir.
4. Sipariş oluşturulur.
5. Ödeme yapılır.
6. Sistem stok, indirim ve minimum sipariş tutarı kontrollerini gerçekleştirir.

Program çalıştırıldığında örnek bir alışveriş senaryosu konsol üzerinden gösterilir. Bu senaryoda bazı hatalı durumlar da bilinçli olarak gösterilmektedir.

## Finalde Eklenen Özellikler

Final çalışması kapsamında projeye aşağıdaki kontroller eklenmiştir:

### Stok Kontrolü

Ürün stokta yoksa veya istenen miktar stoktan fazlaysa sipariş verilmemesi beklenir. Projede bu alanla ilgili bilerek bırakılmış hatalar testlerle kontrol edilmektedir.

### İndirim Uygulaması

Sipariş toplamına indirim uygulanması senaryosu eklenmiştir. Farklı indirim oranları için testler yazılmıştır. Bazı indirim hesaplama hataları testler aracılığıyla tespit edilmektedir.

### Minimum Sipariş Tutarı

Sipariş oluşturulabilmesi için minimum sipariş tutarı kontrolü eklenmiştir. Sınır değerler ve geçersiz değerler test edilmiştir.

## Bilerek Bırakılan Hatalar

Proje içinde testlerle tespit edilmesi amacıyla bazı hatalar bilinçli olarak bırakılmıştır. Bunlardan bazıları şunlardır:

* Stokta olmayan ürünün sepete eklenebilmesi
* Stok miktarının negatif değere düşebilmesi
* İndirim oranının yanlış hesaplanması
* Minimum sipariş tutarının doğru kontrol edilmemesi
* Aynı ürün sepete tekrar eklendiğinde miktarın doğru birleştirilmemesi
* Eksik ödeme tutarının kabul edilmesi
* Sipariş durum geçişlerinde hatalı davranışlar

Bu hatalar, yazılım testlerinin sistemdeki problemleri nasıl ortaya çıkardığını göstermek amacıyla projede korunmuştur.

## Test Türleri

Projede dört farklı test yaklaşımı kullanılmıştır.

### White Box Test

Kodun iç yapısı ve metotların çalışma mantığı dikkate alınarak yazılmış testlerdir. Özellikle stok azaltma, fiyat kontrolü ve iç hesaplama mantıkları bu kapsamda test edilmiştir.

### Black Box Test

Sistemin iç yapısına bakmadan, verilen girdilere karşılık beklenen çıktılar kontrol edilmiştir. Sepete ürün ekleme, stok aşımı ve geçersiz durumlar bu testlerde ele alınmıştır.

### Gray Box Test

Hem sistem davranışı hem de kod yapısı hakkında kısmi bilgi kullanılarak yazılmış testlerdir. Sipariş durumu, ödeme işlemi, indirim ve minimum sipariş kontrolleri bu kapsamda değerlendirilmiştir.

### Integration Test

Ürün, sepet ve sipariş servisinin birlikte çalışması test edilmiştir. Gerçek alışveriş akışına yakın senaryolar integration testler ile kontrol edilmiştir.

## Test Case Design

Test senaryolarında Equivalence Partitioning ve Boundary Value Analysis teknikleri kullanılmıştır.

Örnek değer grupları:

* Geçerli ürün miktarı
* Geçersiz ürün miktarı
* Sıfır stok
* Stok sınırının üzerindeki miktar
* Minimum sipariş tutarının altı
* Minimum sipariş tutarına yakın sınır değerler
* Geçerli ve geçersiz indirim oranları

## Test Sonuçları

Projede toplam 28 test case bulunmaktadır.

```text
Total: 28
Passed: 13
Failed: 15
Skipped: 0
```

Fail olan testler, sistemde bilerek bırakılan hataları göstermektedir. Bu nedenle testlerin tamamının başarılı olması hedeflenmemiştir. Amaç, hatalı davranışların testlerle tespit edilebildiğini göstermektir.

## Projeyi Çalıştırma

Projeyi çalıştırmak için:

```bash
dotnet run --project ./ECommerceApp.csproj
```

Testleri çalıştırmak için:

```bash
dotnet test ./ECommerceApp.csproj
```

## Raporlama

Proje içinde ayrıca detaylı rapor ve test senaryoları dosyaları bulunmaktadır.

```text
Report.md
TEST_CASES.md
```

`Report.md` dosyasında STLC süreci, test türleri, fail olan testler, hata listesi ve test stratejileri açıklanmıştır.

`TEST_CASES.md` dosyasında test senaryoları, kullanılan test tasarım teknikleri ve beklenen sonuçlar yer almaktadır.

## STLC Süreci

Projede test süreci aşağıdaki adımlara göre ele alınmıştır:

1. Requirement Analysis
2. Test Planning
3. Test Design
4. Test Execution
5. Test Result and Reporting

Her adım rapor dosyasında proje kapsamına göre açıklanmıştır.

## Test Stratejileri

Projede aşağıdaki test stratejilerine kısaca yer verilmiştir:

* Agile Testing
* Risk-Based Testing
* Regression Testing

Bu stratejiler, e-ticaret sistemindeki kritik akışların daha kontrollü şekilde test edilmesi amacıyla açıklanmıştır.

## Sonuç

Bu proje, basit bir e-ticaret uygulaması üzerinden yazılım test kavramlarını uygulamalı olarak göstermektedir. Projede hem başarılı senaryolar hem de bilerek bırakılmış hatalı durumlar bulunmaktadır. Yazılan testler sayesinde sistemdeki hataların nasıl tespit edildiği gösterilmiş ve test sonuçları raporlanmıştır.
