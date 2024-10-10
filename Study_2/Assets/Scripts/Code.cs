using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Code : MonoBehaviour
{
    public Button[] entries;
    public Sprite[] code;
    private int i;

    private float time;
    public Sprite loading;
    public Sprite wrong;
    public Sprite right;

    private bool isLoading;
    private IEnumerator CoroutineCheck;
    public GameObject question;
    public Questions questions;
    private bool activeQ;

    public Material listen;

    // Start is called before the first frame update

    void Start()
    {
        i = 0;
        CoroutineCheck = checkShape(i);
        activeQ = false;
        listen.SetColor("_EmissionColor", Color.red);

    }

    void Update()
    {

        
    }

    public void OnShapeClick(Sprite shape)
    {
        for (int j = 0; j < i; j++)
        {
            if(entries[j].image.sprite == null)
            {
                entries[j].image.sprite = shape;
                entries[j].GetComponent<Button>().enabled = true;
                StartCoroutine(checkShape(j));
                return;
            }
        }
        entries[i].image.sprite = shape;
        entries[i].GetComponent<Button>().enabled = true;

        StartCoroutine(checkShape(i));
        
            
        if (i >= entries.Length-1) i = 0;
        else i++;
    }

    public void OnCodeClick(Button button)
    {
        button.image.sprite = null;
        removeCheck(button);
        button.GetComponent<Button>().enabled = false;
        StopAllCoroutines();
    }

    /*public void checkShape()
    {

        entries[i].transform.GetChild(1).GetComponent<Image>().sprite = loading;
        isLoading = true;
        
    }*/

    private IEnumerator checkShape(int i)
    {
        isLoading = true;
        entries[i].transform.GetChild(1).GetComponent<Image>().sprite = loading;

        yield return new WaitForSeconds(1f);

        // Check if input is correct.
        if (entries[i].image.sprite == code[i])
        {
            // Display correct sprite.
            entries[i].transform.GetChild(1).GetComponent<Image>().sprite = right;
            questions.questions[0].SetActive(true);
            questions.Instruction.SetActive(false);
            listen.SetColor("_EmissionColor", Color.red);
            OnOffQ();
        }
        else
        {
            // Display wrong sprite.
            entries[i].transform.GetChild(1).GetComponent<Image>().sprite = wrong;
        }

        // Set isLoading flag to false.
        isLoading = false;
    }

    public void OnOffQ()
    {
        if (activeQ)
        {
            question.SetActive(false);
            activeQ = false;
        }
        else
        {
            question.SetActive(true);
            activeQ = true;
        }
    }

    private void removeCheck(Button button)
    {
        button.transform.GetChild(1).GetComponent<Image>().sprite = null;
    }



}
