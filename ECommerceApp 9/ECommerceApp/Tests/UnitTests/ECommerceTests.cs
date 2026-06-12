using System;
using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.UnitTests
{
    // ==========================================
    // 1. WHITE BOX (UNIT) TESTS - Product
    // İç mantığı bilerek test ediyoruz
    // ==========================================
    [TestFixture]
    public class ProductWhiteBoxTests
    {
        private Product _product = null!;

        [SetUp]
        public void Setup()
        {
            _product = new Product(1, "Test Ürünü", 100m, 10);
        }

        // TC-01: WHITE BOX - SetPrice negatif değer kabul etmemeli
        [Test]
        public void SetPrice_NegativePrice_ShouldThrowException()
        {
            // Arrange
            decimal negativePrice = -50m;

            // Act & Assert
            // BUG #1: Bu test FAIL olacak çünkü negatif fiyata izin veriliyor
            Assert.Throws<ArgumentException>(() => _product.SetPrice(negativePrice),
                "Negatif fiyat set edildiğinde ArgumentException fırlatılmalı.");
        }

        // TC-02: WHITE BOX - DecreaseStock stoku negatife düşürmemeli
        [Test]
        public void DecreaseStock_ExceedsStock_ShouldReturnFalse()
        {
            // Arrange
            int excessiveQuantity = 100; // Stok sadece 10

            // Act
            // BUG #2: Bu test FAIL olacak çünkü stok negatife düşüyor ve true dönüyor
            bool result = _product.DecreaseStock(excessiveQuantity);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False, "Stok yetersizse false dönmeli.");
                Assert.That(_product.Stock, Is.GreaterThanOrEqualTo(0), "Stok negatif olmamalı.");
            });
        }

        // TC-03: WHITE BOX - Geçerli stok azaltma başarılı olmalı
        [Test]
        public void DecreaseStock_ValidQuantity_ShouldSucceed()
        {
            // Arrange
            int quantity = 3;

            // Act
            bool result = _product.DecreaseStock(quantity);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Geçerli miktarda stok azaltma başarılı olmalı.");
                Assert.That(_product.Stock, Is.EqualTo(7), "Stok doğru hesaplanmalı.");
            });
        }

        // TC-04: WHITE BOX - IsAvailable stok 0 iken false dönmeli
        [Test]
        public void IsAvailable_StockIsZero_ShouldReturnFalse()
        {
            // Arrange
            var emptyStockProduct = new Product(2, "Tükendi", 50m, 0);

            // Act
            bool available = emptyStockProduct.IsAvailable();

            // Assert
            Assert.That(available, Is.False, "Stok 0 iken IsAvailable false dönmeli.");
        }
    }

    // ==========================================
    // 2. BLACK BOX TESTS - Cart
    // Sadece giriş/çıkış davranışını test ediyoruz
    // ==========================================
    [TestFixture]
    public class CartBlackBoxTests
    {
        private Cart _cart = null!;
        private Product _product1 = null!;
        private Product _product2 = null!;

        [SetUp]
        public void Setup()
        {
            _cart = new Cart();
            _product1 = new Product(1, "Laptop", 15000m, 5);
            _product2 = new Product(2, "Mouse", 500m, 20);
        }

        // TC-05: BLACK BOX - Aynı ürün iki kez eklenince quantity birleşmeli
        [Test]
        public void AddItem_SameProductTwice_ShouldMergeQuantities()
        {
            // Act
            _cart.AddItem(_product1, 1);
            _cart.AddItem(_product1, 2);

            // Assert
            // BUG #3: Bu test FAIL olacak çünkü aynı ürün iki ayrı item olarak ekleniyor
            Assert.Multiple(() =>
            {
                Assert.That(_cart.Items, Has.Count.EqualTo(1),
                    "Aynı ürün tekrar eklenince item sayısı artmamalı, quantity güncellenmeli.");
                Assert.That(_cart.Items[0].Quantity, Is.EqualTo(3),
                    "Aynı ürün eklenince toplam quantity 3 olmalı.");
            });
        }

        // TC-06: BLACK BOX - Stoktan fazla ürün eklenemez
        [Test]
        public void AddItem_ExceedsStock_ShouldThrowException()
        {
            // Act & Assert
            // BUG #4: Bu test FAIL olacak çünkü stok kontrolü yapılmıyor
            Assert.Throws<InvalidOperationException>(() => _cart.AddItem(_product1, 100),
                "Stoktan fazla ürün eklenince exception fırlatılmalı.");
        }

        // TC-07: BLACK BOX - GetItemCount toplam adet sayısını vermeli
        [Test]
        public void GetItemCount_ReturnsCorrectTotalQuantity()
        {
            // Arrange
            _cart.AddItem(_product1, 3);
            _cart.AddItem(_product2, 2);

            // Act
            int count = _cart.GetItemCount();

            // Assert
            // BUG #6: Bu test FAIL olacak çünkü GetItemCount unique ürün sayısı dönüyor (2), toplam 5 değil
            Assert.That(count, Is.EqualTo(5),
                "GetItemCount toplam ürün adedini (quantity toplamı) dönmeli.");
        }

        // TC-08: BLACK BOX - Sıfır quantity ile ürün eklenemez
        [Test]
        public void AddItem_ZeroQuantity_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _cart.AddItem(_product1, 0),
                "Sıfır quantity ile ürün eklenince ArgumentException fırlatılmalı.");
        }

        // TC-09: BLACK BOX - Ürün kaldırıldıktan sonra sepetten silinmeli
        [Test]
        public void RemoveItem_ExistingProduct_ShouldBeRemovedFromCart()
        {
            // Arrange
            _cart.AddItem(_product1, 1);

            // Act
            _cart.RemoveItem(_product1.Id);

            // Assert
            Assert.That(_cart.Items, Has.Count.EqualTo(0), "Ürün kaldırıldıktan sonra sepet boş olmalı.");
        }
    }

    // ==========================================
    // 3. GRAY BOX TESTS - OrderService
    // Kısmi iç bilgiyle test ediyoruz
    // ==========================================
    [TestFixture]
    public class OrderServiceGrayBoxTests
    {
        private OrderService _orderService = null!;
        private Cart _cart = null!;
        private Product _product = null!;

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService();
            _cart = new Cart();
            _product = new Product(1, "Laptop", 15000m, 5);
        }

        // TC-10: GRAY BOX - Boş sepete sipariş verilemez
        [Test]
        public void PlaceOrder_EmptyCart_ShouldThrowException()
        {
            // Act & Assert
            // BUG #7: Bu test FAIL olacak çünkü boş sepet kontrolü yok
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart),
                "Boş sepete sipariş verilince InvalidOperationException fırlatılmalı.");
        }

        // TC-11: GRAY BOX - Eksik ödeme tutarıyla sipariş onaylanmamalı
        [Test]
        public void ProcessPayment_InsufficientAmount_ShouldReturnFalse()
        {
            // Arrange
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);
            decimal insufficientAmount = order.TotalAmount - 1m; // 1 TL eksik

            // Act
            // BUG #8: Bu test FAIL olacak çünkü tutar kontrolü yapılmıyor
            bool result = _orderService.ProcessPayment(order, insufficientAmount);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False, "Eksik ödeme tutarı kabul edilmemeli.");
                Assert.That(order.Status, Is.EqualTo(OrderStatus.Pending),
                    "Başarısız ödemede sipariş Pending kalmalı.");
            });
        }

        // TC-12: GRAY BOX - İptal edilmiş sipariş tekrar iptal edilemez
        [Test]
        public void CancelOrder_AlreadyCancelled_ShouldReturnFalse()
        {
            // Arrange
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);
            _orderService.CancelOrder(order.OrderId);

            // Act
            // BUG #9: Bu test FAIL olacak çünkü durum kontrolü yapılmıyor
            bool result = _orderService.CancelOrder(order.OrderId);

            // Assert
            Assert.That(result, Is.False,
                "Zaten iptal edilmiş sipariş tekrar iptal edilemez, false dönmeli.");
        }

        // TC-13: GRAY BOX - Confirmed olmayan sipariş kargoya verilemez
        [Test]
        public void ShipOrder_PendingOrder_ShouldReturnFalse()
        {
            // Arrange
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);
            // Ödeme yapılmadan (hala Pending durumunda)

            // Act
            // BUG #10: Bu test FAIL olacak çünkü Confirmed kontrolü yapılmıyor
            bool result = _orderService.ShipOrder(order.OrderId);

            // Assert
            Assert.That(result, Is.False,
                "Pending durumundaki sipariş kargoya verilemez.");
        }

        // TC-14: GRAY BOX - Başarılı tam ödeme sonrası sipariş Confirmed olmalı
        [Test]
        public void ProcessPayment_ExactAmount_ShouldConfirmOrder()
        {
            // Arrange
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);

            // Act
            bool result = _orderService.ProcessPayment(order, order.TotalAmount);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Tam tutar ödenince ödeme başarılı olmalı.");
                Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed),
                    "Ödeme sonrası sipariş Confirmed olmalı.");
            });
        }

        // ==========================================
        // YENİ GEREKSİNİMLER - BVA & EP BİLEŞEN TESTLERİ
        // ==========================================

        // --- Minimum Sipariş Tutarı Testleri ---

        // TC-19: BVA (Boundary Invalid - EXPECTED FAIL) - Minimum sipariş tutarı sınırı 49.99 TL
        [Test]
        public void PlaceOrder_MinimumOrder_Boundary_49_99_BVA()
        {
            var cheapProduct = new Product(99, "Sınır Ürün", 49.99m, 5);
            _cart.AddItem(cheapProduct, 1);

            // Beklenen: Minimum limit (50.00 TL) altındaki siparişin reddedilmesi/hata fırlatılmasıdır.
            // BUG #13: Kod sadece 10 TL altını engellediği için bu siparişi onaylar ve test FAIL olur.
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart),
                "49.99 TL olan sipariş minimum 50 TL limitine takılıp hata fırlatmalıydı.");
        }

        // TC-20: BVA (Boundary Valid) - Minimum sipariş tutarı sınırı 50.00 TL (Tam sınır)
        [Test]
        public void PlaceOrder_MinimumOrder_Boundary_50_00_BVA()
        {
            var cheapProduct = new Product(100, "Sınır Ürün", 50.00m, 5);
            _cart.AddItem(cheapProduct, 1);

            // Beklenen: Tam 50.00 TL sipariş kabul edilmelidir.
            var order = _orderService.PlaceOrder(_cart);
            Assert.That(order, Is.Not.Null);
            Assert.That(order.TotalAmount, Is.EqualTo(50.00m));
        }

        // TC-21: BVA (Boundary Invalid - EXPECTED FAIL) - Minimum sipariş tutarı sınırı 10.00 TL
        [Test]
        public void PlaceOrder_MinimumOrder_Boundary_10_00_BVA()
        {
            var cheapProduct = new Product(101, "Sınır Ürün", 10.00m, 5);
            _cart.AddItem(cheapProduct, 1);

            // Beklenen: 10 TL, 50 TL'nin altında olduğu için reddedilmelidir.
            // BUG #13: 10 TL tam sınır kabul edilip onaylanacağı için test FAIL olur.
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart),
                "10.00 TL olan sipariş 50 TL limitinin altında olduğu için hata fırlatmalıydı.");
        }

        // TC-22: EP (Invalid - Validated) - Minimum sipariş 9.00 TL (10 TL altı)
        [Test]
        public void PlaceOrder_MinimumOrder_Invalid_BelowTen_EP()
        {
            var cheapProduct = new Product(102, "Ucuz Kalem", 9.00m, 5);
            _cart.AddItem(cheapProduct, 1);

            // Beklenen: 9 TL, 10 TL altı olduğu için buggy kod tarafından da reddedilir. Test PASS geçer.
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart));
        }

        // --- İndirim (Discount) Testleri ---

        // TC-23: BVA (Boundary Valid) - %0 indirim uygulanması
        [Test]
        public void PlaceOrder_Discount_ZeroPercent_BVA()
        {
            _cart.AddItem(_product, 1); // 15000 TL
            var order = _orderService.PlaceOrder(_cart, 0);

            Assert.That(order.TotalAmount, Is.EqualTo(15000m), "0% indirim tutarı değiştirmemeli.");
        }

        // TC-24: EP (Valid - EXPECTED FAIL) - %10 indirim uygulanması
        [Test]
        public void PlaceOrder_Discount_TenPercent_EP()
        {
            _cart.AddItem(_product, 1); // 15000 TL
            decimal expectedTotal = 15000m * 0.90m; // 13500 TL

            // Act
            // BUG #12: İndirim uygulandığında sabit %50 indirim (7500 TL) uygulanacağı için test FAIL olur.
            var order = _orderService.PlaceOrder(_cart, 10m);

            Assert.That(order.TotalAmount, Is.EqualTo(expectedTotal), "10% indirim uygulanmalıydı.");
        }

        // TC-25: BVA (Boundary Valid - EXPECTED FAIL) - %1 indirim uygulanması
        [Test]
        public void PlaceOrder_Discount_Boundary_OnePercent_BVA()
        {
            _cart.AddItem(_product, 1); // 15000 TL
            decimal expectedTotal = 15000m * 0.99m; // 14850 TL

            // BUG #12: Sabit %50 uygulanacağı için test FAIL olur.
            var order = _orderService.PlaceOrder(_cart, 1m);

            Assert.That(order.TotalAmount, Is.EqualTo(expectedTotal), "1% indirim uygulanmalıydı.");
        }

        // TC-26: BVA (Boundary Valid - EXPECTED FAIL) - %100 indirim uygulanması
        [Test]
        public void PlaceOrder_Discount_Boundary_100Percent_BVA()
        {
            _cart.AddItem(_product, 1); // 15000 TL

            // BUG #12: Sabit %50 uygulanacağı için test FAIL olur (Bedava olması gerekirken 7500 TL çıkacaktır).
            var order = _orderService.PlaceOrder(_cart, 100m);

            Assert.That(order.TotalAmount, Is.EqualTo(0.00m), "100% indirim sepeti ücretsiz yapmalıydı.");
        }

        // TC-27: EP (Invalid) - Negatif indirim girilmesi
        [Test]
        public void PlaceOrder_Discount_Negative_Invalid_EP()
        {
            _cart.AddItem(_product, 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => _orderService.PlaceOrder(_cart, -5m),
                "Negatif indirim ArgumentOutOfRangeException fırlatmalı.");
        }
    }
}
