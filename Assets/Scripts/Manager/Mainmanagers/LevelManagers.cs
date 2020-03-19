using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManagers : MonoBehaviour
{
    #region Singleton 
    static LevelManagers instance;
    public static LevelManagers Get
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("LevelManager").GetComponent<LevelManagers>();
            return instance;
        }
    }
    #endregion

    #region Public variables
    [Header("Camera Variables")]
    public float m_MaxYPos;
    public float m_MinYPos;
    #endregion

    #region Private variables
    [SerializeField]
    private bool m_CameraProtectionActive;
    #endregion

    #region Properties
    PlayerMover Player { get; set; }
    Camera MainCamera { get; set; }
    #endregion

    #region Protected Virtual Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        MainCamera = Camera.main;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (m_CameraProtectionActive) CameraProtection();
    }
    #endregion




    #region Protected Methods
    protected PlayerMover GetPlayer()
    {
        if (Player != null) return Player;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        return Player;
    }

    protected Camera GetCamera()
    {
        if (MainCamera != null) return MainCamera;

        MainCamera = Camera.main;
        return MainCamera;
    }
    #endregion

    #region Public Methods
    public void ResetPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
    }

    public void ResetCamera()
    {
        MainCamera = Camera.main;
    }
    #endregion

    #region Private Methods
    protected virtual void CameraProtection()
    {
        if (GetCamera().transform.position.y <= m_MinYPos)
            GetCamera().transform.position = new Vector3(
                GetCamera().transform.position.x,
                m_MinYPos,
                GetCamera().transform.position.z
                );

        else if (GetCamera().transform.position.y >= m_MaxYPos)
            GetCamera().transform.position = new Vector3(
                GetCamera().transform.position.x,
                m_MaxYPos,
                GetCamera().transform.position.z
                );
    }
    #endregion
}
