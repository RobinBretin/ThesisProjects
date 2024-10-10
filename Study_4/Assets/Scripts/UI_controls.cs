using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_controls : MonoBehaviour
{
    public GameObject DirectController;
    public GameObject UIController;
    public GameObject Instructions;
    public GameObject Questions;
    public GameObject endScreen;
    public Save save;
    public Slider Q1;
    public Slider Q2;
    public Slider Q3;
    public Slider Q4;
    public Slider Q5;
    public int SceneIndex;
    public DroneController dc;
    // Start is called before the first frame update

    void Start()
    {
        DirectController = GameObject.Find("XR Origin/Camera Offset/RightHand Controller");
        UIController = GameObject.Find("XR Origin/Camera Offset/RightHand Controller UI");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Instructions.SetActive(false);
        }
        
    }


    public void OnClickStart()
    {
        save.started = "Yes";
        dc.start = true;
        Instructions.SetActive(false);
        UIController.SetActive(false);
        DirectController.SetActive(true);    
    }
    
    public void TrainingDrone()
    {
        save.started = "Done";
        dc.start = false;
        Questions.SetActive(true);
        DirectController.SetActive(false);
        UIController.SetActive(true);
    }

    public void OnClickContinue()
    {
        save.WrittingSaveQ(Q1.value, Q2.value, Q3.value, Q4.value, Q5.value);
        Questions.SetActive(false);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex+1 <= 2)
        {
            SceneManager.LoadScene(sceneIndex + 1, LoadSceneMode.Single);
        }
        else
        {
            
            endScreen.SetActive(true);
        }
        

    }

}
