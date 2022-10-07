using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void OnValueChanged(int currentValue, int maxValue)
    {
        _slider.value = (float)currentValue / maxValue;        
    }
}
