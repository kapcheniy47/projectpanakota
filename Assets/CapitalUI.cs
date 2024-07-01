using UnityEngine;
using UnityEngine.UI;

public class CapitalUI : MonoBehaviour
{
    public Slider resourceSlider;
    public Text resourceText;
    public Text mainValueText;
    public Button transferButton;
    public Button resetAllButton;
    public Button upgradeButton;
    public Text upgradeButtonText;
    public Button addCoinsButton;

    private ICapitalManager resourceManager;

    private void Start()
    {
        resourceManager = FindObjectOfType<CapitalManager>() as ICapitalManager;
        if (resourceManager != null)
        {
            resourceSlider.maxValue = resourceManager.MaxValue;
            transferButton.onClick.AddListener(OnTransferButtonClicked);
            resetAllButton.onClick.AddListener(OnResetAllButtonClicked);
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            UpdateUpgradeButtonText();
            addCoinsButton.onClick.AddListener(OnAddCoinsButtonClicked);
        }
    }

    private void Update()
    {
        resourceManager = FindObjectOfType<CapitalManager>() as ICapitalManager;
        if (resourceManager != null)
        {
            resourceSlider.value = resourceManager.CurrentValue;
            resourceText.text = $"{Mathf.Floor(resourceManager.CurrentValue)}/{Mathf.Floor(resourceManager.MaxValue)}";
            mainValueText.text = $"{Mathf.Floor(resourceManager.MainValue)}";
            resourceSlider.maxValue = resourceManager.MaxValue;
        }
    }

    private void OnTransferButtonClicked()
    {
        resourceManager?.TransferToMain();
        UpdateUpgradeButtonText();
    }

    private void OnResetAllButtonClicked()
    {
        resourceManager?.ResetAllProgress();
        UpdateUpgradeButtonText();
    }

    private void OnUpgradeButtonClicked()
    {
        resourceManager?.Upgrade();
        UpdateUpgradeButtonText();
    }

    private void UpdateUpgradeButtonText()
    {
        if (resourceManager != null)
        {
            int upgradeCost = Mathf.FloorToInt(resourceManager.UpgradeCost);
            upgradeButtonText.text = $"{upgradeCost}";
        }
    }
    private void OnAddCoinsButtonClicked()
    {
        resourceManager?.AddCoins(500);
        UpdateUpgradeButtonText();
    }
}