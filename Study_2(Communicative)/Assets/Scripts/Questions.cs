using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions : MonoBehaviour
{

    public GameObject[] questions;

    public int S_Q1;
    public int S_Q2;
    public int S_Q3;
    public int S_Q4;
    public int S_Q5;
    public int S_Q6;
    public int S_Q7;
    public int S_Q8;
    public int S_SAM_valence;
    public int S_SAM_arousal;
    public int S_SAM_dominance;
    //...

    public GameManager GM;
    public Display display;
    public EyeIdle eyeidle;
    public Code code;
    public Save save;
    //public GameObject displayG;
    public GameObject Instruction;
    public int[][] scores = new int[10][];

    private int i;
    private int condition;
    // Start is called before the first frame update
    void Start()
    {

        for (int j = 0; j < 10; j++)
        {
            scores[j] = new int[questions.Length+2];
        }

        condition = 0;
    }

    public void Q1(int value)
    {
        S_Q1 = value;
    }

    public void Q2(int value)
    {
        S_Q2 = value;
    }

    public void Q3(int value)
    {
        S_Q3 = value;
    }

    public void Q4(int value)
    {
        S_Q4 = value;
    }

    public void Q5(int value)
    {
        S_Q5 = value;
    }

    public void Q6(int value)
    {
        S_Q6 = value;
    }

    public void Q7(int value)
    {
        S_Q7 = value;
    }

    public void Q8(int value)
    {
        S_Q8 = value;
    }

    public void Valence(int value)
    {

        S_SAM_valence = value;

    }
    public void Arousal(int value)
    {

        S_SAM_arousal = value;

    }
    public void Dominance(int value)
    {

        S_SAM_dominance = value;

    }

    public void onNext()
    {
        questions[i].SetActive(false);
        if (i+1 >= questions.Length)
        {
            /*scores[condition][0] = S_SAM_arousal;
            scores[condition][1] = S_SAM_valence;
            scores[condition][2] = S_SAM_dominance;
            scores[condition][3] = S_Q1;
            scores[condition][4] = S_Q2;*/
            //...

            save.WrittingSaveQ(S_Q1,S_Q2,S_Q3, S_Q4, S_Q5, S_Q6, S_Q7, S_Q8, S_SAM_arousal,S_SAM_valence,S_SAM_dominance);

            i = 0;
            condition+=1;
            Instruction.SetActive(true);
            display.onSelect();
            code.OnOffQ();
            eyeidle.next = true;
        }
        else
        {
            i+=1;
            questions[i].SetActive(true);
        }
    }
}
