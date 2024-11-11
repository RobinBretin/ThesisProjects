using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SAM : MonoBehaviour
{

    public int valence;
    public int arousal;
    public int dominance;
    public Button buttonV;
    public Button buttonA;
    public Button buttonD;
    public Button next;
    public bool SAManswered;

    private Color validate = new Color(56f / 255f, 214f / 255f, 84f / 255f, 255f / 255f);
    private Color unselected = new Color(224f / 255f, 224f / 255f, 224f / 255f, 1);
    // Start is called before the first frame update


    public void currentBut(Button clickedButton)
    {
        if(clickedButton.tag == "Valence")
        {
            if (buttonV == null)
            {
                buttonV = clickedButton;
                buttonV.image.color = validate;
            }
            else
            {
                if(buttonV != clickedButton)
                {
                    buttonV.image.color = unselected;
                    buttonV = clickedButton;
                    buttonV.image.color = validate;
                }
            }
        }

        if (clickedButton.tag == "Arousal")
        {
            if (buttonA == null)
            {
                buttonA = clickedButton;
                buttonA.image.color = validate;
            }
            else
            {
                if (buttonA != clickedButton)
                {
                    buttonA.image.color = unselected;
                    buttonA = clickedButton;
                    buttonA.image.color = validate;
                }
            }
        }

        if (clickedButton.tag == "Dominance")
        {
            if (buttonD == null)
            {
                buttonD = clickedButton;
                buttonD.image.color = validate;
            }
            else
            {
                if (buttonD != clickedButton)
                {
                    buttonD.image.color = unselected;
                    buttonD = clickedButton;
                    buttonD.image.color = validate;
                }
            }
        }
    }

    public void onSelectedVal(int value)
    {

        valence = value;

    }
    public void onSelectedArou(int value)
    {

        arousal = value;

    }
    public void onSelectedDomi(int value)
    {

        dominance = value;

    }

    public void Reset()
    {
        buttonA.image.color = unselected;
        buttonD.image.color = unselected;
        buttonV.image.color = unselected;
        buttonA = null;
        buttonD = null;
        buttonV = null;
    }

    void Update()
    {
        if (buttonV != null && buttonA != null && buttonD != null)
        {
            next.GetComponent<Button>().interactable = true;
        }
        else next.GetComponent<Button>().interactable = false;
    }
}
