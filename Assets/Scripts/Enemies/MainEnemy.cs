using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class MainEnemy : MainScript
{
    [Tooltip("Max Y-Value Distance from Enemy start position.\nx: lowest\ny: highest")]
    [SerializeField]
    protected Vector2 m_BetweenYPositions;

    [Tooltip("Max Z-Value Distance from Enemy start position.\nx: lowest\ny: highest")]
    [SerializeField]
    protected Vector2 m_BetweenZPositions;

    protected float LowestYPosition
    {
        get
        {
            return StartPosition.y - m_BetweenYPositions.x;
        }
    }

    protected float HighestYPosition
    {
        get
        {
            return StartPosition.y + m_BetweenYPositions.y;
        }
    }

    protected float LowestZPosition
    {
        get
        {
            return StartPosition.z - m_BetweenZPositions.x;
        }
    }

    protected float HighestZPosition
    {
        get
        {
            return StartPosition.z + m_BetweenZPositions.y;
        }
    }

    public abstract GroundType Type { get; }

    protected bool GotHitLastFrame { get; private set; }

    [SerializeField]
    protected float m_Health;
    [SerializeField] 
    private MoveDirection m_MoveDirection;
    [SerializeField]
    [Tooltip("Speed in Z and Y Direction")]
    protected Vector2 m_Speed;

    public int Index { get; private set; }
    protected bool CheckCollisionInFixed { get; set; }

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
        GotHitLastFrame = false;

        Move();

        base.Update();
    }

    public virtual void FixedUpdate()
    {
        if (CheckCollisionInFixed)
            DamageToPlayer();
        CheckCollisionInFixed = false;
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

    /// <summary>
    /// Move object upwards
    /// </summary>
    private void Up()
    {
        RBody.velocity = new Vector3(
            RBody.velocity.x,
            m_Speed.y,
            RBody.velocity.z
            );

        if (this.transform.position.y >= HighestYPosition)
        {
            // Remove upwards direction from current direction
            CurrentDirection &= ~MoveDirection.UP;

            // Add downwards direction to current direction
            CurrentDirection = CurrentDirection | MoveDirection.DOWN;
        }
    }

    /// <summary>
    /// Move object down
    /// </summary>
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

    /// <summary>
    /// Move object to the left
    /// </summary>
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


    /// <summary>
    /// Move object to the right
    /// </summary>
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
        GotHitLastFrame = true;

        if (m_Health <= 0) Die();
    }

    private void Die()
    {
        EnemyManager.Get.RemoveEnemy(Index);
        Destroy(this.gameObject);
    }

    private void DamageToPlayer()
    {
        if (GotHitLastFrame) return;

        Debug.Log("OUCH");
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == PlayerManager.Get.Player.gameObject)
        {
            CheckCollisionInFixed = true;
        }
    }




    public enum GroundType
    {
        GROUND,
        AIR
    }
}
