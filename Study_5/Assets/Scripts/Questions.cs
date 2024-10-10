using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Questions : MonoBehaviour
{
    public Slider Q1;
    public Slider Q2;
    public Slider Q3;
    public Slider Q4;
    public Slider Q5;
    public Slider Q6;
    public Slider Q7;
    public Slider Q8;
    public Slider Q9;
    public Slider Q10;
    public Slider Q11;
    public Slider Q12;

    public Save saveF;
    public N_Back nbackScript;
    
    public void saveResponses()
    {
        saveF.WrittingSaveQ(nbackScript.nBack, nbackScript.D_level, Q1.value, Q2.value, Q3.value, Q4.value, Q5.value, Q6.value, Q7.value, Q8.value, Q9.value, Q10.value, Q11.value, Q12.value);
        ResetSliderValue();
    }

    private void ResetSliderValue()
    {
        Q1.value = 5;
        Q2.value = 5;
        Q3.value = 5;
        Q4.value = 5;
        Q11.value = 5;
        Q5.value = 50;
        Q6.value = 50;
        Q7.value = 50;
        Q8.value = 50;
        Q9.value = 50;
        Q10.value = 50;
        Q11.value = 4;
    }
}
