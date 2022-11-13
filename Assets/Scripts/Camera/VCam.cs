using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class VCam : MonoBehaviour
{
    public Camera mainCam;
    public static VCam Instance { get; private set; }
    public CinemachineVirtualCamera VirtualCam { get; private set; }
    public CinemachineImpulseListener ImpulseListener { get; private set; }

    private float originalImpulseListenerGain;
    private float originalImpulseListenerAmplitude;
    private float originalImpulseListenerFrequency;
    private float originalImpulseListenerDuration;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCam = Camera.main;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(mainCam);
            return;
        }
        DestroyImmediate(gameObject);
        DestroyImmediate(mainCam);
    }

    void Start()
    {
        VirtualCam = GetComponent<CinemachineVirtualCamera>();
        ImpulseListener = GetComponent<CinemachineImpulseListener>();
        originalImpulseListenerGain = ImpulseListener.m_Gain;
        originalImpulseListenerAmplitude = ImpulseListener.m_ReactionSettings.m_AmplitudeGain;
        originalImpulseListenerFrequency = ImpulseListener.m_ReactionSettings.m_FrequencyGain;
        originalImpulseListenerDuration = ImpulseListener.m_ReactionSettings.m_Duration;
    }

    void LateUpdate()
    {
        Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("EnvironmentObject"));

        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }

    public void SetImpulseOptions(float gain, float amplitude, float frequency, float duration)
    {
        ImpulseListener.m_Gain = gain;
        ImpulseListener.m_ReactionSettings.m_AmplitudeGain = amplitude;
        ImpulseListener.m_ReactionSettings.m_FrequencyGain = frequency;
        ImpulseListener.m_ReactionSettings.m_Duration = duration;
    }

    public void ResetImpulseListenerSettings()
    {
        ImpulseListener.m_Gain = originalImpulseListenerGain;
        ImpulseListener.m_ReactionSettings.m_AmplitudeGain = originalImpulseListenerAmplitude;
        ImpulseListener.m_ReactionSettings.m_FrequencyGain = originalImpulseListenerFrequency;
        ImpulseListener.m_ReactionSettings.m_Duration = originalImpulseListenerDuration;
    }
}
