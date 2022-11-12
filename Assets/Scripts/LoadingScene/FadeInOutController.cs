using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FadeInOutController : MonoBehaviour
{
    public static FadeInOutController Instance { get; private set; }
    public bool isDone;
    public Canvas canvas;
    public Image image;
    private WaitForSeconds displayFadeDelay = new WaitForSeconds(0.001f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void FadeIn(StageType nextStageBGM)
    {
        canvas.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        StartCoroutine(ProcessFadeIn());
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BGM_Manager.Instance.StopFadeOutBGM();
        BGM_Manager.Instance.ResetOriginalVolume();

        if (SceneManager.GetActiveScene().name == "LoadingScene")
        {
            FadeInOutController.Instance.SetActive(false);
            return;
        }

        if (SceneManager.GetActiveScene().name == "OpeningScene")
        {
            BGM_Manager.Instance.Stop();
            return;
        }

        FadeIn(LoadingScene.nextSceneBGM);
        BGM_Manager.Instance.Play(LoadingScene.nextSceneBGM);
    }


    public void FadeOut()
    {
        canvas.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        StartCoroutine(ProcessFadeOut(null, StageType.Unknown));
        BGM_Manager.Instance.FadeOutBGM();
    }

    public void FadeOutAndLoadScene(string nextSceneName, StageType nextSceneBGM)
    {
        canvas.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        StartCoroutine(ProcessFadeOut(nextSceneName, nextSceneBGM));
        BGM_Manager.Instance.FadeOutBGM();
    }

    private IEnumerator ProcessFadeIn()
    {
        isDone = false;

        while (image.color.a > 0f)
        {
            float alphaValue = image.color.a - Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
            yield return displayFadeDelay;
        }
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator ProcessFadeOut(string nextSceneName, StageType nextSceneBGM)
    {
        isDone = false;

        while (image.color.a < 1f)
        {
            float alphaValue = image.color.a + Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
            yield return displayFadeDelay;
        }

        isDone = true;

        if(nextSceneName != null)
        {
            LoadingScene.LoadScene(nextSceneName, nextSceneBGM);
        }
    }

    public void SetActive(bool enable)
    {
        canvas.gameObject.SetActive(enable);
    }
}
