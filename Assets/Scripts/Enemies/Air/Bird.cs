using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bird : MainEnemyAir
{

    [Tooltip("Max Y-Value Distance from Enemy start position.\nx: lowest\ny: highest")]
    [SerializeField]
    private Vector2 m_BetweenYPositions;

    [Tooltip("Max Z-Value Distance from Enemy start position.\nx: lowest\ny: highest")]
    [SerializeField]
    private Vector2 m_BetweenZPositions;

    private float LowestYPosition
    {
        get
        {
            return StartPosition.y - m_BetweenYPositions.x;
        }
    }

    private float HighestYPosition
    {
        get
        {
            return StartPosition.y + m_BetweenYPositions.y;
        }
    }

    private float LowestZPosition
    {
        get
        {
            return StartPosition.z - m_BetweenZPositions.x;
        }
    }

    private float HighestZPosition
    {
        get
        {
            return StartPosition.z + m_BetweenZPositions.y;
        }
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        Move();
    }

    private void Move()
    {
        if (CurrentDirection.HasFlag(MoveDirection.UP))
        {
            Up();
        }
        else if (CurrentDirection.HasFlag(MoveDirection.DOWN))
        {
            Down();
        }
        if (CurrentDirection.HasFlag(MoveDirection.LEFT))
        {
            Left();
        }
        else if (CurrentDirection.HasFlag(MoveDirection.RIGHT))
        {
            Right();
        }
    }

    private void Up()
    {
        RBody.velocity = new Vector3(
            RBody.velocity.x,
            m_Speed.y,
            RBody.velocity.z
            );

        if(this.transform.position.y >= HighestYPosition)
        {
            CurrentDirection &= ~MoveDirection.UP;
            CurrentDirection = CurrentDirection | MoveDirection.DOWN;
        }
    }
    private void Down()
    {
        RBody.velocity = new Vector3(
            RBody.velocity.x,
            -m_Speed.y,
            RBody.velocity.z
            );

        if (this.transform.position.y <= LowestYPosition)
        {
            CurrentDirection &= ~MoveDirection.DOWN;
            CurrentDirection = CurrentDirection | MoveDirection.UP;
        }
    }
    private void Left()
    {
        RBody.velocity = new Vector3(
            RBody.velocity.x,
            RBody.velocity.y,
            -m_Speed.x
            );

        if (this.transform.position.z <= LowestZPosition)
        {
            CurrentDirection &= ~MoveDirection.LEFT;
            CurrentDirection = CurrentDirection | MoveDirection.RIGHT;
        }
    }
    private void Right()
    {
        RBody.velocity = new Vector3(
            RBody.velocity.x,
            RBody.velocity.y,
            m_Speed.x
            );

        if (this.transform.position.z >= HighestZPosition)
        {
            CurrentDirection &= ~MoveDirection.RIGHT;
            CurrentDirection = CurrentDirection | MoveDirection.LEFT;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.collider.gameObject.tag != "Player") return;
        //
        //Vector3 hitPoint = collision.GetContact(0).point;
        //if (hitPoint.y <= collision.collider.gameObject.transform.position.y)
        //{
        //    GetDamage(1);
        //    collision.collider.gameObject.GetComponent<PlayerMover>().JumpBoost();
        //}
    }
}
