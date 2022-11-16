using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public static BGM_Manager Instance { get; private set; }
    public const string Path = "Sounds/BGM/";
    private Dictionary<StageType, AudioClip> BGM_list = new Dictionary<StageType, AudioClip>();
    public float OriginalVolume { get; private set; }
    private AudioSource audioSource;
    private WaitForSeconds delay = new WaitForSeconds(0.01f);
    private float speed = 1.5f;
    private Coroutine fadeOutCoroutine;
    public float SeVolume { get; private set; } = 100f;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            OriginalVolume = audioSource.volume;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    void Start()
    {
        Init();
    }
    private void Init()
    {
        AudioClip clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Lobby");
        BGM_list.Add(StageType.Lobby, clip);

        clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Stage1");
        BGM_list.Add(StageType.Stage11, clip);

        clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Title");
        BGM_list.Add(StageType.Title, clip);

        clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Campfire");
        BGM_list.Add(StageType.Loading, clip);

        clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Opening");
        BGM_list.Add(StageType.Opening, clip);

        clip = Resources.Load<AudioClip>(Path + "Sound_Bgm_Ending");
        BGM_list.Add(StageType.Ending, clip);

        DataManager.Instance.LoadOptionData();
        SetVolume((float)DataManager.Instance.OptData.BgmValue / 100);
        SetSeVolume((float)DataManager.Instance.OptData.SeValue / 100);
    }

    public void Play(StageType BGM_Scene)
    {
        if(BGM_list.TryGetValue(BGM_Scene, out AudioClip bgm))
        {
            audioSource.clip = bgm;
            audioSource.Play();
        }
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void ResetOriginalVolume()
    {
        if(audioSource != null)
            audioSource.volume = OriginalVolume;
    }

    public void FadeOutBGM()
    {
        fadeOutCoroutine = StartCoroutine(ProcessFadeOutBGM());
    }

    public void StopFadeOutBGM()
    {
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
    }

    private IEnumerator ProcessFadeOutBGM()
    {
        // OriginalVolume = audioSource.volume;

        while (audioSource.volume > 0.15f)
        {
            audioSource.volume -= Time.deltaTime * speed;
            yield return delay;
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        OriginalVolume = audioSource.volume;
    }

    public void SetSeVolume(float value)
    {
        SeVolume = value;
    }

    public void SetLoop(bool enable)
    {
        audioSource.loop = enable;
    }
}
