﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_DistanceToPlayer;
    public float m_YPositionAbovePlayer;

    private GameObject Player { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPosition(CalculateNewCameraPosition());
    }

    private Vector3 CalculateNewCameraPosition()
    {
        Vector3 newCamPos = new Vector3();
        newCamPos.x = m_DistanceToPlayer;
        newCamPos.y = Player.transform.position.y + m_YPositionAbovePlayer;
        newCamPos.z = Player.transform.position.z;

        return newCamPos;
    }

    private void SetCameraPosition(Vector3 _newPosition)
    {
        this.transform.position = _newPosition;
    }
}
