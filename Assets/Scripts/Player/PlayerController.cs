using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 5.0f;
    public float floatiness = 2.0f;
    public float slamSpeed = 5.0f;
    public float bpm = 100f;

    public Transform spawnPoint;

    Rigidbody playerPhys;
    Collider playerCollider;
    Animator playerAnimator;
    Health health;
    private bool jumpHeld = false;
    private bool slideHeld = false;

    private OSCSendReceive osc;

    private void Awake()
    {
        playerPhys = this.GetComponent<Rigidbody>();
        playerCollider = this.GetComponent<Collider>();
        health = this.GetComponent<Health>();
        playerAnimator = this.GetComponentInChildren<Animator>();
        osc = GameObject.FindGameObjectWithTag("OSC").GetComponent<OSCSendReceive>();
    }

    private void Start()
    {
        playerPhys.velocity = this.transform.forward * speed;
        playerAnimator.SetBool("IsWalking", true);
        playerAnimator.speed = (bpm / 60f);
    }

    private void FixedUpdate()
    {
        if (jumpHeld)
        {
            Vector3 gravityForce = (-Vector3.up * Physics.gravity.magnitude) / floatiness;
            playerPhys.AddForce(gravityForce);
            
        }
    }

    private void Update()
    {
        if (!IsGrounded())
        {
            playerAnimator.SetBool("InAir", true);
        }
        else
        {
            playerAnimator.SetBool("InAir", false);
        }

        if (playerPhys.velocity.magnitude > 1f)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
            TakeDamage();
        }
    }

    public void Respawn()
    {
        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnPoint.rotation;
        health.ResetHealth();
        playerPhys.velocity = this.transform.forward * speed;
    }

    public void TakeDamage()
    {
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Damaged " + 1);
        health.Decrement();
    }

    public void Kill()
    {
        health.Die();
    }
    
    public void OnJump()
    {
        jumpHeld = !jumpHeld;
        if (jumpHeld && IsGrounded())
        {
            osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Jumped " + 1);
            playerPhys.velocity += this.transform.up * jumpHeight;
            playerPhys.useGravity = false;
        }
        else
        {
            playerPhys.useGravity = true;
        }
    }

    public void OnSlide()
    {
        slideHeld = !slideHeld;
        Vector3 colliderScale = playerCollider.transform.localScale;
        if (slideHeld)
        {
            colliderScale = new Vector3(colliderScale.x, colliderScale.y * 0.5f, colliderScale.z);
            playerCollider.transform.localScale = colliderScale;

            playerPhys.velocity -= this.transform.up * slamSpeed;
        }
        else
        {
            colliderScale = new Vector3(colliderScale.x, colliderScale.y * 2f, colliderScale.z);
            playerCollider.transform.localScale = colliderScale;
        }
    }

    public void updateBPM(float bpm)
    {
        this.bpm = bpm;
        playerAnimator.speed = (bpm / 60f); 
    }

    public void Beat()
    {
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Step " + 1);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, playerCollider.bounds.extents.y + 0.1f);
    }
}
