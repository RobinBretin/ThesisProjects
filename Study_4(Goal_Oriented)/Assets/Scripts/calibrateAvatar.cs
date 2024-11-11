using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


public class calibrateAvatar : MonoBehaviour
{
    public VRIK ik;
    // Start is called before the first frame update
    void Start()
    {
        ik = GetComponent<VRIK>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / (ik.references.head.position.y - ik.references.root.position.y);
        ik.references.root.localScale *= sizeF;

    }
}
