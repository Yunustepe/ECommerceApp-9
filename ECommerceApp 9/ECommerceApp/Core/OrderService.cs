using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp.Core
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order(int orderId, List<CartItem> items, decimal totalAmount)
        {
            OrderId = orderId;
            Items = items;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
        }
    }

    public class OrderService
    {
        private List<Order> _orders = new List<Order>();
        private int _nextOrderId = 1;

        public decimal MinimumOrderAmount { get; } = 50.00m;

        // BUG #7: Sepet boşsa sipariş oluşturulabiliyor (midterm bug)
        // BUG #11 (Stok Kontrolü Hatası): Ürün stoğu tam 0 ise siparişi onaylar.
        // BUG #12 (İndirim Hesaplama Hatası): Herhangi bir indirim istendiğinde sabit %50 indirim uygular.
        // BUG #13 (Minimum Sipariş Hatası): Minimum limit 50 TL olmasına rağmen sadece 10 TL altındaki siparişleri engeller.
        public Order PlaceOrder(Cart cart, decimal discountPercentage = 0)
        {
            // Boş sepet kontrolü yok! (BUG #7 korunuyor)
            var items = cart.Items.ToList();
            decimal total = cart.GetTotal();

            // BUG #13: Minimum sipariş tutarı kontrol hatası
            // Limit 50 TL olmalı, ancak kodda sadece 10 TL altı engelleniyor.
            if (total < 10.00m)
            {
                throw new InvalidOperationException("Minimum order amount (50.00 TL) not met.");
            }

            // BUG #11: Stok kontrol hatası
            // Ürün stoğu 0 olduğunda stock != 0 kontrolü yüzünden stok kontrolü bypass edilir!
            foreach (var item in items)
            {
                if (item.Quantity > item.Product.Stock && item.Product.Stock != 0)
                {
                    throw new InvalidOperationException($"Insufficient stock for product '{item.Product.Name}'.");
                }
            }

            // BUG #12: İndirim oranı kontrol hatası
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discountPercentage), "Discount percentage must be between 0 and 100.");
            }
            if (discountPercentage > 0)
            {
                total = total * 0.50m; // Parametreden bağımsız her zaman %50 indirim!
            }

            var order = new Order(_nextOrderId++, items, total);
            _orders.Add(order);

            // Stok azaltma işlemi (midterm TC-17 gereksinimi için)
            foreach (var item in items)
            {
                item.Product.DecreaseStock(item.Quantity);
            }

            cart.Clear();
            return order;
        }

        // BUG #8: Ödeme tutarı sipariş tutarıyla eşleşmeden ödeme onaylanıyor (midterm bug)
        public bool ProcessPayment(Order order, decimal paymentAmount)
        {
            if (paymentAmount <= 0)
                return false;

            // paymentAmount == order.TotalAmount kontrolü yapılmıyor!
            order.Status = OrderStatus.Confirmed;
            return true;
        }

        public Order? GetOrder(int orderId)
        {
            return _orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        // BUG #9: İptal edilmiş siparişler de iptal edilebilir (durum kontrolü yok - midterm bug)
        public bool CancelOrder(int orderId)
        {
            var order = GetOrder(orderId);
            if (order == null) return false;

            order.Status = OrderStatus.Cancelled;
            return true;
        }

        // BUG #10: Kargo durumuna geçiş için Confirmed kontrolü yapılmıyor (midterm bug)
        public bool ShipOrder(int orderId)
        {
            var order = GetOrder(orderId);
            if (order == null) return false;

            order.Status = OrderStatus.Shipped;
            return true; // Pending durumundaki sipariş de kargoya verilebilir!
        }
    }
}
