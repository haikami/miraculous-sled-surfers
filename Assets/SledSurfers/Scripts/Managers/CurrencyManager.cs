using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Utils.Extensions;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class CurrencyManager
    {
        public event Action<CurrencyType, int> OnCurrencyChanged;

        private readonly DataManager _dataManager;

        public CurrencyManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void Reset(CurrencyType type)
        {
            var currencySource = GetCurrencySource(type);
            currencySource.Remove(type);
            OnCurrencyChanged?.Invoke(type, 0);
        }

        public void Add(Dictionary<CurrencyType, int> currencies)
        {
            foreach (var currency in currencies)
            {
                Add(currency.Key, currency.Value);
            }
        }

        public bool TrySpend(CurrencyType currencyType, int amount)
        {
            if (amount > GetAmount(currencyType))
            {
                return false;
            }

            Add(currencyType, -amount);
            return true;
        }

        public void Add(CurrencyData data) => Add(data.currencyType, data.amount);

        public void Add(CurrencyType currencyType, int amount)
        {
            var currencySource = GetCurrencySource(currencyType);
            var currentAmount = currencySource.GetAmount(currencyType);
            var newAmount = Mathf.Max(0, currentAmount + amount);
            currencySource.SetAmount(currencyType, newAmount);
            OnCurrencyChanged?.Invoke(currencyType, newAmount);
            Debug.Log($"Currency {currencyType} updated: {newAmount}");
        }

        public int GetAmount(CurrencyType type)
        {
            return GetCurrencySource(type).GetAmount(type);
        }

        public void ResetLevelCurrencies()
        {
            var removedCurrencies = _dataManager.PlayerData.levelCurrencies;
            foreach (var currency in removedCurrencies)
            {
                OnCurrencyChanged?.Invoke(currency.currencyType, 0);
            }

            _dataManager.PlayerData.levelCurrencies.Clear();
        }

        private List<CurrencyData> GetCurrencySource(CurrencyType type) => type switch
        {
            CurrencyType.Coins => _dataManager.PlayerData.levelCurrencies,
            CurrencyType.Gems => _dataManager.PlayerData.persistentCurrencies,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}