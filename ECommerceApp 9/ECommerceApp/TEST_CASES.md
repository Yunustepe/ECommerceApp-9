# Test Case Dökümanı — ECommerceApp Final

Bu dosya projede yazılan tüm 28 test senaryosunu içermektedir.
EP (Equivalence Partitioning) ve BVA (Boundary Value Analysis) teknikleri uygulanmıştır.

---

## Genel Sayılar

| Kapsam | Test Sayısı |
|---|---:|
| White Box (Unit) | 4 |
| Black Box | 5 |
| Gray Box | 6 |
| Integration | 4 |
| BVA / EP (Final) | 9 |
| **Toplam** | **28** |

---

## Kullanılan Test Tasarım Teknikleri

### Equivalence Partitioning (EP)

| Özellik | Geçerli Sınıf | Geçersiz Sınıf |
|---|---|---|
| İndirim oranı | %0 - %100 arası | %0'dan küçük veya %100'den büyük |
| Sipariş tutarı | 50 TL ve üzeri | 50 TL altı |
| Ürün miktarı | 1 ve üzeri | 0 veya negatif |

### Boundary Value Analysis (BVA)

| Alan | Test Edilen Sınır Değerler |
|---|---|
| Minimum sipariş tutarı | 9 TL, 10 TL, 49.99 TL, 50.00 TL |
| İndirim oranı | %0, %1, %100 |
| Stok miktarı | 0, 1 |

---

## Test Case Listesi

### 1. White Box (Unit) Testleri — Product

| TC No | Test Adı | Teknik | Beklenen Sonuç | Durum |
|---|---|---|---|---|
| TC-01 | `SetPrice_NegativePrice_ShouldThrowException` | EP (Geçersiz) | ArgumentException fırlatılmalı | **Fail** (BUG-01) |
| TC-02 | `DecreaseStock_ExceedsStock_ShouldReturnFalse` | EP (Geçersiz) | false dönmeli, stok negatif olmamalı | **Fail** (BUG-02) |
| TC-03 | `DecreaseStock_ValidQuantity_ShouldSucceed` | EP (Geçerli) | true dönmeli, stok 7 olmalı | **Pass** |
| TC-04 | `IsAvailable_StockIsZero_ShouldReturnFalse` | BVA (Sınır=0) | false dönmeli | **Pass** |

### 2. Black Box Testleri — Cart

| TC No | Test Adı | Teknik | Beklenen Sonuç | Durum |
|---|---|---|---|---|
| TC-05 | `AddItem_SameProductTwice_ShouldMergeQuantities` | EP (Geçerli) | Item sayısı 1, quantity 3 olmalı | **Fail** (BUG-03) |
| TC-06 | `AddItem_ExceedsStock_ShouldThrowException` | EP (Geçersiz) | InvalidOperationException fırlatılmalı | **Fail** (BUG-04) |
| TC-07 | `GetItemCount_ReturnsCorrectTotalQuantity` | EP (Geçerli) | 5 dönmeli | **Fail** (BUG-06) |
| TC-08 | `AddItem_ZeroQuantity_ShouldThrowException` | BVA (Sınır=0) | ArgumentException fırlatılmalı | **Pass** |
| TC-09 | `RemoveItem_ExistingProduct_ShouldBeRemovedFromCart` | EP (Geçerli) | Sepet boş olmalı | **Pass** |

### 3. Gray Box Testleri — OrderService

| TC No | Test Adı | Teknik | Beklenen Sonuç | Durum |
|---|---|---|---|---|
| TC-10 | `PlaceOrder_EmptyCart_ShouldThrowException` | EP (Geçersiz) | InvalidOperationException fırlatılmalı | **Fail** (BUG-07) |
| TC-11 | `ProcessPayment_InsufficientAmount_ShouldReturnFalse` | EP (Geçersiz) | false dönmeli, Pending kalmalı | **Fail** (BUG-08) |
| TC-12 | `CancelOrder_AlreadyCancelled_ShouldReturnFalse` | EP (Geçersiz) | false dönmeli | **Fail** (BUG-09) |
| TC-13 | `ShipOrder_PendingOrder_ShouldReturnFalse` | EP (Geçersiz) | false dönmeli | **Fail** (BUG-10) |
| TC-14 | `ProcessPayment_ExactAmount_ShouldConfirmOrder` | EP (Geçerli) | true dönmeli, Confirmed olmalı | **Pass** |

### 4. Integration Testleri

| TC No | Test Adı | Teknik | Beklenen Sonuç | Durum |
|---|---|---|---|---|
| TC-15 | `FullPurchaseFlow_ValidProducts_ShouldSucceed` | EP (Geçerli) | Tam akış başarılı olmalı | **Pass** |
| TC-16 | `PurchaseFlow_OutOfStockProduct_ShouldFailAtCartLevel` | BVA (Sınır=0) | Exception fırlatılmalı | **Fail** (BUG-04) |
| TC-17 | `PlaceOrder_ShouldDecreaseProductStock` | EP (Geçerli) | Stok azalmalı | **Pass** |
| TC-18 | `MultipleOrders_ShouldBeTrackedIndependently` | EP (Geçerli) | Siparişler bağımsız yönetilmeli | **Pass** |

### 5. BVA & EP — Minimum Sipariş Tutarı Testleri

| TC No | Test Adı | Teknik | Sınır Değer | Beklenen Sonuç | Durum |
|---|---|---|---|---|---|
| TC-19 | `PlaceOrder_MinimumOrder_Boundary_49_99_BVA` | BVA (Geçersiz Sınır) | 49.99 TL | Exception fırlatılmalı | **Fail** (BUG-13) |
| TC-20 | `PlaceOrder_MinimumOrder_Boundary_50_00_BVA` | BVA (Geçerli Sınır) | 50.00 TL | Sipariş onaylanmalı | **Pass** |
| TC-21 | `PlaceOrder_MinimumOrder_Boundary_10_00_BVA` | BVA (Geçersiz Sınır) | 10.00 TL | Exception fırlatılmalı | **Fail** (BUG-13) |
| TC-22 | `PlaceOrder_MinimumOrder_Invalid_BelowTen_EP` | EP (Geçersiz) | 9.00 TL | Exception fırlatılmalı | **Pass** |

### 6. BVA & EP — İndirim Testleri

| TC No | Test Adı | Teknik | Sınır Değer | Beklenen Sonuç | Durum |
|---|---|---|---|---|---|
| TC-23 | `PlaceOrder_Discount_ZeroPercent_BVA` | BVA (Sınır=%0) | %0 | Tutar değişmemeli | **Pass** |
| TC-24 | `PlaceOrder_Discount_TenPercent_EP` | EP (Geçerli) | %10 | 13500 TL olmalı | **Fail** (BUG-12) |
| TC-25 | `PlaceOrder_Discount_Boundary_OnePercent_BVA` | BVA (Sınır=%1) | %1 | 14850 TL olmalı | **Fail** (BUG-12) |
| TC-26 | `PlaceOrder_Discount_Boundary_100Percent_BVA` | BVA (Sınır=%100) | %100 | 0 TL olmalı | **Fail** (BUG-12) |
| TC-27 | `PlaceOrder_Discount_Negative_Invalid_EP` | EP (Geçersiz) | -%5 | ArgumentOutOfRangeException | **Pass** |

### 7. BVA — Stok Kontrolü Testi

| TC No | Test Adı | Teknik | Sınır Değer | Beklenen Sonuç | Durum |
|---|---|---|---|---|---|
| TC-28 | `PlaceOrder_StockControl_ZeroStock_BVA` | BVA (Sınır=0) | Stok=0 | Exception fırlatılmalı | **Fail** (BUG-11) |

---

## Özet

| Durum | Sayı |
|---|---:|
| **Toplam** | **28** |
| Geçti (Pass) | 13 |
| Başarısız (Fail) | 15 |

> Başarısız olan 15 test, sistemde bilerek bırakılan 13 bug'ı tespit etmektedir.
