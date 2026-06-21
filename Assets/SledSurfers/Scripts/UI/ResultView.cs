using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace SledSurfers.Scripts.UI
{
    public class ResultView : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TextMeshProUGUI _headerText;
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
            bool isVictory = resultData.reason == FinishReason.ReachedEnd;
            _distance.text = $"Distance: {resultData.distanceTraveled}m";
            _distance.gameObject.SetActive(!isVictory);
            _headerText.text = isVictory ? "VICTORY!" : "RESULTS";
            SetupCurrencies(resultData.currencies);
        }

        private void SetupCurrencies(Dictionary<CurrencyType, int> currencies)
        {
            var numGems = currencies?.GetValueOrDefault(CurrencyType.Gems) ?? 0;
            var numCoins = currencies?.GetValueOrDefault(CurrencyType.Coins) ?? 0;

            _gemsText.text = numGems.ToString();
            _gemsContainer.SetActive(numGems > 0);
            
            _coinsText.text = numCoins.ToString();
            _coinsContainer.SetActive(numCoins > 0);
        }
    }
}