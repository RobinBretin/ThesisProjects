using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class N_Back : MonoBehaviour
{
    public GameObject canvasTask;
    public GameObject canvasQuestions;
    public GameObject canvasGI;
    public GameObject canvasN1;
    public GameObject canvasN3;
    public GameObject canvasRest;
    public GameObject startButton;
    public GameObject continueButton;
    public GameObject endCanva;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI prev1;
    public TextMeshProUGUI prev2;
    public TextMeshProUGUI prev3;
    public TextMeshProUGUI current;

    public float time;

    public InputAction triggerLeft;
    public InputAction triggerRight;
    public Image feedback;

    //Measures
    public float rt;
    public List<float> scores; // 1 = true positive, 2 = true negative, 3 = false positive, 4 = false negative, 5 = omission
    public float TP;
    public float TN;
    public float FP;
    public float FN;
    public float Omission;
    public float Accuracy;

    //Visual Measures
    public TextMeshProUGUI rtText;
    public TextMeshProUGUI AccuracyTxt;
    public TextMeshProUGUI TPTxt;
    public TextMeshProUGUI TNTxt;
    public TextMeshProUGUI FPTxt;
    public TextMeshProUGUI FNTxt;
    public TextMeshProUGUI OTxt;

    //Parameters
    public int nBack;
    private bool guided;

    public bool answered;


    //N-back content
    private List<string[]> conditionOrder;
    private string[] letters;
    private string[] A_T1;
    private string[] A_T3;

    private string[] A_1;
    private string[] B_1;
    private string[] C_1;

    private string[] D_3;
    private string[] E_3;
    private string[] F_3;

    public string D_level;
    public string proxemic;

    public float displayTime;
    public float nextTime;
    public string N0;
    public string N1;
    public string N2;
    public string N3;

    //Manager
    public bool start;
    public Save saveF;

    public bool responded;

    private float timerS;
    public Image timerLineBG;
    public Image timerLine;
    private float timerSRest;
    public Image timerLineBGRest;
    public Image timerLineRest;
    public bool same;
    private int i;
    public int j;

    public bool resting;
    public float timerRest;
    public float timerRestMax;

    //ABFCED  BCADFE  CDBEAF  DECFBA  EFDACB  FAEBDC




    void Start()
    {
        
        ResetScores();
        SetScores();

        D_level = "none";
        proxemic = "fix";

        //Actions
        triggerLeft.Enable();
        triggerRight.Enable();
        responded = false;

        feedback.color = Color.grey;

        nBack = 1;
        guided = true;
        start = false;

        same = false;
        answered = false;
        resting = false;


        A_T1 = new string[] { "", "5", "1", "5", "5", "3", "2", "5", "5", "7", "9", "8", "8", "8", "7", "7", "9", "4", "1", "1", "7", "7", "4", "4", "8", "4", "4" };
        A_T3 = new string[] { "", "1", "8", "8", "3", "0", "8", "3", "7", "2", "3", "3", "2", "0", "7", "4", "2", "1", "7", "2", "7", "3", "0", "7", "5", "0", "0", "5", "4" };

        //A_T1 = new string[] { "", "2", "3", "6" };
        //A_T3 = new string[] { "", "7", "3", "9"};

        A_1 = new string[] { "", "3", "7", "8", "8", "0", "1", "9", "9", "6", "5", "3", "3", "8", "9", "4", "4", "0", "0", "0", "7", "2", "2", "9", "6", "6", "4", "", "5", "5", "2", "2", "8", "3", "9", "6", "4", "4", "5", "7", "7", "3", "3", "4" };
        B_1 = new string[] { "", "2", "2", "1", "8", "8", "3", "3", "7", "6", "5", "5", "8", "9", "1", "1", "0", "3", "3", "7", "5", "7", "7", "8", "4", "4", "7", "", "6", "6", "9", "2", "8", "1", "1", "7", "7", "1", "6", "9", "0", "4", "3", "8" };
        C_1 = new string[] { "", "4", "4", "9", "6", "6", "6", "5", "3", "9", "9", "3", "5", "5", "7", "1", "1", "2", "1", "6", "6", "7", "6", "9", "2", "2", "4", "", "3", "1", "4", "4", "4", "7", "1", "4", "0", "1", "1", "3", "7", "5", "7", "9" };

        D_3 = new string[] { "", "3", "6", "3", "2", "6", "7", "7", "2", "7", "7", "4", "1", "6", "4", "8", "6", "7", "8", "1", "6", "1", "6", "8", "5", "0", "8", "2", "0", "", "2", "7", "8", "0", "7", "6", "8", "4", "6", "6", "7", "4", "7", "2", "4", "3", "2", "3" };
        E_3 = new string[] { "", "2", "1", "4", "2", "8", "4", "1", "1", "0", "1", "4", "0", "7", "5", "0", "5", "6", "5", "1", "6", "6", "9", "2", "9", "9", "2", "2", "8", "", "0", "1", "6", "7", "1", "7", "7", "5", "6", "2", "5", "7", "2", "6", "1", "6", "6", "5" };
        F_3 = new string[] { "", "5", "3", "5", "8", "3", "2", "8", "2", "6", "3", "3", "9", "2", "3", "9", "1", "3", "5", "3", "3", "5", "8", "4", "9", "8", "1", "8", "5", "", "3", "3", "0", "4", "3", "5", "4", "9", "2", "9", "9", "4", "5", "7", "7", "4", "7", "7" };

        //"7","7","3","8","5","8","9","0","3","1","7","5","6","6","4","3","3","0","4","9","6","6","6","9","6","2","6","7","2","0","0","4","5","8","1","8","7","2"
        //"9","3","9","0","1","7","7","5","2","2","6","8","7","2","6","8","8","9","9","0","8","3","5","5","1","9","9","8","3","0","1","0","0","4","2","5","9","6"
        //"1","1","6","7","7","7","1","4","8","4","6","2","8","2","9","8","6","6","5","7","6","5","9","0","5","4","4","0","0","2","5","0","0","2","3","6","6","5"
        //"2","4","6","8","3","0","3","3","6","1","7","3","2","2","7","6","7","7","4","4","2","6","5","5","9","5","7","7","2","5","0","7","2","2","5","3","0","5"
        //"3","0","3","9","6","9","4","4","1","1","5","1","5","7","6","6","5","2","5","8","8","4","7","4","9","9","6","1","8","8","3","0","4","7","3","8","9","7"
        //"2","2","9","1","3","0","8","8","8","9","5","3","6","1","1","4","7","9","0","8","2","7","2","2","2","9","3","6","4","5","6","3","7","9","2","6","0","8"
        //"6","8","6","6","4","9","9","9","3","5","4","4","4","1","3","8","1","2","5","0","2","1","1","6","6","8","7","9","3","6","6","9","6","8","2","4","3"

        //"7","3","9","2","3","4","6","3","4","2","0","7","8","0","6","5","9","3","0","2","8","9","7","6","9","5","1","3","5","2"

        // 26 + 15 Digits, Target Proportion 30%
        //"", "3", "7", "8", "8", "0", "1", "9", "9", "6", "5", "3", "3", "8", "9", "4", "4", "0", "0", "0", "7", "2", "2", "9", "6", "6", "4", "1", "5", "5", "2", "2", "8", "3", "9", "6", "4", "4", "5", "7", "7"

        letters = A_T1;

        i = 0;
        j = 0;

        //Visual Timer
        timerS = 2 * canvasTask.GetComponent<RectTransform>().offsetMax.x + timerLineBG.GetComponent<RectTransform>().offsetMax.x - timerLineBG.GetComponent<RectTransform>().offsetMin.x;
        timerLine.rectTransform.offsetMax = new Vector2(-timerS, 0);

        timerSRest= 2 * canvasRest.GetComponent<RectTransform>().offsetMax.x + timerLineBGRest.GetComponent<RectTransform>().offsetMax.x - timerLineBGRest.GetComponent<RectTransform>().offsetMin.x;
        timerLineRest.rectTransform.offsetMax = new Vector2(-timerSRest, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (j > 3)
        {
            canvasGI.SetActive(false);
            if (((i == 27 && nBack==1)||(i == 29 && nBack == 3)) && !answered)
            {
                continueButton.SetActive(true);
                canvasQuestions.SetActive(true);
                start = false;
                answered = true;
            }
        }

        if (i > letters.Length - 1)
        {
            resting = true;
            j = j + 1;
            Start_Training();
            startButton.SetActive(true);
            i = 0;
        }

        if (resting && j > 3)
        {
            timerRest += Time.deltaTime;
            timerLineRest.rectTransform.offsetMax = new Vector2(-timerSRest + timerSRest * (timerRest / timerRestMax), 0);

            if (timerRest < timerRestMax)
            {
                startButton.GetComponent<Button>().interactable = false;
                canvasRest.SetActive(true);
            }
            else
            {
                startButton.GetComponent<Button>().interactable = true;
                resting = false;
                timerRest = 0;
            }
        }
        else resting = false;

        if (start)
        {
            time += Time.deltaTime;
            timerLine.rectTransform.offsetMax = new Vector2(-timerS + timerS * (time / nextTime), 0);
            if (i > 1 && nBack == 1)
            {
                if (triggerRight.triggered && !responded)
                {
                    rt = time;
                    rtText.text = "RT: " + System.Math.Round(rt, 2);
                    if (same)
                    {
                        if (j > 3) feedback.color = new Color(70,130,180); 
                        else feedback.color = Color.green;
                        saveF.WrittingSave(rt, 1, nBack, D_level,proxemic);
                        scores.Add(1);
                        TP += 1;
                        SetScores();

                    }
                    else
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.red;
                        saveF.WrittingSave(rt, 3, nBack, D_level, proxemic);
                        scores.Add(3);
                        FP += 1;
                        SetScores();
                    }
                    responded = true;
                }
                if (triggerLeft.triggered && !responded)
                {
                    rt = time;
                    rtText.text = "RT: " + System.Math.Round(rt, 2);
                    if (!same)
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.green;
                        saveF.WrittingSave(rt, 2, nBack, D_level, proxemic);
                        scores.Add(2);
                        TN += 1;
                        SetScores();
                    }
                    else
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.red;
                        saveF.WrittingSave(rt, 4, nBack, D_level, proxemic);
                        scores.Add(4);
                        FN += 1;
                        SetScores();
                    }
                    responded = true;
                }
            }

            if (i > 3 && nBack == 3)
            {
                if (triggerRight.triggered && !responded)
                {
                    rt = time;
                    rtText.text = "RT: " + System.Math.Round(rt, 2);
                    if (same)
                    {
                        if(j>3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.green;
                        saveF.WrittingSave(rt, 1, nBack, D_level, proxemic);
                        scores.Add(1);
                        TP += 1;
                        SetScores();

                    }
                    else
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.red;
                        saveF.WrittingSave(rt, 3, nBack, D_level, proxemic);
                        scores.Add(3);
                        FP += 1;
                        SetScores();
                    }
                    responded = true;
                }
                if (triggerLeft.triggered && !responded)
                {
                    rt = time;
                    rtText.text = "RT: " + System.Math.Round(rt, 2);
                    if (!same)
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.green;

                        saveF.WrittingSave(rt, 2, nBack, D_level, proxemic);
                        scores.Add(2);
                        TN += 1;
                        SetScores();
                    }
                    else
                    {
                        if (j > 3) feedback.color = new Color(70, 130, 180);
                        else feedback.color = Color.red;
                        saveF.WrittingSave(rt, 4, nBack, D_level, proxemic);
                        scores.Add(4);
                        FN += 1;
                        SetScores();
                    }
                    responded = true;
                }
            }

            if (time > nextTime)
            {
                if (!responded && ((nBack == 1 && i > 1) || (nBack == 3 && i > 3)))
                {
                    saveF.WrittingSave(rt, 5, nBack, D_level, proxemic);
                    scores.Add(5); //Add omission if not response above allowed time
                    Omission += 1;
                    SetScores();
                }

                N_Back_Change();
                time = 0;
            }

            if (time > displayTime)
            {
                current.text = "";
            }
        }
    }

    public void N_Back_Change()
    {
        //Reset
        feedback.color = Color.grey;
        responded = false;

        //Next
        i = i + 1;
        if (i <= letters.Length - 1)
        {
            N0 = letters[i];
            current.text = N0;
        }

        if (i > 0)
        {
            N1 = letters[i - 1];
            if (nBack == 1)
            {
                if (N0 == N1) same = true;
                else same = false;
                SetColorN1(prev1, N1);
            }
            else SetColorN(prev1, N1);

            if (i > 1)
            {
                N2 = letters[i - 2];
                SetColorN(prev2, N2);

                if (i > 2)
                {
                    N3 = letters[i - 3];
                    if (nBack == 3)
                    {
                        if (N0 == N3) same = true;
                        else same = false;
                        SetColorN1(prev3, N3);
                    }
                    else SetColorN(prev3, N3);
                }
            }
        }
    }

    public void SetScores()
    {
        //Set Scores
        if (TP + TN + FP + FN + Omission != 0)
        {
            Accuracy = Mathf.Round(((TP + TN) / (TP + TN + FP + FN + Omission)) * 100);
        }
        AccuracyTxt.text = "Accuracy: " + Accuracy + "%";
        TPTxt.text = "TP: " + TP;
        TNTxt.text = "TN: " + TN;
        FPTxt.text = "FP: " + FP;
        FNTxt.text = "FN: " + FN;
        OTxt.text = "O: " + Omission;
    }

    public void ResetScores()
    {
        //Init Scores
        TP = 0;
        TN = 0;
        FP = 0;
        FN = 0;
        Omission = 0;
        Accuracy = 100;
    }

    public void SetColorN(TextMeshProUGUI prev, string N)
    {
        if (guided)
        {
            prev.text = N;
            prev.color = Color.grey;
        }
    }
    public void SetColorN1(TextMeshProUGUI prev, string N)
    {
        if (guided)
        {
            prev.text = N;
            if (same)
            {
                prev.color = Color.green;
            }
            else prev.color = Color.red;
        }
    }

    public void SetColorFeedback()
    {
        if (j > 3) feedback.color = Color.blue;
    }

    public void Start_Training()
    {
        //Reset
        start = !start;
        time = 0;
        proxemic = "fix";

        prev1.text = "";
        prev2.text = "";
        prev3.text = "";
        current.text = "";

        N0 = "";
        N1 = "";
        N2 = "";
        N3 = "";

        responded = false;
        answered = false;
        feedback.color = Color.grey;

        if (j < 10) {
            letters = conditionOrder[j];
        }
        else
        {
            endCanva.SetActive(true);
        }

        if (j == 0 || j == 2) guided = true;
        else guided = false;

        if (letters == A_T1 || letters == A_1 || letters == B_1 || letters == C_1)
        {
            nBack = 1;
            if (guided) Title.text = "1-Back (Guided)";
            else Title.text = "1-Back";
            canvasN1.SetActive(true);
            canvasN3.SetActive(false);
        }
        else 
        {
            nBack = 3;
            if (guided) Title.text = "3-Back (Guided)";
            else Title.text = "3-Back";
            canvasN1.SetActive(false);
            canvasN3.SetActive(true);
        }

        if (letters == A_T1 || letters == A_T3) D_level = "none";
        if (letters == A_1 || letters == D_3) D_level = "Low";
        if (letters == B_1 || letters == E_3) D_level = "Medium";
        if (letters == C_1 || letters == F_3) D_level = "High";
    }

    public void Continue()
    {
        proxemic = "walk";
        start = true;
        time = 0;
        responded = false;
    }

    public void setOrder(float order)
    {
        if (order == 1)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, A_1, B_1, F_3, C_1, E_3, D_3 };
        }
        if (order == 2)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, B_1, C_1, A_1, D_3, F_3, E_3 };
        }
        if (order == 3)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, C_1, D_3, B_1, E_3, A_1, F_3 };
        }
        if (order == 4)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, D_3, E_3, C_1, F_3, B_1, A_1 };
        }
        if (order == 5)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, E_3, F_3, D_3, A_1, C_1, B_1 };
        }
        if (order == 6)
        {
            conditionOrder = new List<string[]> { A_T1, A_T1, A_T3, A_T3, F_3, A_1, E_3, B_1, D_3, C_1 };
        }
    }
}
