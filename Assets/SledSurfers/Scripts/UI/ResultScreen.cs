using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
using TMPro;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.UI
{
    public class ResultScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private  TextMeshProUGUI _distance;
        [SerializeField] private GameObject _coinsContainer;
        [SerializeField] private  TextMeshProUGUI _coinsText;
        
        [SerializeField] private GameObject _gemsContainer;
        [SerializeField] private  TextMeshProUGUI _gemsText;
        
        private Action _onClosed;
        
        public void Show(RunResultData resultData, Action onClose)
        {
            _onClosed = onClose;
            
            SetupCurrencies(resultData.currencies);
            
            gameObject.SetActive(true);
        }

        private void SetupCurrencies(Dictionary<CurrencyType, int> currencies)
        {
            var numGems = currencies.GetValueOrDefault(CurrencyType.Gems);
            var numCoins = currencies.GetValueOrDefault(CurrencyType.Coins);

            _gemsText.text = $"Gems: {numGems}";
            _gemsContainer.SetActive(numGems > 0);
            
            _coinsText.text = $"Coins: {numCoins}";
            _coinsContainer.SetActive(numCoins > 0);
            
        }
        
        public void Close()
        {
            _onClosed?.Invoke();
            _onClosed = null;
            gameObject.SetActive(false);
        }
    }
}