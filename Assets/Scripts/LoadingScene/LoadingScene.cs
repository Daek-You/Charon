using System.Collections;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScene : BaseScene
{
    public static string nextSceneName;
    public static StageType nextSceneBGM;
    [SerializeField] private Slider prograssBar;
    private float timer = 0f;
    private float speed = 1.5f;

    public static void LoadScene(string sceneName, StageType sceneBGM)
    {
        nextSceneName = sceneName;
        nextSceneBGM = sceneBGM;
        UIManager.Instance.Clear();
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        //Init();
        BGM_Manager.Instance.Play(StageType.Loading);
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    private IEnumerator LoadAsynSceneCoroutine()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        operation.allowSceneActivation = false;
        timer = 0f;
        prograssBar.value = 0f;

        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if(operation.progress < 0.9f)
            {
                prograssBar.value = Mathf.Lerp(prograssBar.value, 1f, Time.deltaTime * speed);

                if (prograssBar.value >= operation.progress)
                    timer = 0f;
            }
            else
            {

                prograssBar.value = prograssBar.value > 0.98f ? Mathf.Lerp(prograssBar.value, 1f, timer) : Mathf.Lerp(prograssBar.value, 1f, Time.deltaTime * speed);

                if (Mathf.Approximately(prograssBar.value, 1f))
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }

    public override void Init()
    {
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
        StageManager.Instance.CurrentStage = StageType.Unknown;
    }

    public override void Clear()
    {
        // UI.Instance.Clear();가 있었으나, LoadScene()이 static 함수이므로 그냥 빼놨음
    }
}
