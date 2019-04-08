using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Audio : MonoBehaviour
{
    AudioSource as_AudioSource;
    public AudioClip ac_Wind;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Audio!");
        as_AudioSource = GetComponent<AudioSource>();
        as_AudioSource.PlayOneShot(ac_Wind);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
