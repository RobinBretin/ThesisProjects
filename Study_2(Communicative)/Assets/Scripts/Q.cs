using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Q : MonoBehaviour
{
    public Button button;
    public GameObject next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onClick(Button clickedB)
    {
        button = clickedB;
    }

    // Update is called once per frame
    void Update()
    {
        if (button != null)
        {
            next.GetComponent<Button>().interactable = true;
        }
        else next.GetComponent<Button>().interactable = false;
    }
}
