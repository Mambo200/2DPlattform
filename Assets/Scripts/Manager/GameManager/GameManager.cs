using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MainScript
{
    public Framerate m_Frames;
    private int lastMaxFrameRate;
    private bool lastCapInfo;

    public Camera MainCamera { get; private set; }

    /// <summary>
    /// Instance of GameManager
    /// </summary>
    private static GameManager instance;

    /// <summary>
    /// Get instance of GameManager
    /// </summary>
    public static GameManager Get
    {
        get
        {
            if (instance == null)
                instance = GetGameManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = GetGameManager();
        else
        {
            Debug.LogError("Gamemanager already exists. Deleting...");
            Destroy(this.gameObject);
        }
    }

    public override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        m_Frames.SetMaxFrameRate(this);
#else
        m_Frames.SetMaxFrameRate();
#endif
        lastMaxFrameRate = m_Frames.MaxFrameCount;
        lastCapInfo = m_Frames.CapFrames;

        MainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        if (lastMaxFrameRate != m_Frames.MaxFrameCount
            || lastCapInfo != m_Frames.CapFrames)
            m_Frames.SetMaxFrameRate();

        lastMaxFrameRate = m_Frames.MaxFrameCount;
        lastCapInfo = m_Frames.CapFrames;

        Debug.Log(m_Frames.GetCurrentFrameRate());
    }

    /// <summary>
    /// Get gamemanager
    /// </summary>
    /// <returns>Gamemanager</returns>
    private static GameManager GetGameManager()
    {
        return FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Set FrameRate
    /// </summary>
    /// <param name="_newFPS">New FrameRate</param>
    public void SetMaxFrameRate(int _newFPS)
    {
        Application.targetFrameRate = _newFPS;
    }

    public int GetMaxFrameRate()
    {
        return Application.targetFrameRate;
    }
}
