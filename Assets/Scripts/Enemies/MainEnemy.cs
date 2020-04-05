﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class MainEnemy : MainScript
{
    [SerializeField]
    protected float m_Health;
    [SerializeField] 
    private MoveDirection m_MoveDirection;
    [SerializeField]
    [Tooltip("Speed in Z and Y Direction")]
    protected Vector2 m_Speed;

    public int Index { get; private set; }

    public MoveDirection Movedirection { get => m_MoveDirection; protected set => m_MoveDirection = value; }
    protected MoveDirection CurrentDirection { get; set; }
    public Vector3 StartPosition { get; private set; }
    protected Rigidbody RBody { get; private set; }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        StartPosition = this.gameObject.transform.position;
        SetCurrentDirection();
        RBody = GetComponent<Rigidbody>();

        Index = EnemyManager.Get.AddEnemy(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void SetCurrentDirection()
    {
        CurrentDirection = Movedirection;

        if (CurrentDirection.HasFlag(MoveDirection.UP) && CurrentDirection.HasFlag(MoveDirection.DOWN))
            CurrentDirection &= ~MoveDirection.DOWN;

        if (CurrentDirection.HasFlag(MoveDirection.LEFT) && CurrentDirection.HasFlag(MoveDirection.RIGHT))
            CurrentDirection &= ~MoveDirection.RIGHT;
    }

    public float GetHealth() => m_Health;

    [System.Flags]
    public enum MoveDirection
    {
        NODIRECTION = 0,
        UP = 1 << 0,
        DOWN = 1 << 1,
        LEFT = 1 << 2,
        RIGHT = 1 << 3
    }

    public void GetDamage(float _damage)
    {
        m_Health -= _damage;

        if (m_Health <= 0) Die();
    }

    private void Die()
    {
        EnemyManager.Get.RemoveEnemy(Index);
        Destroy(this.gameObject);
    }
}
