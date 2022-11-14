using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Setting : UI_Popup
{
    enum GameObjects
    {
        BackgroundPanel
    }

    enum Texts
    {
        TxtSettings,
        TxtResolution,
        TxtBGM,
        TxtBGMSlider,
        TxtEffect,
        TxtEffectSlider,
        TxtReturn
    }

    enum Buttons
    {
        BtnReturn
    }

    enum Sliders
    {
        SliBGM,
        SliEffect
    }

    enum Dropdowns
    {
        BoxResolution
    }

    void Start()
    {
        Init();
    }

    private int sliderMin = 0;
    private int sliderMax = 100;
    private string[] options = { "1920 * 1080" };

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<TMP_Dropdown>(typeof(Dropdowns));

        GameObject button = GetButton((int)Buttons.BtnReturn).gameObject;
        BindEvent(button, OnReturn, UIEvent.Click);

        Slider bgmSlider = Get<Slider>((int)Sliders.SliBGM);
        Slider effectSlider = Get<Slider>((int)Sliders.SliEffect);
        InitSlider(bgmSlider, OnChangeBGMSlider);
        InitSlider(effectSlider, OnChangeEffectSlider);

        TMP_Dropdown dropdown = InitDropdown();

        DataManager.Instance.LoadOptionData();
        bgmSlider.value = DataManager.Instance.OptData.BgmValue;
        effectSlider.value = DataManager.Instance.OptData.SeValue;
    }

    public void InitSlider(Slider slider, UnityEngine.Events.UnityAction<float> action)
    {
        slider.maxValue = sliderMax;
        slider.minValue = sliderMin;
        slider.wholeNumbers = true;
        slider.onValueChanged.AddListener(action);
        BindEvent(slider.gameObject, OnPointerUp, UIEvent.Up);
    }

    public TMP_Dropdown InitDropdown()
    {
        TMP_Dropdown box = Get<TMP_Dropdown>((int)Dropdowns.BoxResolution);
        box.options.Clear();

        for (int i = 0; i < options.Length; i++)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = options[i];
            box.options.Add(data);
        }

        box.SetValueWithoutNotify(-1);
        box.SetValueWithoutNotify(0);
        box.onValueChanged.AddListener(delegate { OnChangeDropdown(box); });

        return box;
    }

    public void OnReturn(PointerEventData data)
    {
        ClosePopupUI();
    }

    public void OnPointerUp(PointerEventData data)
    {
        DataManager.Instance.SaveOptionData();
    }

    public void OnChangeBGMSlider(float sound)
    {
        GetText((int)Texts.TxtBGMSlider).text = sound.ToString();
        DataManager.Instance.OptData.BgmValue = (int)sound;
        BGM_Manager.Instance.SetVolume(sound / 100);
    }

    public void OnChangeEffectSlider(float sound)
    {
        GetText((int)Texts.TxtEffectSlider).text = sound.ToString();
        DataManager.Instance.OptData.SeValue = (int)sound;
    }

    public void OnChangeDropdown(TMP_Dropdown box)
    {
        Debug.Log($"{box.options[box.value].text}");
    }
}
