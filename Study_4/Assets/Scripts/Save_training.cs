using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.G2OM;
using Tobii.XR;
using System.IO;

public class Save_training : MonoBehaviour
{
    [Header("--------------")]
    [Header("PARAMETERS")]
    [Header("--------------")]
    [SerializeField]
    private int id_participant;
    [SerializeField]
    private float freq; //Sample frequency, save every freq second

    //SAVE PARAMETERS
    private float timer;
    private float time;
    private StreamWriter sw;

    [SerializeField]
    private string defaultPath;
    [SerializeField]
    private string fileName;

    //EYE_TRACKING
    private GameObject ET_focussedObject; //Current object being watched if any
    private string ET_name; //Name of the object 


    //REQUIREMENTS
    [SerializeField]
    private GameObject participant;

    //QUESTIONS
    private StreamWriter swQ; //Saved in UI_controls script

    //DATA
    //condition
    [SerializeField]
    private string Condition;

    //game
    private string[] states = new string[] { "Search", "Carry" }; //Searching object - Carry object to drop locations

    //participant
    private Vector3 P_Position; //3D position
    private Vector3 P_Rotation; //3D rotation
    private string P_state; //Searching object - Carry object to drop locations
    private GameObject P_ObjectTarget; //Name of the carried object if any
    private Vector3 P_targetPosition; //3D position of the current target (object or drop location)


    // Start is called before the first frame update
    void Start()
    {

        //Initialize timer (for sampling frequency) and time for real time
        time = 0;
        timer = 0;

        //Default path for save data
        defaultPath = Application.persistentDataPath;
        //Select a unique id for the participant
        while (File.Exists(defaultPath + id_participant + ".txt"))
        {
            id_participant++;
        }

        //Create and name the file after this id
        fileName = id_participant.ToString();
        sw = File.CreateText(defaultPath + fileName + "_" + Condition + ".txt");
        swQ = File.CreateText(defaultPath + fileName + "_" + Condition + "_Q.txt");

        //Initialize Data
        ET_name = "none";
        P_Position = participant.transform.position;
        P_Rotation = participant.transform.rotation.eulerAngles;
        P_state = states[0];
        P_ObjectTarget = GetComponent<GameManagerTraining>().currentTarget;
        P_targetPosition = P_ObjectTarget.transform.position;
       


        //Write headers + initial conditions in the save file
        sw.WriteLine("id,TimeStamp,focussedObject, P_x, P_y, P_z, P_ox, P_oy, P_oz, P_state, P_objectTarget, P_targetX, P_targetY, P_targetZ, Condition");
        sw.Flush();

        swQ.WriteLine("id,Condition,Q1,Q2");
        swQ.Flush();

        WrittingSave();

    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        time += Time.deltaTime;

        if (timer >= freq)
        {

            // Check whether TobiiXR has any focused objects.
            if (TobiiXR.FocusedObjects.Count > 0)
            {
                ET_focussedObject = TobiiXR.FocusedObjects[0].GameObject;
                ET_name = ET_focussedObject.name;
                // Do something with the focused game object
            }
            else ET_name = "none";

            //Update Variables when saving (and not every frame)
            P_Position = participant.transform.position;
            P_Rotation = participant.transform.rotation.eulerAngles;
            P_ObjectTarget = GetComponent<GameManagerTraining>().currentTarget;

            if (GetComponent<GameManagerTraining>().grabbed)
            {
                P_state = states[1];
                P_targetPosition = GetComponent<GameManagerTraining>().currentDrop.transform.position;

            }
            else
            {
                P_targetPosition = P_ObjectTarget.transform.position;
                P_state = states[0];
            }

            //Write the line
            WrittingSave();
            timer = 0;
        }
    }

    private void OnDestroy()
    {
        // Make sure to close the file before terminating the application
        swQ.Flush();
        sw.Flush();
        swQ.Close();
        sw.Close();
    }

    private void WrittingSave()
    {
        sw.WriteLine(id_participant + "," + time + "," + ET_name + "," + P_Position.x + "," + P_Position.y + "," + P_Position.z + "," + P_Rotation.x + "," + P_Rotation.y + "," + P_Rotation.z + "," + P_state + "," + P_ObjectTarget.name + "," + P_targetPosition.x + "," + P_targetPosition.y + "," + P_targetPosition.z + "," + Condition);
        sw.Flush();
    }

    public void WrittingSaveQ(float Q1, float Q2)
    {
        swQ.WriteLine(id_participant + "," + Condition + "," + Q1 + "," + Q2);
        swQ.Flush();
    }


}
