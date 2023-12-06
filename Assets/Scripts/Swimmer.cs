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
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioLowPassFilter lowPassFilter;
    [SerializeField] AudioSource ambience;
    [SerializeField] AudioClip surfaceAmbience;
    [SerializeField] AudioClip underwaterAmbience;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip dive;
    float cooldownTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
            if (transform.position.y < maxSwimHeight){
                rb.useGravity = false;
                ambience.clip = underwaterAmbience;
                ambience.volume = 0.85f;
                lowPassFilter.cutoffFrequency = 4000;
            }
            else{
                rb.useGravity = true;
                ambience.clip = surfaceAmbience;
                ambience.volume = 0.75f;
                lowPassFilter.cutoffFrequency = 10000;
            }
            if (!ambience.isPlaying){
                ambience.Play();
            }
            if ((transform.position.y > maxSwimHeight - 1f) && (transform.position.y < maxSwimHeight + 1f) && !sfx.isPlaying){
                sfx.PlayOneShot(dive);
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

                    Vector3 horizontalVelocity = new Vector3(worldVelocity.x, 0, worldVelocity.z);
                    Vector3 verticalVelocity = new Vector3(0, worldVelocity.y, 0);

                    if (transform.position.y < maxSwimHeight)
                    {
                        rb.AddForce(verticalVelocity * swimForce, ForceMode.Acceleration);  
                    }

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



    public void SurfaceControl(){
        rb.useGravity = false;
        cc.enabled = true;
        collider.isTrigger = true;
        moveProvider.enabled = true;
    }

    public void UnderwaterControl(){
        cc.enabled = false;
        collider.isTrigger = false;
        moveProvider.enabled = false;
        rb.useGravity = true;
    }
}
