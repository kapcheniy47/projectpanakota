public interface ICapitalManager
{
    float MaxValue { get; }
    float CurrentValue { get; }
    float MainValue { get; }
    float UpgradeCost { get; }
    void LoadResource();
    void SaveResource();
    void AccumulateResource();
    void TransferToMain();
    void ResetAllProgress();
    void Upgrade();
    void AddCoins(int amount);
}   