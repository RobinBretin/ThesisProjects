using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    [Header("--------------")]
    [Header("PARAMETERS")]
    [Header("--------------")]
    [SerializeField]
    public float id_participant;
    [SerializeField]
    public float freq; //Sample frequency, save every freq second

    //SAVE PARAMETERS
    private float time;
    private StreamWriter sw;
    private StreamWriter swQ;
    public string started;

    [SerializeField]
    private string defaultPath;
    [SerializeField]
    private string fileName;

    //REQUIREMENTS
    public GameObject participant;
    public GameObject drone;

    //QUESTIONS


    //DATA
    //Task    
    public float order;

    // Start is called before the first frame update
    void Start()
    {
        started = "No";
        //Initialize timer (for sampling frequency) and time for real time
        time = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnDestroy()
    {
        //Make sure to close the file before terminating the application
        sw.Flush();
        sw.Close();
        swQ.Flush();
        swQ.Close();
    }

    private float get_distance2D(Vector3 from, Vector3 target)
    {
        return Vector3.Distance(new Vector3(from.x, 0, from.z), new Vector3(target.x, 0, target.z));
    }

    public void WrittingSave(float rt, float response, int TaskCondition, string droneLevel, string Pstate)
    {
        sw.WriteLine(id_participant + "," + time + "," + rt + "," + response + "," + droneLevel + "," + TaskCondition + ","+Pstate+","+participant.transform.position.x+","+ participant.transform.position.y+","+ participant.transform.position.z+","+drone.transform.position.x+","+ drone.transform.position.y + "," + drone.transform.position.z + "," + Vector3.Distance(participant.transform.position, drone.transform.position)+ ","+ get_distance2D(participant.transform.position, drone.transform.position)+","+order);
        sw.Flush();
    }

    public void WrittingSaveQ(int TaskCondition, string droneLevel, float Q1, float Q2, float Q3, float Q4, float Q5, float Q6, float Q7, float Q8, float Q9, float Q10, float Q11, float Q12)
    {
        swQ.WriteLine(id_participant + "," + droneLevel + "," + TaskCondition + "," + Q12 +","+ Q1 +"," + Q2 + "," + Q3 + "," + Q4 + "," + Q11 + ","+ Q5 + "," + Q6 + "," + Q7 + "," + Q8 + "," + Q9 + "," + Q10 + ","+ order);
        swQ.Flush();
    }

    public void InitSave()
    {
        //Default path for save data
        defaultPath = Application.persistentDataPath;
        //Select a unique id for the participant
        while (File.Exists(defaultPath + id_participant + "_Task.txt"))
        {
            id_participant++;
        }

        //Create and name the file after this id
        fileName = id_participant.ToString();
        sw = File.CreateText(defaultPath + fileName + "_Task.txt");
        swQ = File.CreateText(defaultPath + fileName + "_Q.txt");

        //Write headers + initial conditions in the save file
        sw.WriteLine("id,TimeStamp, RT, Response, D_Level, Task_Condition, P_state, P_x, P_y, P_z, D_x, D_y, D_z, distance3D, distance2D, order");
        sw.Flush();

        swQ.WriteLine("id,D_Level, Task_Condition,Arousal,PL,PA,PT,Awareness,Bother,Mental,Physical,Pressure,Performance,Effort,Frustration, order");
        swQ.Flush();
    }
}
