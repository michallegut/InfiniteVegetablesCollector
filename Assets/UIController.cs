using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Text collectedVegetablesText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void updateCollectedVegetablesText(int collectedVegetables)
    {
        collectedVegetablesText.text = "Collected vegetables: " + collectedVegetables.ToString();
    }
}