using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;
using System.Text;
using UnityEngine.SceneManagement;
public class Narration_Opening : MonoBehaviour
{
    public TMP_Text dialogText;
    public TMP_Text nameText;
    public AudioSource BGM_SoundSource;
    public AudioSource Eff_SoundSource;
    private JsonData _jsonDialogData;
    private JsonData _jsonNameData;
    private int indexD; private int indexN;
    private StringBuilder _jsonDialogStringBD = new StringBuilder();
    private StringBuilder _jsonNameStringBD = new StringBuilder();
    private WaitForSeconds delayTime = new WaitForSeconds(0.08f);
    private WaitForSeconds displayFadeDelay = new WaitForSeconds(0.01f);
    private WaitForSeconds soundFadeOutDelay = new WaitForSeconds(0.01f);

    private string jsonDialogLine;
    private string jsonNameLine;
    private float speed = 0.3f;
    public GameObject removableBackground;
    public Image img;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;


    void Awake()
    {
        _jsonDialogData = ReadNarrationFile();
        _jsonNameData = ReadNameFile();
        indexD = 0;
        indexN = 0;

    }

    void Start()
    {
        BGM_SoundSource.volume = BGM_Manager.Instance.OriginalVolume;
        Eff_SoundSource.volume = BGM_Manager.Instance.OriginalVolume;
    }

    public void ShowNarration()
    {
        if (_jsonDialogData != null && indexD < _jsonDialogData[0].Count)
        {
            dialogText.text = "";
            _jsonDialogStringBD.Clear();
            jsonDialogLine = _jsonDialogData[0][indexD].ToString();
            StartCoroutine(DialogTyping());
            indexD++;
        }
    }

    public void ShowName()
    {
        if (_jsonNameData != null && indexN < _jsonNameData[0].Count)
        {
            nameText.text = "";
            _jsonNameStringBD.Clear();
            jsonNameLine = _jsonNameData[0][indexN].ToString();
            StartCoroutine(NameTyping());
            indexN++;
        }
    }

    private IEnumerator DialogTyping()
    {
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 1f);

        foreach (char i in jsonDialogLine)
        {
            dialogText.text = _jsonDialogStringBD.Append(i).ToString();
            yield return delayTime;
        }
    }
    private IEnumerator NameTyping()
    {
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 1f);

        foreach (char i in jsonNameLine)
        {
            nameText.text = _jsonNameStringBD.Append(i).ToString();
            yield return delayTime;
        }
    }
    public void TextFadeOut()
    {
        StartCoroutine(FadeOutText());
    }

    public void NameFadeOut()
    {
        StartCoroutine(FadeOutName());
    }

    private IEnumerator FadeOutText()
    {
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, dialogText.color.a);

        while (dialogText.color.a > 0f)
        {
            float alphaValue = dialogText.color.a - (Time.deltaTime);
            dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, alphaValue);
            yield return null;
        }
    }

    private IEnumerator FadeOutName()
    {
        nameText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, dialogText.color.a);

        while (nameText.color.a > 0f)
        {
            float alphaValue = nameText.color.a - (Time.deltaTime);
            nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, alphaValue);
            yield return null;
        }
    }

    private JsonData ReadNarrationFile()
    {
        var jsonTextFile = Resources.Load("Json/DialogList");
        JsonData jsonData = JsonMapper.ToObject(jsonTextFile.ToString());
        return jsonData;
    }

    private JsonData ReadNameFile()
    {
        var jsonTextFile = Resources.Load("Json/NameList");
        JsonData jsonData = JsonMapper.ToObject(jsonTextFile.ToString());
        return jsonData;
    }

    public void TurnBackground()
    {
        removableBackground.gameObject.SetActive(false);
    }

    public void ChangeScene()
    {
        //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "LobbyScene");
        LoadingScene.LoadScene("LobbyScene", StageType.Lobby);
    }

    public void FadeOutDisplay()
    {
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = StartCoroutine(OpeningSceneFadeOut());
    }

    private IEnumerator OpeningSceneFadeOut()
    {
       img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a);

        while (img.color.a < 1f)
        {
            float alphaValue = img.color.a + (Time.deltaTime);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);
            
            yield return displayFadeDelay;
        }
    }

    public void FadeInDisplay()
    {
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
        fadeInCoroutine = StartCoroutine(OpeningSceneFadeIn());
    }

    private IEnumerator OpeningSceneFadeIn()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a);

        while (img.color.a > 0f)
        {
            float alphaValue = img.color.a - (Time.deltaTime);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);

            yield return displayFadeDelay;
        }
    }

    public void FadeOutSound()
    {
        StartCoroutine(FadeOutOpeningSounde());
    }

    public void FadeInSound()
    {
        StartCoroutine(FadeInOpeningSounde());
    }

    private IEnumerator FadeOutOpeningSounde()
    {
        while(Eff_SoundSource.volume > 0.15f && BGM_SoundSource.volume > 0.15f)
        {
            BGM_SoundSource.volume -= Time.deltaTime;
            Eff_SoundSource.volume -= Time.deltaTime;
            yield return soundFadeOutDelay;
        }
    }

    private IEnumerator FadeInOpeningSounde()
    {
        while (Eff_SoundSource.volume < BGM_Manager.Instance.OriginalVolume && BGM_SoundSource.volume < BGM_Manager.Instance.OriginalVolume)
        {
            BGM_SoundSource.volume += Time.deltaTime * speed;
            Eff_SoundSource.volume += Time.deltaTime * speed;
            yield return soundFadeOutDelay;
        }
    }
}
