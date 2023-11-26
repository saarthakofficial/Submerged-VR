using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    [SerializeField] float swimForce = 2f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce;
    [SerializeField] float minTimeBetweenStrokes;
    [SerializeField] float maxSwimHeight = 5f; // Adjust this value to set the maximum swim height
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference leftControllerVelocity;
    [SerializeField] InputActionReference rightControllerSwimReference;
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] Transform trackingTransform;
    Rigidbody rb;
    [SerializeField] DynamicMoveProvider moveProvider;
    [SerializeField] CharacterController cc;
    [SerializeField] CapsuleCollider collider;
    float cooldownTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
            if (transform.position.y < maxSwimHeight){
                rb.useGravity = false;
            }
            else{
                rb.useGravity = true;
            }

            cooldownTimer += Time.fixedDeltaTime;
            if (cooldownTimer > minTimeBetweenStrokes && leftControllerSwimReference.action.IsPressed() && rightControllerSwimReference.action.IsPressed())
            {
                var leftHandVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
                var rightHandVelocity = rightControllerVelocity.action.ReadValue<Vector3>();
                Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
                localVelocity *= -1;

                if (localVelocity.sqrMagnitude > minForce * minForce)
                {
                    Vector3 worldVelocity = trackingTransform.TransformDirection(localVelocity);

                    // Separate the horizontal and vertical components
                    Vector3 horizontalVelocity = new Vector3(worldVelocity.x, 0, worldVelocity.z);
                    Vector3 verticalVelocity = new Vector3(0, worldVelocity.y, 0);

                    // Check if the player is below the maximum swim height for vertical movement
                    if (transform.position.y < maxSwimHeight)
                    {
                        rb.AddForce(verticalVelocity * swimForce, ForceMode.Acceleration);  
                    }


                    // Always apply the vertical force for buoyancy
                        rb.AddForce(horizontalVelocity * swimForce, ForceMode.Acceleration);

                    cooldownTimer = 0f;
                }
            }

            if (rb.velocity.sqrMagnitude > 0.01f)
            {
                rb.AddForce(-rb.velocity * dragForce, ForceMode.Acceleration);
            }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Surface"){
            SurfaceControl();
        }
        if (other.gameObject.tag == "Dive"){
            UnderwaterControl();
        }
    }

    void SurfaceControl(){
        rb.useGravity = false;
        cc.enabled = true;
        collider.isTrigger = true;
        moveProvider.enabled = true;
    }

    void UnderwaterControl(){
        cc.enabled = false;
        collider.isTrigger = false;
        moveProvider.enabled = false;
        rb.useGravity = true;
    }
}
