using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLikert : MonoBehaviour
{
    public Slider sliderStress;

    public void Change()
    {
        if (sliderStress.value ==1 )
        {
            GetComponent<TextMeshProUGUI>().text = "Not at all";
        }
        if (sliderStress.value == 2)
        {
            GetComponent<TextMeshProUGUI>().text = "Slightly";
        }
        if (sliderStress.value == 3)
        {
            GetComponent<TextMeshProUGUI>().text = "Moderately";
        }
        if (sliderStress.value == 4)
        {
            GetComponent<TextMeshProUGUI>().text = "Very much";
        }
        if (sliderStress.value == 5)
        {
            GetComponent<TextMeshProUGUI>().text = "Completely";
        }
    }

    public void ChangeSpeed()
    {
        if (sliderStress.value == 1)
        {
            GetComponent<TextMeshProUGUI>().text = "Too slow";
        }
        if (sliderStress.value == 2)
        {
            GetComponent<TextMeshProUGUI>().text = "Somewhat slow";
        }
        if (sliderStress.value == 3)
        {
            GetComponent<TextMeshProUGUI>().text = "Just fine";
        }
        if (sliderStress.value == 4)
        {
            GetComponent<TextMeshProUGUI>().text = "Somewhat fast";
        }
        if (sliderStress.value == 5)
        {
            GetComponent<TextMeshProUGUI>().text = "Too fast";
        }
    }
}
