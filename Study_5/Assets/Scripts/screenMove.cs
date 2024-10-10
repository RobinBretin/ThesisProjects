using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenMove : MonoBehaviour
{
    public GameObject player;
    public N_Back nbackScript;
    private Vector3 initialPo;
    private Vector3 targetPo;
    public float initDist;
    private float deltaDist;
    public float speedMove;
    IEnumerator coroutineGoTo;
    // Start is called before the first frame update
    void Start()
    {
        initialPo = transform.position;
        coroutineGoTo = GoTo();
        //initDist = Vector3.Distance(player.transform.position, initialPo);
    }

    // Update is called once per frame
    void Update()
    {

        if (nbackScript.proxemic == "walk")
        {
            if (transform.position.z - player.transform.position.z < initDist + .03 && transform.position.z - player.transform.position.z > initDist - .03) StopCoroutine(coroutineGoTo);
            else StartCoroutine(coroutineGoTo);
        }
        else {
            transform.position = initialPo;
            StopCoroutine(coroutineGoTo);
        } 

    }

    private IEnumerator GoTo()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(new Vector3(initialPo.x, initialPo.y, transform.position.z), player.transform.position+new Vector3(0,0,initDist), Time.deltaTime);
            yield return null;
        }
    }
}
