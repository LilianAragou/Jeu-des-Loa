using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    public Button plusButton;
    public Button minusButton;
    public TextMeshProUGUI minusButtonText;

    public int count = 3; // Valeur initiale

    void Start()
    {
        UpdateMinusButtonText();

        plusButton.onClick.AddListener(() =>
        {
            count++;
            UpdateMinusButtonText();
        });

        minusButton.onClick.AddListener(() =>
        {
            if (count > 0)
            {
                count--;
                UpdateMinusButtonText();
            }
        });
    }

    void UpdateMinusButtonText()
    {
        minusButtonText.text = $"{count}\n-";
    }
}
