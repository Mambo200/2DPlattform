using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Framerate
{
    [SerializeField]
    [Range(30, 180)]
    [Min(30)]
    [Tooltip("Die maximal Anzahl an Frames die das Spiel haben darf (Sofern \"Cap Frames\" ausgeschaltet ist)")]

    private int m_MaxFrameCount;
    [SerializeField]
    [Tooltip(
        "Ist diese Option ausgeschaltet geht die FPS-Zahl so hoch wie das Gerät stark ist.\n" +
        "Ist diese Option eingeschaltet richtet sich er FPS-Wert an die maximale Anzahl an \"Max Frame Count\""
        )]
    private bool m_CapFrames;

    public int MaxFrameCount
    {
        get => m_MaxFrameCount;

        set
        {
            m_MaxFrameCount = value;
            if (m_CapFrames)
                SetMaxFrameRate(m_MaxFrameCount);
        }
    }

    public bool CapFrames
    {
        get => m_CapFrames;

        set
        {
            m_CapFrames = value;
            if (m_CapFrames)
                SetMaxFrameRate(m_MaxFrameCount);
            else
                SetMaxFrameRate(-1);
        }
    }

    public Framerate(int _maxFrames, bool _capFrames)
    {
        m_MaxFrameCount = _maxFrames;
        CapFrames = _capFrames;
    }
    //
    /// <summary>
    /// Set FrameRate
    /// </summary>
    /// <param name="_newFPS">New FrameRate</param>
    private void SetMaxFrameRate(int _newFPS)
    {
        Application.targetFrameRate = _newFPS;
    }

    public void SetMaxFrameRate() => Start();

    private void Start()
    {
        if (m_CapFrames)
            SetMaxFrameRate(m_MaxFrameCount);
        else
            SetMaxFrameRate(-1);
    }

    public int GetCurrentFrameRate() => (int) ((1.0f / Time.deltaTime + 0.5f) + 0.5f);

#if UNITY_EDITOR
    private void Start(MonoBehaviour _source)
    {
        Start();
        Debug.Log("Framerate changed by Class \"" + _source.GetType() + "\"", _source);
    }


    public void SetMaxFrameRate(MonoBehaviour _source) => Start(_source);
#endif

}
