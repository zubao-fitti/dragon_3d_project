using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    public TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        SetHealthText();
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        
        SetHealthText();
    }

    void SetHealthText()
    {
        tmp.SetText("{0} / {1}", slider.value, slider.maxValue);
    }
}
