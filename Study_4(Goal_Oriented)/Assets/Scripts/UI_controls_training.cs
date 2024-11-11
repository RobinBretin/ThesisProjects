using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_controls_training : MonoBehaviour
{
    public GameObject DirectController;
    public GameObject UIController;
    public GameObject Instructions;
    public GameObject Questions;
    public Save_training save;
    public Slider Q1;
    public Slider Q2;
    public int SceneIndex;
    // Start is called before the first frame update
    public void OnClickStart()
    {
        DirectController.SetActive(true);
        UIController.SetActive(false);
        Instructions.SetActive(false);

    }

    public void TrainingDrone()
    {
        Questions.SetActive(true);
        DirectController.SetActive(false);
        UIController.SetActive(true);
    }

    public void OnClickContinue()
    {
        save.WrittingSaveQ(Q1.value, Q2.value);
        SceneManager.LoadScene(SceneIndex, LoadSceneMode.Single);

    }

}
