using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Object/Sound", order = int.MaxValue)]
public class Sound : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
}


public class SoundManager : MonoBehaviour
{
    public AudioSource[] audiosourceEffects;
    public AudioSource audioSourceBGM;
    public Dictionary<string, Sound> sounds;

    public static SoundManager Instance { get; private set; }
    private static SoundManager instance;


    #region #Singleton
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }
    #endregion


}
