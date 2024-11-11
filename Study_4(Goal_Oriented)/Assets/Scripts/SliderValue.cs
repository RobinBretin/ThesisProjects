using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue : MonoBehaviour
{
    public Slider sliderStress;

    public void Change()
    {
        if (sliderStress.value < 20)
        {
            GetComponent<TextMeshProUGUI>().text =  sliderStress.value.ToString() + " (Very Light)";
        }
        if (sliderStress.value > 20 && sliderStress.value < 40)
        {
            GetComponent<TextMeshProUGUI>().text =  sliderStress.value.ToString() + " (Mild)";
        }
        if (sliderStress.value > 40 && sliderStress.value < 60)
        {
            GetComponent<TextMeshProUGUI>().text =  sliderStress.value.ToString() + " (Moderate)";
        }
        if (sliderStress.value > 60 && sliderStress.value < 80)
        {
            GetComponent<TextMeshProUGUI>().text =  sliderStress.value.ToString() + " (High)";
        }
        if (sliderStress.value > 80)
        {
            GetComponent<TextMeshProUGUI>().text = sliderStress.value.ToString() + " (Extreme)";
        }
    }
}
