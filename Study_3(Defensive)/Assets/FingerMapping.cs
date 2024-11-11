using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[System.Serializable]
public class Finger
{
    //https://forum.unity.com/threads/final-ik-full-body-ik-aim-look-at-fabrik-ccd-ik-1-0-released.222685/page-87#post-5737060
    public Transform[] avatarBones = new Transform[0];
    public Transform[] targetBones = new Transform[0];
    public Vector3 avatarForwardAxis = Vector3.forward;
    public Vector3 avatarUpAxis = Vector3.up;
    public Vector3 targetForwardAxis = Vector3.forward;
    public Vector3 targetUpAxis = Vector3.up;
}

public class FingerMapping : MonoBehaviour {

    public Vector3 offset;

    public IK ik;
    [Range(0f, 1f)]public float weight = 1f;

    [System.Serializable]
    public enum Mode
    {
        FromToRotation,
        MatchRotation
    }

    public Mode mode;

    public Finger[] fingers;

    private void Start()
    {
        // Register to get a call from VRIK every time after it updates
        ik.GetIKSolver().OnPostUpdate += AfterVRIK;
    }

    private void AfterVRIK()
    {
        if (weight <= 0f) return;

        switch (mode)
        {
            case Mode.FromToRotation:

                foreach (Finger finger in fingers)
                {
                    for (int i = 0; i < finger.avatarBones.Length - 1; i++)
                    {
                        // Get the Quaternion to rotate the current finger bone towards the next target bone position.
                        Quaternion f = Quaternion.FromToRotation(finger.avatarBones[i + 1].position - finger.avatarBones[i].position, finger.targetBones[i + 1].position - finger.avatarBones[i].position);

                        // Weight blending
                        if (weight < 1f)
                        {
                            f = Quaternion.Slerp(Quaternion.identity, f, weight);
                        }

                        // Rotate this finger bone
                        finger.avatarBones[i].rotation = f * finger.avatarBones[i].rotation;

                    }
                }

                break;
            case Mode.MatchRotation:
                foreach (Finger finger in fingers)
                {
                    bool orientationMatch = finger.avatarForwardAxis == finger.targetForwardAxis && finger.avatarUpAxis == finger.targetUpAxis;

                    for (int i = 0; i < finger.avatarBones.Length; i++)
                    {
                        Quaternion r = orientationMatch? finger.targetBones[i].rotation: RootMotion.QuaTools.MatchRotation(finger.targetBones[i].rotation, finger.targetForwardAxis, finger.targetUpAxis, finger.avatarForwardAxis, finger.avatarUpAxis);

                        if (weight < 1f)
                        {
                            r = Quaternion.Slerp(finger.avatarBones[i].rotation, r, weight);
                        }

                        finger.avatarBones[i].rotation = r;
                        finger.avatarBones[i].rotation *= Quaternion.Euler(offset);
                    }
                }
                break;
        }
    }
}
