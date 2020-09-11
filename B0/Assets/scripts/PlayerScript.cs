using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float rollForce = 3f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private Text scoreText;
    
    private Rigidbody rb;
    private bool grounded = true;
    private bool jump = false;
    private Vector2 move = new Vector2();
    private Vector3 vec = new Vector3();

    private int score = 0;

    // Use start to get the needed components
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateScore();
    }

    private void OnRoll(InputValue v) {
        move = v.Get<Vector2>();
    }

    private void OnJump() {
        if (!jump)
            jump = grounded;
    }

    // Use this for all input
    private void Update() {
        // Rather than deallocate and reallocate,
        // just use the same vector
        vec.Set(move.x, 0.0f, move.y);
    }

    // Pickup pickups
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("pickup")) {
            score++;
            other.gameObject.SetActive(false);
            UpdateScore();
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
        if (grounded) {
            rb.AddForce(Vector3.up * 0.01f);
        }
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + score.ToString();
    }
}
