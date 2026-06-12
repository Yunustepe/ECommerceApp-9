using System;
using ECommerceApp.Core;

namespace ECommerceApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================");
            Console.WriteLine("E-Commerce App - Final Demo Shopping Session");
            Console.WriteLine("==================================================");

            // 1. Urunlerin Tanimlanmasi
            Console.WriteLine("\n1. Urunler Tanimlaniyor...");
            var keyboard = new Product(1, "Mekanik Klavye", 80.00m, 10);
            var mouse = new Product(2, "Kablosuz Mouse", 30.00m, 15);
            var headset = new Product(3, "Oyuncu Kulakligi", 120.00m, 0); // Stokta yok!

            Console.WriteLine($"   - {keyboard.Name}: {keyboard.Price:C} (Stok: {keyboard.Stock})");
            Console.WriteLine($"   - {mouse.Name}: {mouse.Price:C} (Stok: {mouse.Stock})");
            Console.WriteLine($"   - {headset.Name}: {headset.Price:C} (Stok: {headset.Stock})");

            // 2. Sepet Olusturulmasi ve Urun Ekleme
            Console.WriteLine("\n2. Sepet Olusturuluyor ve Urunler Ekleniyor...");
            var cart = new Cart();
            cart.AddItem(keyboard, 1);
            Console.WriteLine($"   + {keyboard.Name} eklendi. Sepet Toplami: {cart.GetTotal():C}");
            cart.AddItem(mouse, 2);
            Console.WriteLine($"   + 2 adet {mouse.Name} eklendi. Sepet Toplami: {cart.GetTotal():C}");
            
            // Midterm BUG #6 Gosterimi
            Console.WriteLine($"   [BILGI] GetItemCount() Sonucu: {cart.GetItemCount()} adet (Hatali: 2 benzersiz urun goruyor, toplam 3 adet yerine)");

            // 3. Siparis Olusturulmasi
            Console.WriteLine("\n3. Siparis Olusturuluyor...");
            var orderService = new OrderService();
            decimal discountPercentage = 10.00m; // %10 indirim uygulanmak isteniyor

            Console.WriteLine($"   - Gereken Minimum Siparis Tutari: {orderService.MinimumOrderAmount:C}");
            Console.WriteLine($"   - Sepet Toplami: {cart.GetTotal():C}");
            Console.WriteLine($"   - Talep Edilen Indirim: %{discountPercentage}");

            // Siparis veriliyor
            Order order = orderService.PlaceOrder(cart, discountPercentage);

            Console.WriteLine($"\nSiparis Olusturuldu!");
            Console.WriteLine($"   Siparis ID: {order.OrderId}");
            Console.WriteLine($"   Siparis Durumu: {order.Status}");
            Console.WriteLine($"   Odenmesi Gereken Nihai Tutar (Indirimli): {order.TotalAmount:C}");

            // 4. Kasitli Hatalarin Konsolda Gosterilmesi (Bug Manifestation)
            Console.WriteLine("\n4. Sistemdeki Kasitli Hatalarin (Bugs) Gosterimi...");

            // Bug A: Indirim Hatasi
            Console.WriteLine("\nHATA A: Indirim Hesaplama Hatasi (BUG #12)");
            Console.WriteLine("   Beklenen Tutar (%10 indirimle): 126.00 TL");
            Console.WriteLine($"   Hesaplanan Tutar: {order.TotalAmount:C} (Her zaman %50 uygulaniyor!)");

            // Bug B: Stok Kontrol Hatasi
            Console.WriteLine("\nHATA B: Stok Kontrolunun Bypass Edilmesi (BUG #11)");
            Console.WriteLine($"   '{headset.Name}' stogu su an: {headset.Stock}");
            
            var buggyCart = new Cart();
            buggyCart.AddItem(headset, 1);
            Console.WriteLine($"   Stogu 0 olan '{headset.Name}' urunden 1 adet siparis verilmeye calisiliyor...");
            
            try
            {
                var stockBugOrder = orderService.PlaceOrder(buggyCart, 0);
                Console.WriteLine($"   Siparis Basarili! Siparis ID: {stockBugOrder.OrderId}");
                Console.WriteLine($"   Siparis Durumu: {stockBugOrder.Status}");
                Console.WriteLine($"   Urunun Yeni Stok Miktari: {headset.Stock} (Negatife dustu!)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Siparis Basarisiz! Hata: {ex.Message}");
            }

            // Bug C: Minimum Siparis Limiti Hatasi
            Console.WriteLine("\nHATA C: Minimum Siparis Tutari Sinir Ihlali (BUG #13)");
            Console.WriteLine($"   Gereken Minimum Tutar: {orderService.MinimumOrderAmount:C}");
            
            var cheapCart = new Cart();
            var cheapItem = new Product(4, "Ucuz Kalem", 15.00m, 5);
            cheapCart.AddItem(cheapItem, 1);
            
            Console.WriteLine($"   15.00 TL tutarinda siparis verilmeye calisiliyor...");
            try
            {
                var cheapOrder = orderService.PlaceOrder(cheapCart, 0);
                Console.WriteLine($"   Siparis Basarili! Siparis ID: {cheapOrder.OrderId}, Tutar: {cheapOrder.TotalAmount:C}");
                Console.WriteLine("   (Bu siparis 50 TL altinda oldugu icin reddedilmeliydi ama onaylandi!)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Siparis Basarisiz! Hata: {ex.Message}");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("Demo Kosumu Tamamlandi. Lutfen testleri kontrol edin!");
            Console.WriteLine("==================================================");
        }
    }
}
