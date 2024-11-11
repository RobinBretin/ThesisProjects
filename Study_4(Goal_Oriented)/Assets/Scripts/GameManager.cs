using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;

    [SerializeField]
    private GameObject[] dropLocations;

    public GameObject currentTarget;
    public GameObject currentDrop;
    public int i;
    public int nbMoves;
    public bool grabbed;
    public DroneController DC;
    public bool P_dropped;
    public int NB_dropped;

    // Start is called before the first frame update
    void Start()
    {
        NB_dropped = 0;

        i = 0;
        currentTarget = objects[i];
        currentDrop = dropLocations[i];
        currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = true;
        P_dropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        P_dropped = false;
        grabbed = currentTarget.GetComponent<XRGrabInteractable>().isSelected;
        if (grabbed)
        {
            currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = false;
            if (DC.grabbed)
            {
                currentDrop.SetActive(true);
                currentDrop.GetComponent<AnimatedHighlight>().isHighlighting = true;
            }
        }
        else
        {
            if (currentDrop.GetComponent<DropDetection>().enteredObject == currentTarget)
            {
                P_dropped = true;
                NB_dropped ++;
                if (i == nbMoves - 1)
                {
                    GetComponent<UI_controls>().TrainingDrone();
                }
                else
                {
                    i++;
                    currentDrop.SetActive(false);
                    currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = false;
                    currentTarget = objects[i];
                    currentDrop = dropLocations[i];
                }
                
            }
            if (DC.dropReached)
            {
                currentTarget.GetComponent<AnimatedHighlight>().isHighlighting = true;
            }
            
            
            
        }

    }
}
