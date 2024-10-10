using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Display : MonoBehaviour
{
    public GameObject[] canvas;
    private GameObject ActiveCanva;
    private int i;

    public AudioSource audioSource;
    public AudioClip[] clips;

    public TextMeshProUGUI text;

    public InputAction input;
    public InputAction input2;
    public Material listen;
    public bool active;

    public EyeIdle eye;


    // Start is called before the first frame update
    void Start()
    {
        ActiveCanva = canvas[0];
        audioSource.clip = clips[0];
        active = false;
        input.Enable();
        input2.Enable();
        i = 0;
    }

    void Update()
    {
        text.text = eye.next.ToString();
        if (input.triggered)
        {
            onOFF();
            /*if (active) ActiveCanva.SetActive(false);
            else
            {
                ActiveCanva.SetActive(true);
                active = true;
            }
            //onSelect();*/
        }
        if (input2.triggered)
        {
            listen.SetColor("_EmissionColor", Color.green);
        }
    }

    // Update is called once per frame
    public void onSelect()
    {
        if (i + 1 >= canvas.Length)
        {
            i = 0;
        }
        else
        {
            i++;
        }
        if (active)
        {
            onOFF();
        }
        ActiveCanva = canvas[i];
        audioSource.clip = clips[i];


        /*if (i + 1 < canvas.Length)
        {
            if (canvas[i] != null)
            {
                canvas[i].SetActive(false);
            }
            i++;
            ActiveCanva=canvas[i];
        }
        else i = 0;*/
    }

    public void onOFF()
    {
        if (active)
        {
            ActiveCanva.SetActive(false);
            active = false;
        }
        else
        {
            ActiveCanva.SetActive(true);
            active = true;
            audioSource.Play();
        }

    }
}
