using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //Conditions
    public int order;
    public int currentCondition;
    public int idParticipant;

    public bool start;
    public InputAction input;
    public SpriteRenderer face;
    public SpriteRenderer eyes;
    public SpriteRenderer black;

    //public EyeIdle eyeIdle;


    //A-Joy F, B-Joy A
    //C- Anger F, D- Anger A,
    //E- Fear F, F- Fear A
    //G- Sad F, H Sad A
    //I- Surprise F, K Surprise A

    private List<object> A = new List<object> { 0, true };
    private List<object> B = new List<object> { 0, false };
    private List<object> C = new List<object> { 1, true };
    private List<object> D = new List<object> { 1, false };
    private List<object> E = new List<object> { 2, true };
    private List<object> F = new List<object> { 2, false };
    private List<object> G = new List<object> { 3, true };
    private List<object> H = new List<object> { 3, false };
    private List<object> I = new List<object> { 4, true };
    private List<object> J = new List<object> { 4, false };


    public List<List<object>> conditions = new List<List<object>>();
    // Start is called before the first frame update
    void Start()
    {
        face.enabled = false;
        eyes.enabled = false;
        black.enabled = true;
        start = false;
        input.Enable();
        currentCondition =0;

        //See https://cs.uwaterloo.ca/~dmasson/tools/latin_square/ for a 10x10 matrix
        if (order == 1)
        {
            conditions = new List<List<object>> { A, B, J, C, I, D, H, E, G, F };
        }

        if (order == 2)
        {
            conditions = new List<List<object>> { B, C, A, D, J, E, I, F, H, G };
        }
        if (order == 3)
        {
            conditions = new List<List<object>> { C, D, B, E, A, F, J, G, I, H };
        }
        if (order == 4)
        {
            conditions = new List<List<object>> { D, E, C, F, B, G, A, H, J, I };
        }
        if (order == 5)
        {
            conditions = new List<List<object>> { E, F, D, G, C, H, B, I, A, J };
        }
        if (order == 6)
        {
            conditions = new List<List<object>> { F, G, E, H, D, I, C, J, B, A };
        }
        if (order == 7)
        {
            conditions = new List<List<object>> { G, H, F, I, E, J, D, A, C, B };
        }
        if (order == 8)
        {
            conditions = new List<List<object>> { H, I, G, J, F, A, E, B, D, C };
        }
        if (order == 9)
        {
            conditions = new List<List<object>> { I, J, H, A, G, B, F, C, E, D };
        }
        if (order == 10)
        {
            conditions = new List<List<object>> { J, A, I, B, H, C, G, D, F, E };
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (input.triggered || start)
        {
            start = true;
            eyes.enabled = true;
            face.enabled = true;
            black.enabled = false;
        }

    }

}
