using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MainScript
{
    /// <summary>
    /// Rigidbody of this Object
    /// </summary>
    private Rigidbody RBody { get; set; }

    /// <summary>
    /// Collider of this Object
    /// </summary>
    private Collider Collider { get; set; }

    /// <summary>
    /// Movementspeed of Player
    /// </summary>
    public float m_MovementSpeed;

    /// <summary>
    /// Jump velocity of Player
    /// </summary>
    public float m_JumpSpeed;

    /// <summary>
    /// Dash is allowed
    /// </summary>
    [Header("Dash Variables")]
    public bool m_DashAllowed;

    /// <summary>
    /// Multiplier of Dash speed (add to <see cref="m_MovementSpeed"/>)
    /// </summary>
    public float m_DashSpeedMultiplier;

    /// <summary>
    /// Duration of Dash
    /// </summary>
    public float m_DashDuration;

    /// <summary>
    /// Player can Jump in the Air once while dash
    /// </summary>
    [SerializeField]
    private bool m_JumpInAirWhileDash;

    /// <summary>
    /// Player already jumped in the air
    /// </summary>
    private bool jumpedInAir;

    /// <summary>
    /// Player is currently Dashing
    /// </summary>
    public bool InDash { get; private set; }

    /// <summary>
    /// When Dash beginns, value will be set to <see cref="m_DashDuration"/>. 
    /// If this hits 0, Player stopps dashing 
    /// (for more information, see <see cref="GetDash(float)"/>)
    /// </summary>
    private float tempDashTime;

    /// <summary>
    /// true if player is grounded. should be set at the beginning of Update Function
    /// </summary>
    public bool Grounded { get; private set; }

    // Start is called before the first frame update
    public override void Start()
    {
        RBody = GetComponent<Rigidbody>();
        Collider = GetComponent<MeshCollider>();
        Respawnmanager.Get.AddItem(
            this.gameObject,
            () =>
            {
                tempDashTime = 0;
                jumpedInAir = false;
                InDash = false;
            }
            );
    }

    // Update is called once per frame
    public override void Update()
    {
        Grounded = IsGrounded();
        Vector3 toMove = new Vector3(
            0, 
            0,
            GetMovement(m_DashAllowed) * m_MovementSpeed
            );
        Move(toMove);
        Jump();
        VelocityCheck();
    }

    /// <summary>
    /// Moves the Player to x, y and z Axis.
    /// </summary>
    /// <param name="_direction">Direction to Move</param>
    private void Move(Vector3 _direction)
    {
        Vector3 velocity = _direction;
        velocity.y += RBody.velocity.y;
        RBody.velocity = velocity;
    }

    /// <summary>
    /// Get horizontal Input (A and D) and calculate with Dash
    /// </summary>
    /// <returns>Horizontal input</returns>
    private float GetMovement(bool _dashAllowed)
    {
        float toReturn = Input.GetAxisRaw("Horizontal");
        if (_dashAllowed) toReturn = GetDash(toReturn);
        return toReturn;
    } 

    /// <summary>
    /// Get horizontal Dash.
    /// </summary>
    /// <param name="_currentMovement">current Movement of Player</param>
    /// <returns></returns>
    private float GetDash(float _currentMovement)
    {
        // Check if Player is NOT Dashing, if Player is moving and Player pressed the Dash-Button
        if(!InDash && _currentMovement != 0  && Input.GetAxisRaw("Dash") != 0)
        {
            // Start dashing
            _currentMovement *= m_DashSpeedMultiplier;
            tempDashTime = m_DashDuration;
            DashBeginn();
        }
        else if (InDash)
        {
            _currentMovement *= m_DashSpeedMultiplier;
            DashStay();
            if (tempDashTime <= 0 && Grounded)
            {
                DashEnd();
            }
        }

        return _currentMovement;
    }

    /// <summary>
    /// Add Jump velocity if Jump key is pressed
    /// </summary>
    private void Jump()
    {
        if(Grounded || (InDash && m_JumpInAirWhileDash && !jumpedInAir))
            if (Input.GetButton("Jump"))
            {
                RBody.velocity = new Vector3(
                    RBody.velocity.x,
                    m_JumpSpeed,
                    RBody.velocity.z
                    );

                if (!jumpedInAir) DashJump();
                jumpedInAir = true;
            }
    }

    /// <summary>
    /// Check if Velocity is higher than is should be
    /// </summary>
    private void VelocityCheck()
    {
        if (RBody.velocity.y >= 10)
        {

            RBody.velocity = new Vector3(
                RBody.velocity.x,
                10,
                RBody.velocity.z
                );
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    /// <summary>
    /// Check if Player is on the ground
    /// </summary>
    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            -Vector3.up,
            (Collider.bounds.extents.y + 0.1f)
            );
    }





    #region Dash Methods
    private void DashBeginn()
    {
        InDash = true;
    }

    private void DashStay()
    {
        tempDashTime -= Time.deltaTime;
    }

    private void DashJump()
    {

    }

    private void DashEnd()
    {
        InDash = false;
        jumpedInAir = false;
    }
    #endregion

    private void OnDestroy()
    {
        MainRespawnManager.Get.RemoveItem(this.gameObject);
    }
}
