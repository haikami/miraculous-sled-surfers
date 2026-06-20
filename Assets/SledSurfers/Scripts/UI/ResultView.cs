using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace SledSurfers.Scripts.UI
{
    public class ResultView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private  TextMeshProUGUI _distance;
        [SerializeField] private GameObject _coinsContainer;
        [SerializeField] private  TextMeshProUGUI _coinsText;
        
        [SerializeField] private GameObject _gemsContainer;
        [SerializeField] private  TextMeshProUGUI _gemsText;

        private void OnEnable()
        {
            var resultData = ServiceLocator.Get<RunResultManager>().LastRunResultData;
            Setup(resultData);
        }

        private void Setup(RunResultData resultData)
        {
            _distance.text = $"Distance: {resultData.distanceTraveled}m";
            SetupCurrencies(resultData.currencies);
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
    }
}