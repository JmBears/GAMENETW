using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider enemySlider;

    private void Awake()
    {
        enemySlider = GetComponent<Slider>();
    }

    public void UpdateEnemyHealthBar(float currentValue, float maxValue)
    {
        enemySlider.value = currentValue / maxValue;
    }

    void Update()
    {
        
    }
}
