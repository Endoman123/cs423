using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float rollForce = 3f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private float penaltyTime = 3f;
    
    // Events that the GameController will handle
    private UnityEvent eAddPoint = new UnityEvent(), eSubPoint = new UnityEvent();
    
    private Rigidbody rb;
    private bool grounded = true;
    private bool jump = false;
    private Vector3 vec = new Vector3();

    private float penaltyTimer = -1f;

    // This is definitely not a gucci way to do multiple players
    private int index = -1;

    // Use start to get the needed components
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Reset player's velocity and rotation
    public void ResetPlayer() {
        if (rb != null) {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity; 
        }
    }

    // Subscribe actions to player's events
    public void AddListeners(UnityAction add, UnityAction sub) {
        eAddPoint.AddListener(add);
        eSubPoint.AddListener(sub);
    }

    // Pickup pickups
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.GetComponent<PickupControllerScript>().Pickup();
            eAddPoint.Invoke();
        }
    }

    // Give penalties
    private void OnCollisionEnter(Collision other) {
        // Penalize the player for touching someone
        // but give them some invincibility time instead of wiping their score immediately
        if (other.gameObject.CompareTag("Player") && 
            other.transform.position.y < transform.position.y && 
            penaltyTimer <= 0f) {
            Debug.Log("Touching");
            eSubPoint.Invoke();
            penaltyTimer = penaltyTime;
        }
    }

    // Use this for physics-based calculations
    // I guess ground checking belongs here?
    private void FixedUpdate() {
        // Basic rolling movement
        rb.AddForce(vec * rollForce);

        // Jump force will be applied separately
        // This should also flip the grounded and jump bools to false immediately after
        if (jump) {
            jump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Figure out if we are touching the ground;
        // Only consider ourselves grounded if we are touching anything tagged as such.
        // Raycast needs to start inside the player, helps the raycast detect the ground underneath if it slightly coincides
        // with the ground.
        grounded = Physics.Raycast(transform.position + Vector3.down * 0.45f, Vector3.down, 0.055f, 1 << 8);
        
        // Keep us slightly off ground to make sure the raycast be working
        if (grounded)
            rb.AddForce(Vector3.up * 0.01f);
    }

    // Use this for all input
    // as well as updating the penalty timer
    private void Update() {
        // Rather than deallocate and reallocate,
        // just use the same vector
        vec.Set(Input.GetAxis($"P{index} Horizontal"), 0.0f, Input.GetAxis($"P{index} Vertical"));

        if (!jump)
            jump = grounded && Input.GetButtonDown($"P{index} Jump");

        if (penaltyTimer > 0)
            penaltyTimer = Mathf.Clamp(penaltyTimer - Time.deltaTime, 0f, penaltyTime);
    }

    public int Index {
        set {
            index = value;
        }
    }
}
