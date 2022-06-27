using TMPro;
using UnityEngine;

public class UILayer : MonoBehaviour
{



    [SerializeField] private TextMeshProUGUI repUI;
    [SerializeField] private TextMeshProUGUI coinUI;
    [SerializeField] private Player player;
    

    public void ChangeMoney(int amount)
    {
        player.ChangeMoney(amount);
        UpdateUI();
    }
    
    //CHANGED -= TO +=, USE THIS INSTEAD
    public void ChangeReputation(int amount)
    {
        player.ChangeReputation(amount);
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        repUI.text = $"{player.Rep}";
        coinUI.text = $"{player.Money}";

    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

}
