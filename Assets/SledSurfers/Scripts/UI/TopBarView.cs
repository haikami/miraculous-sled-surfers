using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Gameplay.UI;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.UI
{
    public class TopBarView : MonoBehaviour
    {
        [SerializeField] private List<TopBarCurrencyView> _currencyViews;

        private void Awake()
        {
            var manager = ServiceLocator.Get<CurrencyManager>();
            manager.OnCurrencyChanged += UpdateCurrency;
            foreach (var currencyView in _currencyViews)
            {
                currencyView.SetAmount(manager.GetAmount(currencyView.CurrencyType));
            }
        }

        private void UpdateCurrency(CurrencyType type, int amount)
        {
            GetCurrencyView(type)?.SetAmount(amount);
        }

        public void ShowCurrency(CurrencyType type) => GetCurrencyView(type)?.Show();

        public void SetCurrenciesDisplayed(List<CurrencyType> currencies)
        {
            foreach (var currency in _currencyViews)
            {
                if (currencies.Contains(currency.CurrencyType))
                {
                    currency.Show();
                }
                else
                {
                    currency.Hide();
                }
            }
        }
        
        public void HideAllCurrencies() => _currencyViews.ForEach(c => c.Hide());
        
        private TopBarCurrencyView GetCurrencyView(CurrencyType type) => _currencyViews.Find(view => view.CurrencyType == type);

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet(out CurrencyManager manager))
            {
                manager.OnCurrencyChanged -= UpdateCurrency;
            }
        }
    }
}