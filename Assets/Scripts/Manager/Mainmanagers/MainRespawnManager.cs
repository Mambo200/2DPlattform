using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MainRespawnManager : MainScript
{
    protected static MainRespawnManager instance;

    public static MainRespawnManager Get { get => instance; }

    /// <summary>
    /// All Objects which shall do something after Object is set to new position
    /// </summary>
    private Dictionary<GameObject, UnityEvent> ObjectsToCheck { get; set; }

    /// <summary>
    /// Respawn position
    /// </summary>
    public Vector3 m_RespawnPosition;

    /// <summary>
    /// Y Position for Deah
    /// </summary>
    public float m_DeathPosition;

    protected virtual void Awake()
    {
        ObjectsToCheck = new Dictionary<GameObject, UnityEvent>();
    }
    // Start is called before the first frame update
    public override void Start()
    {


    }

    // Update is called once per frame
    public override void Update()
    {
        Check();
    }

    /// <summary>
    /// Checks each Item in List. Call <see cref="Respawn(KeyValuePair{GameObject, UnityEvent})"/> if items y-Axis is too low
    /// </summary>
    protected void Check()
    {
        foreach (KeyValuePair<GameObject, UnityEvent> item in ObjectsToCheck)
        {
            if (item.Key.transform.position.y < m_DeathPosition)
                Respawn(item);
        }

    }

    /// <summary>
    /// Set GameObject to new position and Call Event
    /// </summary>
    /// <param name="_obj">object to respawn</param>
    protected virtual void Respawn(KeyValuePair<GameObject, UnityEvent> _obj)
    {
        _obj.Key.gameObject.transform.position = m_RespawnPosition;
        _obj.Value.Invoke();
    }

    /// <summary>
    /// Add Item to respawn list
    /// </summary>
    /// <param name="_toRespawn">Object to respawn</param>
    /// <param name="_toInvoke">Actions to call after Respawn</param>
    public void AddItem(GameObject _toRespawn, params Action[] _toInvoke)
    {
        UnityEvent ev = new UnityEvent();

        foreach (Action action in _toInvoke)
        {
            ev.AddListener(() => action());
        }

        ObjectsToCheck.Add(_toRespawn, ev);
    }

    /// <summary>
    /// Remove Item from list
    /// </summary>
    /// <param name="_toRemove">Item to remove</param>
    /// <returns></returns>
    public bool RemoveItem(GameObject _toRemove)
    {
        return ObjectsToCheck.Remove(_toRemove);
    }
}
