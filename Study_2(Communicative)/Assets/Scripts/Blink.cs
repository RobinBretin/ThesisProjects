using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private Animator animator;
    private float timeToBlink;
    private IEnumerator blinkCoroutine;
    [SerializeField] private GameObject eyes;

    //CONDITIONS
    [SerializeField] private GameManager GM;
    private int order;
    private bool follow;
    


    private string currentEmotion;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timeToBlink = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        currentEmotion = GetComponent<SpriteRenderer>().sprite.name;
        currentEmotion = currentEmotion.Substring(0, currentEmotion.IndexOf("_"));
        //Debug.Log(currentEmotion);

        if(Time.time > timeToBlink)
        {
            blinkCoroutine = DoBlink();
            StartCoroutine(blinkCoroutine);
        }
        
    }

    private IEnumerator DoBlink()
    {
        
        animator.Play(currentEmotion+"_blink");
        yield return new WaitForSeconds(0.5f);
        
        animator.Play(currentEmotion);
        timeToBlink = Time.time + Random.Range(0.5f, 7f);

    }
}
