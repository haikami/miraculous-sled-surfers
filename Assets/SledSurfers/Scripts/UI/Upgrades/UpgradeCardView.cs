using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Managers;
using SledSurfers.Scripts.Meta.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SledSurfers.Scripts.UI.Upgrades
{
    public class UpgradeCardView : MonoBehaviour
    {
        [SerializeField] private List<Image> _coloredElements;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _upgradeableContent;
        [SerializeField] private GameObject _maxUpgradeContent;
        [SerializeField] private Button _upgradeButton;
        
        private UpgradeType _upgradeType;
        private UpgradesManager _upgradesManager;
        private CurrencyManager _currencyManager;
        private bool _isSubscribedToEvents = false;
        private bool IsSetup => _upgradesManager != null && _currencyManager != null;

        public void Setup(UpgradeType upgradeType, UpgradesManager upgradesManager, CurrencyManager currencyManager)
        {
            gameObject.SetActive(true);
            _upgradeType = upgradeType;
            _upgradesManager = upgradesManager;
            _currencyManager = currencyManager;
            SetupPersistentData();
            RefreshContent();
            TrySubscribe();
        }
        
        private void RefreshContent()
        {
            SetupCurrentValue();
            SetupUpgradeableContent();
        }
        
        private void OnCurrencyChanged(CurrencyType currencyType, int currencyAmount) => SetupUpgradeableContent();

        public void OnUpgradeButtonClicked()
        {
            if (_upgradesManager.TryUpgrade(_upgradeType))
            {
                SetupCurrentValue();
                return;
            }
            
            Debug.LogWarning("Couldn't upgrade " + _upgradeType);
            SetupUpgradeableContent();
        }

        private void SetupPersistentData()
        {
            var upgradeConfig = _upgradesManager.GetUpgradeConfig(_upgradeType);
            _title.text = upgradeConfig.Name;
            _icon.sprite = upgradeConfig.Icon;
            foreach (var coloredElement in _coloredElements)
            {
                coloredElement.color = upgradeConfig.Color;
            }
        }

        private void SetupCurrentValue()
        {
            _value.text = _upgradesManager.GetUpgradeDisplayValue(_upgradeType);
        }

        private void SetupUpgradeableContent()
        {
            var upgradeCostInfo = _upgradesManager.GetUpgradeCostInfo(_upgradeType);
            _upgradeButton.interactable = upgradeCostInfo is { canAfford: true, isMaxLevel: false };
            _level.text = $"Lvl. {upgradeCostInfo.currentLevel + 1}";
            _maxUpgradeContent.SetActive(upgradeCostInfo.isMaxLevel);
            _upgradeableContent.SetActive(!upgradeCostInfo.isMaxLevel);
            _cost.text = $"{upgradeCostInfo.upgradeCost} {upgradeCostInfo.currencyType}";
        }
        
        private void OnEnable()
        {
            TrySubscribe();
            
            if (IsSetup)
            {
                RefreshContent();
            }
        }
        
        private void OnDisable() => TryUnsubscribe();

        private void TrySubscribe()
        {
            if (_isSubscribedToEvents || !IsSetup) return;

            _currencyManager.OnCurrencyChanged += OnCurrencyChanged;
            _upgradesManager.OnUpgradesReset += RefreshContent;
            _isSubscribedToEvents = true;
        }

        private void TryUnsubscribe()
        {
            if (!_isSubscribedToEvents || !IsSetup) return;
            
            _currencyManager.OnCurrencyChanged -= OnCurrencyChanged;
            _upgradesManager.OnUpgradesReset -= RefreshContent;
            _isSubscribedToEvents = false;
        }
    }
}