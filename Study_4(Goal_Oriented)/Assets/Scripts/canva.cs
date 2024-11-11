using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canva : MonoBehaviour
{
    private float time;
    private string text;
    [SerializeField] private DroneController drone;
    [SerializeField] private GameManager player;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (drone.start)
        {
            if (drone.grabbed == player.grabbed)
            {
                time += Time.deltaTime;
            }
            else
            {
                //if (!player.grabbed) time += Time.deltaTime;
            }

            if (time > 10) text = "<color=#FF0000>" + (Mathf.Round(time * 10) / 10f).ToString() + "</color >";
            else text = (Mathf.Round(time * 10) / 10f).ToString();
            GetComponent<TextMeshProUGUI>().text = text;

            if (player.P_dropped) time = 0;
        }

    }
}
