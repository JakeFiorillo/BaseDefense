using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateUI(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodText.text = $"Wood: {amount}";
                break;
            case ResourceType.Stone:
                stoneText.text = $"Stone: {amount}";
                break;
            case ResourceType.Gold:
                goldText.text = $"Gold: {amount}";
                break;
        }
    }
}
