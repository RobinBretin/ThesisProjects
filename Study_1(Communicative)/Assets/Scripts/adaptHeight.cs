using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adaptHeight : MonoBehaviour
{
    public AnimationClip eye_level_clip;
    private AnimationCurve eye_level_curve;
    public float height;
    
    // Start is called before the first frame update
    void Start()
    {
        //kfs=eye_level.AnimationCurve.keys;
        

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keys;
        keys = new Keyframe[2];
        keys[0] = new Keyframe(0.0f, 0.0f);
        keys[1] = new Keyframe(0.0f, height);
        eye_level_curve = new AnimationCurve(keys);
        eye_level_clip.SetCurve("", typeof(Transform), "localPosition.x", eye_level_curve);      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
