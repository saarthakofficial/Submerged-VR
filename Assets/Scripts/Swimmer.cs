using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    [SerializeField] float swimForce = 2f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce;
    [SerializeField] float minTimeBetweenStrokes;
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference leftControllerVelocity;
    [SerializeField] InputActionReference rightControllerSwimReference;
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] Transform trackingTransform;
    Rigidbody rb;
    float cooldownTimer;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate(){
        cooldownTimer += Time.fixedDeltaTime;
        if (cooldownTimer > minTimeBetweenStrokes && leftControllerSwimReference.action.IsPressed() && rightControllerSwimReference.action.IsPressed()){
            var leftHandVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
            var rightHandVelocity = rightControllerVelocity.action.ReadValue<Vector3>();
            Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
            localVelocity *= -1;

            if (localVelocity.sqrMagnitude > minForce * minForce){
                Vector3 worldVelocity = trackingTransform.TransformDirection(localVelocity);
                rb.AddForce(worldVelocity * swimForce, ForceMode.Acceleration);
                cooldownTimer = 0f;
            }
        }

        if (rb.velocity.sqrMagnitude > 0.01f){
            rb.AddForce(-rb.velocity * dragForce, ForceMode.Acceleration);
        }
    }

}
