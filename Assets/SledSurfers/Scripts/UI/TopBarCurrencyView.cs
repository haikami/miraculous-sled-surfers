using SledSurfers.Scripts.Data.Models;
using TMPro;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.UI
{
    public class TopBarCurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private TextMeshProUGUI _text;
        
        public CurrencyType CurrencyType => _currencyType;
        
        public void SetAmount(int amount)
        {
            _text.text = amount.ToString();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}