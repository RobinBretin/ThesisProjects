using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagment : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public bool pitchMove;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        audioSource.clip = audioClips[i];
        pitchMove = true;
    }

    public void ChangeAudio()
    {
        if (i < audioClips.Length) i++;
        else i = 0;

        if (i == 1) pitchMove = false;
        else pitchMove = true;

        audioSource.clip = audioClips[i];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
