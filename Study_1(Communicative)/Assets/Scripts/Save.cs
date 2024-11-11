using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Save : MonoBehaviour
{
    public float freq;
    private float timer;
    private float time;

    private bool social_framing;
    private string height_condition;
    public int id_participant;

    private StreamWriter sw;
    public string defaultPath;
    public string fileName;

    private bool save;
    private bool isFlying;

    public GameObject participant;
    private Vector3 p_position;
    public GameObject drone;
    private Vector3 d_position;
    private float participant_drone_distance;
    //private float horizontal_participant_drone_distance
    private float velocity;
    private Vector3 lastPosition;

    void Start()
    {
        //The drone is not initially flying
        isFlying=false;

        //Initialize timer (for sampling frequency) and time for real time
        time=Time.realtimeSinceStartup;
        timer=0;

        //Get initial positions/distance
        p_position=participant.transform.position;
        d_position=drone.transform.position;
        participant_drone_distance=Vector3.Distance(p_position,d_position);
        //horizontal_participant_drone_distance=sqrt
        velocity=0;
        lastPosition=p_position;

        //Get initial experiment settings from gameManager
        social_framing=GetComponent<GameManager>().social_framing;
        height_condition=GetComponent<GameManager>().height_conditions_order[0];

        //Default path for save data
        //defaultPath="C:/Users/robin/OneDrive/Bureau/These/R_Scripts/Saves/";
        defaultPath=Application.persistentDataPath;


        //Select a unique id for the participant
        while (File.Exists(defaultPath+id_participant+".txt"))
        {
            id_participant++; 
        }

        //Create and name the file after this id
        fileName=id_participant.ToString();
        sw = File.CreateText(defaultPath+fileName+".txt");

        //Write headers + initial conditions in the save file
        sw.WriteLine("id,height_condition,social_framing,participant_y,participant_x,participant_z,drone_x,drone_z,drone_y,time,participant_drone_distance,velocity");
        sw.WriteLine(id_participant+","+height_condition+","+social_framing+","+p_position.y*100+","+p_position.x*100+","+p_position.z*100+","+d_position.x*100+","+d_position.z*100+","+d_position.y*100+","+time+","+participant_drone_distance*100+","+velocity);
    }

    void Update()
    {
        //Calculate participant velocity
        if(participant.transform.position != lastPosition)
        {
            velocity=Vector3.Distance(participant.transform.position,lastPosition)/Time.deltaTime;
            lastPosition = participant.transform.position;
        }
        else velocity=0;

        //Check if the drone is flying by checking animation state
        if(drone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("1"))
        {
            height_condition=drone.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
            isFlying=true;
        }
        else isFlying=false;

        //Updating timer
        timer+=Time.deltaTime;

        //if timer > selected period for sampling, data are saved and timer reinitialized
        if(timer>=freq) 
        {
            p_position=participant.transform.position;
            d_position=drone.transform.position;
            time=Time.realtimeSinceStartup;
            participant_drone_distance=Vector3.Distance(p_position,d_position);
            //Only save if the drone is flying
            if(isFlying) sw.WriteLine(id_participant+","+height_condition+","+social_framing+","+p_position.y*100+","+p_position.x*100+","+p_position.z*100+","+d_position.x*100+","+d_position.z*100+","+d_position.y*100+","+time+","+participant_drone_distance*100+","+velocity);
            else
            {
                height_condition="end";
                sw.WriteLine(id_participant+","+height_condition+","+social_framing+","+p_position.y*100+","+p_position.x*100+","+p_position.z*100+","+d_position.x*100+","+d_position.z*100+","+d_position.y*100+","+time+","+participant_drone_distance*100+","+velocity);
            }
            timer=0;
        }
    }

    //Close the file when the session is stoped
    void OnApplicationQuit()
    {
        sw.Close();
    }
}
