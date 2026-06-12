using System;

namespace ECommerceApp.Core
{
    public enum DiscountType
    {
        None,
        Percentage,
        FixedAmount
    }

    /// <summary>
    /// İndirim türünü ve değerini tutan değer nesnesidir.
    /// </summary>
    public sealed class Discount
    {
        private Discount(DiscountType type, decimal value)
        {
            Type = type;
            Value = value;
        }

        public DiscountType Type { get; }
        public decimal Value { get; }

        public static Discount None() => new Discount(DiscountType.None, 0m);

        public static Discount Percentage(decimal rate)
        {
            if (rate < 0m || rate > 100m)
                throw new ArgumentOutOfRangeException(nameof(rate), "Yüzde indirim oranı 0 ile 100 arasında olmalıdır.");

            return new Discount(DiscountType.Percentage, rate);
        }

        public static Discount FixedAmount(decimal amount)
        {
            if (amount < 0m)
                throw new ArgumentOutOfRangeException(nameof(amount), "Sabit indirim tutarı negatif olamaz.");

            return new Discount(DiscountType.FixedAmount, amount);
        }
    }
}
