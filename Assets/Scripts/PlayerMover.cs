using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MainScript
{
    private Rigidbody RBody { get; set; }
    private Collider Collider { get; set; }

    public float m_MovementSpeed;
    public float m_JumpSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        RBody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    public override void Update()
    {
        float movement = GetMovement() * m_MovementSpeed;
        Vector3 toMove = Jump(movement);
        Move(toMove);
        Debug.Log(RBody.velocity);
    }

    private void Move(Vector3 _direction)
    {
        Vector3 velocity = _direction;
        velocity.y = RBody.velocity.y;
        RBody.velocity = velocity;
    }

    private float GetMovement() => Input.GetAxisRaw("Horizontal");

    private Vector3 Jump(float _movement)
    {
        bool isGrounded = IsGrounded();
        Vector3 toReturn = new Vector3(0, 0, _movement);
        if (Input.GetButton("Jump") && isGrounded)
        {
            //RBody.AddForce(
            //    Vector3.up * m_JumpSpeed,
            //    ForceMode.VelocityChange
            //    );
            RBody.AddExplosionForce(
                m_JumpSpeed,
                this.transform.position - new Vector3(0,Collider.bounds.extents.y, 0),
                3f,
                10f,
                ForceMode.VelocityChange
                );

            if (RBody.velocity.y >= 10)
                RBody.velocity = new Vector3(
                    RBody.velocity.x,
                    10,
                    RBody.velocity.z
                    );

            Debug.Log(this.transform.position - new Vector3(0, (this.transform.position.y - Collider.bounds.extents.y), 0));
        }



        return toReturn;
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            -Vector3.up,
            (Collider.bounds.extents.y + 0.1f)
            );
    }
}
