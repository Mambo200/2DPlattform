using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnmanager : MainRespawnManager
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if (instance != null && instance.GetType() == this.GetType())
        {
            Debug.Log("there is already an instance of " + ToString() + ". Destroy this");
            Destroy(this);
            return;
        }

        instance = GameObject.FindObjectOfType<Respawnmanager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
