using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MainRespawnManager : MonoBehaviour
{
    protected static MainRespawnManager instance;

    public static MainRespawnManager Get { get => instance; }

    private Dictionary<GameObject, UnityEvent> ObjectsToCheck { get; set; }
    public Vector3 m_RespawnPosition;
    public float m_DeathPosition;

    protected virtual void Awake()
    {
        ObjectsToCheck = new Dictionary<GameObject, UnityEvent>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Check();
    }

    protected void Check()
    {
        foreach (KeyValuePair<GameObject, UnityEvent> item in ObjectsToCheck)
        {
            if (item.Key.transform.position.y < m_DeathPosition)
                Respawn(item);
        }

    }

    protected virtual void Respawn(KeyValuePair<GameObject, UnityEvent> _obj)
    {
        _obj.Key.gameObject.transform.position = m_RespawnPosition;
        _obj.Value.Invoke();
    }

    public void AddItem(GameObject _toRespawn, params Action[] _toInvoke)
    {
        UnityEvent ev = new UnityEvent();

        foreach (Action action in _toInvoke)
        {
            ev.AddListener(() => action());
        }

        ObjectsToCheck.Add(_toRespawn, ev);
    }

    public bool RemoveItem(GameObject _toRemove)
    {
        return ObjectsToCheck.Remove(_toRemove);
    }
}
