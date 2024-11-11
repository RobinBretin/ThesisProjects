using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManagerTraining : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject[] dropLocations;

    public GameObject currentTarget;
    public GameObject currentDrop;
    public int i;
    public int nbMoves;
    public bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        currentTarget = objects[i];
        currentDrop = dropLocations[i];
        currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        grabbed = currentTarget.GetComponent<XRGrabInteractable>().isSelected;
        if (grabbed)
        {
            currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = false;
            currentDrop.SetActive(true);
            currentDrop.GetComponent<AnimatedHighlight>().isHighlighting = true;
        }
        else
        {
            if (currentDrop.GetComponent<DropDetection>().enteredObject == currentTarget)
            {

                if (i <= nbMoves-2)
                {
                    i++;
                }
                else
                {
                    //text.text = "You've completed the training. Well done. Go the the screen to answer some questions.";
                    GetComponent<UI_controls_training>().TrainingDrone();
                }
                currentDrop.SetActive(false);
                currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = false;
                currentTarget = objects[i];
                currentDrop = dropLocations[i];

            }

                currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = true;
            

        }




    }
}
