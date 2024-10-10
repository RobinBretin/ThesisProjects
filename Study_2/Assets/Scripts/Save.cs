using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    // id_participant, order, gaze, facial_expression, task, participant_x, participant_y, participant_z, drone_x, drone_y, drone_z, . 

    [Header("--------------")]
    [Header("PARAMETERS")]
    [Header("--------------")]
    public int id_participant;
    private int order;
    [SerializeField]
    private float freq; //Sample frequency, save every freq second


    //SAVE PARAMETERS
    private float timer;
    private float time;
    private StreamWriter sw;
    private StreamWriter swQ;
    [SerializeField]
    private string defaultPath;
    [SerializeField]
    private string fileName;

    //REQUIREMENTS
    [SerializeField]
    private GameObject participant;
    [SerializeField]
    private GameObject drone;
    [SerializeField]
    private EyeIdle face;
    [SerializeField]
    private Questions questions;
    [SerializeField]
    private Display display;

    public bool start;


    //DATA
    //game
    public string task;
    public string gaze;
    public string facial_expression;

    //participant
    private Vector3 P_Position; //3D position
    private Vector3 P_Rotation; //3D rotation
    private float P_relativeOrientation; //Angle difference between where the participant is facing and where the drone is

    //drone
    private Vector3 D_Position;
    private Vector3 D_Rotation;
    private float D_relativeOrientation;


    //distances
    private float distance3D; //3D distance between drone position and headset position
    private float distance2D; //2D distance on the horizontal plan



    void Start()
    {
        start = false;
        //Initialize timer (for sampling frequency) and time for real time
        time = 0;
        timer = 0;

        id_participant = GetComponent<GameManager>().idParticipant;
        order = GetComponent<GameManager>().order;

        //Default path for save data
        defaultPath = Application.persistentDataPath;
        //Select a unique id for the participant
        while (File.Exists(defaultPath + id_participant + ".txt"))
        {
            id_participant++;
        }

        //Create and name the file after this id
        fileName = id_participant.ToString();
        sw = File.CreateText(defaultPath + fileName + ".txt");
        swQ = File.CreateText(defaultPath + fileName + "_Q.txt");

        //Initialize Data
        //Positions
        P_Position = participant.transform.position;
        P_Rotation = participant.transform.rotation.eulerAngles;
        P_relativeOrientation = get_relativeAngle(participant, drone);

        D_Position = drone.transform.position;
        D_Rotation = drone.transform.rotation.eulerAngles;
        D_relativeOrientation = get_relativeAngle(drone, participant);

        distance3D = Vector3.Distance(P_Position, D_Position);
        distance2D = get_distance2D(P_Position, D_Position);


        //Conditions
        task = "Talking";
        if (face.follow) gaze = "Follow";
        else gaze = "Avert";
        facial_expression = face.currentEmotion;

        //Write headers + initial conditions in the save file
        sw.WriteLine("id,TimeStamp,Task,Gaze_behaviour,Facial_expression,P_x,P_y,P_z,P_ox,P_oy,P_oz,P_ro,D_x,D_y,D_z,D_ox,D_oy,D_oz,D_ro,distance3D,distance2D,started");
        sw.Flush();

        swQ.WriteLine("id,Gaze_behaviour,Facial_expression,Q1,Q2,Q3,Q4,Q5,Q6,Q7,Q8,Arousal,Valence,Dominance");
        swQ.Flush();

        WrittingSave();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        time += Time.deltaTime;

        if (timer >= freq)
        {
            //Update Variables when saving (and not every frame)
            P_Position = participant.transform.position;
            P_Rotation = participant.transform.rotation.eulerAngles;
            P_relativeOrientation = get_relativeAngle(participant, drone);

            D_Position = drone.transform.position;
            D_Rotation = drone.transform.rotation.eulerAngles;
            D_relativeOrientation = get_relativeAngle(drone, participant);

            distance3D = Vector3.Distance(P_Position, D_Position);
            distance2D = get_distance2D(P_Position, D_Position);

            //Conditions
            if (display.active)
            {
                if (questions.Instruction.activeSelf)
                {
                    task = "Bypass";
                }
                else task = "Questions";
            }
            else task = "Talking";

            if (face.follow) gaze = "Follow";
            else gaze = "Avert";
            facial_expression = face.currentEmotion;

            start = GetComponent<GameManager>().start;

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
        sw.WriteLine(id_participant + "," + time + "," + task + "," + gaze + "," + facial_expression + "," + P_Position.x + "," + P_Position.y + "," + P_Position.z + "," + P_Rotation.x + "," + P_Rotation.y + "," + P_Rotation.z + "," + P_relativeOrientation + "," + D_Position.x + "," + D_Position.y + "," + D_Position.z + "," + D_Rotation.x + "," + D_Rotation.y + "," + D_Rotation.z + "," + D_relativeOrientation + "," + distance3D + "," + distance2D + "," + GetComponent<GameManager>().start);
        sw.Flush();
    }

    public void WrittingSaveQ(int S_Q1, int S_Q2, int S_Q3, int S_Q4, int S_Q5, int S_Q6, int S_Q7, int S_Q8, int S_SAM_arousal, int S_SAM_valence, int S_SAM_dominance)
    {
        swQ.WriteLine(id_participant + "," + gaze + "," + facial_expression + "," + S_Q1 + "," + S_Q2 + "," + S_Q3 + "," + S_Q4 + "," + S_Q5 + "," + S_Q6 + "," + S_Q7 + "," + S_Q8 + "," + S_SAM_valence + "," + S_SAM_arousal + "," + S_SAM_dominance); //Valence and arousal are inversed!!
        swQ.Flush();
    }
}
