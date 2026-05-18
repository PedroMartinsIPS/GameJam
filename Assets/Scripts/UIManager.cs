using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI treasureText;

    public void UpdateTreasure(int amount)
    {
        treasureText.text = "Treasures: " + amount;
    }
}