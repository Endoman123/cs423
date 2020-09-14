using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mesh;
    [SerializeField]
    private Collider collider;

    [SerializeField]
    private float respawnTime = 5f;

    private float respawnTimer = -1;

    // Update is called once per frame
    void Update()
    {
        if (respawnTimer > 0)
            respawnTimer = Mathf.Clamp(respawnTimer - Time.deltaTime, 0f, respawnTime);

        if (respawnTimer == 0)
            ResetPickup();
    }

    // Function to call to reset pickup object
    // This means to reset the timer, make physical, etc.
    public void ResetPickup() {
        respawnTimer = -1;
        SetPhysicality(true);
    }

    // Function to call on object pickup
    public void Pickup() {
        respawnTimer = respawnTime;
        SetPhysicality(false);
    }

    // Enable or disable the physicality of the pickup
    // i.e: it's physical existence in the game area
    private void SetPhysicality(bool physical) {
        mesh.SetActive(physical);
        collider.enabled = physical;
    }
}
