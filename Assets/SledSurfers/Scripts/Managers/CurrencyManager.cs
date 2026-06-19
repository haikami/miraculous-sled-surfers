using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
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
            var currentAmount = currencySource.GetValueOrDefault(currencyType);
            currencySource[currencyType] = Mathf.Max(0, currentAmount + amount);
            OnCurrencyChanged?.Invoke(currencyType, currencySource[currencyType]);
            Debug.Log($"Currency  {currencyType} upodated: {currencySource[currencyType]}");
        }

        public int GetAmount(CurrencyType type)
        {
            return GetCurrencySource(type).GetValueOrDefault(type);
        }

        public void ResetLevelCurrencies()
        {
            var removedCurrencies = _dataManager.PlayerData.LevelCurrencies.Keys;
            foreach (var currency in removedCurrencies)
            {
                OnCurrencyChanged?.Invoke(currency, 0); 
            }
            
            _dataManager.PlayerData.LevelCurrencies.Clear();
        }

        private Dictionary<CurrencyType, int> GetCurrencySource(CurrencyType type) => type switch
        {
            CurrencyType.Coins => _dataManager.PlayerData.LevelCurrencies,
            CurrencyType.Gems => _dataManager.PlayerData.PersistentCurrencies,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}