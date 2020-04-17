using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MainScript 
{
    private static PlayerManager instance;
    public static PlayerManager Get
    {
        get
        {
            if (instance == null)
                instance = GetInstance();
            return instance;
        }
    }

    [HideInInspector]
    public PlayerMover Player;

    // Start is called before the first frame update
    public override void Start()
    {
        if(instance == null)
        {
            instance = GetInstance();
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    static PlayerManager GetInstance()
    {
        return GameObject.FindObjectOfType<PlayerManager>();
    }

    public static void ReloadInstance()
    {
        instance = GetInstance();
    }
}
