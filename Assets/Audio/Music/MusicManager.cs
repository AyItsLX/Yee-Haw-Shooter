using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public Music[] musics;

    private void Awake()
    {
        foreach (Music m in musics)
        {
            m.audiosource = gameObject.AddComponent<AudioSource>();

            m.audiosource.clip = m.clip;

            m.audiosource.volume = m.volume;
            m.audiosource.pitch = m.pitch;

            m.audiosource.loop = m.loop;
        }

        Play("Opening");
    }

    public void Play(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            return;
        }
        m.audiosource.Play();
    }

    public void Stop(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            return;
        }
        m.audiosource.Stop();
    }

    public void Volume(string name, float level)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            return;
        }
        m.audiosource.volume -= level;
    }
}
