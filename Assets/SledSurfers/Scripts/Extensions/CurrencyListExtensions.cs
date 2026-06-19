using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;

namespace SledSurfers.Scripts.Utils.Extensions
{
    public static class CurrencyListExtensions
    {
        public static int GetAmount(this List<CurrencyData> list, CurrencyType type)
        {
            var index = list.FindIndex(c => c.currencyType == type);
            return index >= 0 ? list[index].amount : 0;
        }

        public static void SetAmount(this List<CurrencyData> list, CurrencyType type, int amount)
        {
            var index = list.FindIndex(c => c.currencyType == type);

            if (index >= 0)
            {
                var existing = list[index];
                existing.amount = amount;
                list[index] = existing;
            }
            else
            {
                list.Add(new CurrencyData { currencyType = type, amount = amount });
            }
        }

        public static void Remove(this List<CurrencyData> list, CurrencyType type)
        {
            list.RemoveAll(c => c.currencyType == type);
        }
    }
}