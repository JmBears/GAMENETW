using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticHealthBar : MonoBehaviour
{
    [SerializeField] private Slider planetSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    public void UpdatePlanetHealthBar(float currentValue, float maxValue)
    {
        planetSlider.value = currentValue / maxValue;
        healthText.text = currentValue + "/" + maxValue;
    }
}
