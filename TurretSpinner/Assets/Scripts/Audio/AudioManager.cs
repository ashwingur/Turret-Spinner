using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    public static AudioManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Only 1 instance exists
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        // Dont destroy between scenes since we want it to persist
        DontDestroyOnLoad(gameObject); 

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        instance.Play("Background");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("AudioManager: Sound " + name + "not found");
            return;
        }
        s.source.Play();
    }
}
