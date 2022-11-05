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
    private JsonData _jsonDialogData;
    private JsonData _jsonNameData;
    private int indexD; private int indexN;
    private StringBuilder _jsonDialogStringBD = new StringBuilder();
    private StringBuilder _jsonNameStringBD = new StringBuilder();
    private WaitForSeconds delayTime = new WaitForSeconds(0.08f);
    private WaitForSeconds commaDelayTime = new WaitForSeconds(1.5f);
    private WaitForSeconds loadingDelay = new WaitForSeconds(2f);
    private string jsonDialogLine;
    private string jsonNameLine;
    public GameObject removableBackground;
    void Awake()
    {
        _jsonDialogData = ReadNarrationFile();
        _jsonNameData = ReadNameFile();
        indexD = 0;
        indexN = 0;
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
        SceneManager.LoadScene("LobbyScene");
    }
}
