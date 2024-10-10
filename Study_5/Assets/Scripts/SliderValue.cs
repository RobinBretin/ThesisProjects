using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider slider;

    public void ChangeLoudness()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value+" - Not Loud at all";
        }
        if (slider.value > 0 && slider.value <= 3)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Slightly Loud";
        }
        if (slider.value > 3 && slider.value <= 6)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Moderately Loud";
        }
        if (slider.value == 7 || slider.value == 8)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Very Loud";
        }
        if (slider.value == 9 ||slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Extremely Loud";
        }
    }

    public void ChangeAnnoyance()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Not Annoyed at all";
        }
        if (slider.value > 0 && slider.value <= 3)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Slightly Annoyed";
        }
        if (slider.value > 3 && slider.value <= 6)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Moderately Annoyed";
        }
        if (slider.value == 7 || slider.value == 8)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Very Annoyed";
        }
        if (slider.value == 9 || slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Extremely Annoyed";
        }
    }

    public void ChangeThreat()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Not Threatening at all";
        }
        if (slider.value > 0 && slider.value <= 3)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Slightly Threatening";
        }
        if (slider.value > 3 && slider.value <= 6)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Moderately Threatening";
        }
        if (slider.value == 7 || slider.value == 8)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Very Threatening";
        }
        if (slider.value == 9 || slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Extremely Threatening";
        }
    }

    public void ChangeAware()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Not Aware at all";
        }
        if (slider.value > 0 && slider.value <= 3)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Slightly Aware";
        }
        if (slider.value > 3 && slider.value <= 6)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Moderately Aware";
        }
        if (slider.value == 7 || slider.value == 8)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Very Aware";
        }
        if (slider.value == 9 || slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Extremely Aware";
        }
    }

    public void ChangeBother()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Not Bothered at all";
        }
        if (slider.value > 0 && slider.value <= 3)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Slightly Bothered";
        }
        if (slider.value > 3 && slider.value <= 6)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Moderately Bothered";
        }
        if (slider.value == 7 || slider.value == 8)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Very Bothered";
        }
        if (slider.value == 9 || slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Extremely Bothered";
        }
    }

    public void ChangeArousal()
    {
        if (slider.value == 0)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Minimum";
        }
        if (slider.value > 0 && slider.value < 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + "";
        }
        if (slider.value == 10)
        {
            GetComponent<TextMeshProUGUI>().text = slider.value + " - Maximum";
        }
    }
}
