using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.G2OM;
using Tobii.XR;
using System.IO;

public class Save : MonoBehaviour
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
    public string started;

    [SerializeField]
    private string defaultPath;
    [SerializeField]
    private string fileName;

    //EYE_TRACKING
    private GameObject ET_focussedObject; //Current object being watched if any
    private string ET_name; //Name of the object 


    //REQUIREMENTS
    public GameObject participant;
    [SerializeField]
    private GameObject drone;

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
    private float P_relativeOrientation; //Angle difference between where the participant is facing and where the drone is
    private string P_state; //Searching object - Carry object to drop locations
    private GameObject P_ObjectTarget; //Name of the carried object if any
    private Vector3 P_targetPosition; //3D position of the current target (object or drop location)
    
    //drone
    private Vector3 D_Position; 
    private Vector3 D_Rotation; 
    private float D_relativeOrientation; 
    private string D_state; 
    private GameObject D_ObjectTarget;
    private Vector3 D_targetPosition;
    private string D_dangerLevel;

    //distances
    private float distance3D; //3D distance between drone position and headset position
    private float distance2D; //2D distance on the horizontal plan

    void Awake()
    {
        participant = GameObject.Find("XR Origin/Camera Offset/Main Camera");
    }
    // Start is called before the first frame update
    void Start()
    {
        started = "No";
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
        sw = File.CreateText(defaultPath + fileName + "_"+ Condition+ ".txt");
        swQ = File.CreateText(defaultPath + fileName +"_"+Condition+ "_Q.txt");

        //Initialize Data
        ET_name = "none";
        P_Position = participant.transform.position;
        P_Rotation = participant.transform.rotation.eulerAngles;
        P_relativeOrientation = get_relativeAngle(participant, drone);
        P_state = states[0];
        P_ObjectTarget = GetComponent<GameManager>().currentTarget;
        P_targetPosition = P_ObjectTarget.transform.position;
        D_Position = drone.transform.position;
        D_Rotation = drone.transform.rotation.eulerAngles;
        D_relativeOrientation = get_relativeAngle(drone, participant);
        D_state = states[0];
        D_ObjectTarget=drone.GetComponent<DroneController>().destinations[drone.GetComponent<DroneController>().i];
        D_targetPosition = D_ObjectTarget.transform.position;

        distance3D = Vector3.Distance(P_Position, D_Position);
        distance2D = get_distance2D(P_Position, D_Position);


        //Write headers + initial conditions in the save file
        sw.WriteLine("id,TimeStamp,focussedObject, P_x, P_y, P_z, P_ox, P_oy, P_oz, P_ro, P_state, P_objectTarget, P_targetX, P_targetY, P_targetZ, D_x, D_y, D_z, D_ox, D_oy, D_oz, D_ro, D_state, D_objectTarget, D_targetX, D_targetY, D_targetZ, D_danger, distance3D, distance2D, Condition, Started");
        sw.Flush();

        swQ.WriteLine("id,Condition,Q1,Q2,Q3,Q4,Q5");
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
            P_relativeOrientation = get_relativeAngle(participant, drone);
            P_ObjectTarget = GetComponent<GameManager>().currentTarget;

            if (GetComponent<GameManager>().grabbed)
            {
                P_state = states[1];
                P_targetPosition = GetComponent<GameManager>().currentDrop.transform.position;

            }
            else
            {
                P_targetPosition = P_ObjectTarget.transform.position;
                P_state = states[0];
            }


            D_Position = drone.transform.position;
            D_Rotation = drone.transform.rotation.eulerAngles;
            D_relativeOrientation = get_relativeAngle(drone, participant);
            D_ObjectTarget = drone.GetComponent<DroneController>().destinations[drone.GetComponent<DroneController>().i];

            if (drone.GetComponent<DroneController>().grabbed)
            {
                D_state = states[1];
                D_targetPosition = drone.GetComponent<DroneController>().destination;
                D_dangerLevel = D_ObjectTarget.tag;
            }
            else
            {
                D_targetPosition = D_ObjectTarget.transform.position;
                D_state = states[0];
                D_dangerLevel = "Low";
            }

            distance3D = Vector3.Distance(P_Position, D_Position);
            distance2D = get_distance2D(P_Position, D_Position);

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

    private float get_relativeAngle(GameObject from, GameObject target)
    {
        float relativeAngle = 0;
        Vector3 relativePos = target.transform.position - from.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        relativeAngle = (Quaternion.Inverse(transform.rotation) * rotation).eulerAngles.y;
        return relativeAngle;
    }

    private float get_distance2D(Vector3 from, Vector3 target)
    {
        return Vector3.Distance(new Vector3(from.x, 0, from.z), new Vector3(target.x, 0, target.z));
    }

    private void WrittingSave()
    {
        sw.WriteLine(id_participant+ ","+ time + "," + ET_name + "," + P_Position.x + "," + P_Position.y + "," + P_Position.z + "," + P_Rotation.x +","+ P_Rotation.y+","+ P_Rotation.z+","+P_relativeOrientation+","+P_state+","+P_ObjectTarget.name+","+P_targetPosition.x+","+ P_targetPosition.y+","+ P_targetPosition.z+","+ D_Position.x + "," + D_Position.y + "," + D_Position.z + "," + D_Rotation.x + "," + D_Rotation.y + "," + D_Rotation.z + "," + D_relativeOrientation + "," + D_state + "," + D_ObjectTarget.name + "," + D_targetPosition.x + "," + D_targetPosition.y + "," + D_targetPosition.z + "," + D_dangerLevel+","+distance3D+","+distance2D+","+Condition+","+started);
        sw.Flush();
    }

    public void WrittingSaveQ(float Q1, float Q2, float Q3, float Q4, float Q5)
    {
        swQ.WriteLine(id_participant + "," + Condition + "," +Q1+","+Q2+","+Q3+","+Q4+","+Q5); 
        swQ.Flush();
    }


}
