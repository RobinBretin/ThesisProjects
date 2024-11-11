using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Save : MonoBehaviour
{
    
    //GAMEOBJECT NEEDED
    public GameObject drone;
    
    //Var to save
    [Header("--------------")]
    [Header("PARAMETERS")]
    [Header("--------------")]
    public int id_participant; //ID
    public float[] speeds;


    private int orderId; //id de l'ordre des vitesse
    private float speed_condition; //current speed condition
    private string envir_condition; //current environment
    private string phase; //current phase
    private string proxemicSpace; //current proxemic area


    //FREQ
    public float freq;

    //SAVE START
    private bool startSave;
    private bool ended;

    //SAVE PARAMETERS
    private float timer;
    private float time;
    private StreamWriter sw;
    public string defaultPath;
    public string fileName;

    private bool save;



    void Start()
    {

        if(speeds[0]==1) orderId=0;
        else orderId=1;
        //The drone is not initially flying
        startSave=false;
        ended=false;

        //Initialize timer (for sampling frequency) and time for real time
        time=0;
        timer=0;

        //Get initial experiment settings from gameManager
        envir_condition="VR";
        speed_condition=drone.GetComponent<moveTest>().speed;
        phase=drone.GetComponent<moveTest>().phase;
        proxemicSpace=drone.GetComponent<moveTest>().proxemicSpace;
        //speed_condition=GetComponent<GameManager>().height_conditions_order[0];

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
        sw.WriteLine("id,orderId,envir_condition,speed_condition,time,phase,proxemicArea");
        sw.WriteLine(id_participant+","+orderId+","+envir_condition+","+speed_condition+","+time+","+phase+","+proxemicSpace);
    }

    void Update()
    {
        startSave=drone.GetComponent<moveTest>().start;
        ended=drone.GetComponent<moveTest>().ended;
        speed_condition=drone.GetComponent<moveTest>().speed;
        phase=drone.GetComponent<moveTest>().phase;
        proxemicSpace=drone.GetComponent<moveTest>().proxemicSpace;

        if(startSave && !ended){
            timer+=Time.deltaTime;
            time+=Time.deltaTime;
            if(timer>=freq) 
            {
            sw.WriteLine(id_participant+","+orderId+","+envir_condition+","+speed_condition+","+time+","+phase+","+proxemicSpace);
            timer=0;
            } 
        }
    }

    //Close the file when the session is stoped
    void OnApplicationQuit()
    {
        sw.Close();
    }
}
