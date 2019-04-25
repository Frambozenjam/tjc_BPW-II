using UnityEngine.Audio;
using System;
using UnityEngine;

public class Script_ManagerAudio : MonoBehaviour
{

    public Class_Sound[] Sounds;

    public static Script_ManagerAudio instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Class_Sound s in Sounds)
        {
            s.as_AudioSource = gameObject.AddComponent<AudioSource>();
            s.as_AudioSource.clip = s.ac_AudioClip;
            s.as_AudioSource.volume = s.f_Volume;
            s.as_AudioSource.pitch = s.f_Pitch;
            s.as_AudioSource.loop = s.b_Loop;
            s.as_AudioSource.time = s.f_StartTime;
        }
    }

    public void Function_PlayAudio (string name)
    {
        Class_Sound s = Array.Find(Sounds, Class_Sound => Class_Sound.s_Name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist.");
            return;
        }
        s.as_AudioSource.Play();
    }

    public void Function_StopAudio (string name)
    {
        Class_Sound s = Array.Find(Sounds, Class_Sound => Class_Sound.s_Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not playing.");
            return;
        }
        s.as_AudioSource.Stop();
    }
}
