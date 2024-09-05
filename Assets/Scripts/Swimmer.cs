using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] float maxSpeed;
    [SerializeField] float minTimeBetweenStrokes;
    [SerializeField] float maxSwimHeight = 5f;
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference leftControllerVelocity;
    [SerializeField] InputActionReference rightControllerSwimReference;
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] InputActionReference bButtonReference;
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
    public bool grabbing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < maxSwimHeight)
        {
            rb.useGravity = false;
            ambience.clip = underwaterAmbience;
            ambience.volume = 0.85f;
            lowPassFilter.cutoffFrequency = 4000;
        }
        else
        {
            rb.useGravity = true;
            ambience.clip = surfaceAmbience;
            ambience.volume = 0.75f;
            lowPassFilter.cutoffFrequency = 10000;
        }
        if (!ambience.isPlaying)
        {
            ambience.Play();
        }
        if ((transform.position.y > maxSwimHeight - 1f) && (transform.position.y < maxSwimHeight + 1f) && !sfx.isPlaying)
        {
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
        Debug.Log(rb.velocity.magnitude);
        // rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            rb.AddForce(-rb.velocity * dragForce, ForceMode.Acceleration);
        }

        if (GameManager.instance.map != null && grabbing == false)
        {
            if (bButtonReference.action.IsPressed())
            {
                if (!GameManager.instance.map.activeInHierarchy)
                {
                    GameManager.instance.map.SetActive(true);
                }
            }
            else
            {
                GameManager.instance.map.SetActive(false);
            }
        }

    }

    public void DelayedFalsifyGrabbing()
    {
        Invoke("FalsifyGrabbing", 1.5f);
    }

    public void FalsifyGrabbing()
    {
        grabbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Surface")
        {
            SurfaceControl();
        }
        if (other.gameObject.tag == "Dive")
        {
            UnderwaterControl();
        }

    }



    public void SurfaceControl()
    {
        rb.useGravity = false;
        cc.enabled = true;
        collider.isTrigger = true;
        moveProvider.enabled = true;
    }

    public void UnderwaterControl()
    {
        cc.enabled = false;
        collider.isTrigger = false;
        moveProvider.enabled = false;
        rb.useGravity = true;
    }
}
