using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Movement : MonoBehaviour
{
    public float maxMagnitude;
    public float movementForce;
    public float maxJumpForce;
    public float preventativeForce;
    public float impactThreshold;
    public float jumpChargeTime;

    public AudioClip clip;
    public BoxCollider2D floor;

    private Vector2 prevVelocity;
    private float timeOfLastImpact;
    private float timeOfLastFloorTouch;
    private float timeOfLastCharge;
    private bool jumpCharging;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        timeOfLastImpact = Time.time * 100;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        if (collider.IsTouching(floor))
        {
            timeOfLastFloorTouch = Time.time;
        }

        if (Input.GetKeyDown("space"))
        {
            timeOfLastCharge = Time.time;
            jumpCharging = true;
        }

        if (Input.GetKey("space") && jumpCharging)
        {
            gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, getDestinationScale(), ref velocity, jumpChargeTime);
        }

        if (Input.GetKeyUp("space"))
        {
            if (Time.time - timeOfLastFloorTouch < 0.1)
            {
                float jumpForce = maxJumpForce * (Time.time - timeOfLastCharge) / jumpChargeTime;
                jumpForce = jumpForce > maxJumpForce ? maxJumpForce : jumpForce;

                body.AddForce(new Vector2(0, jumpForce));
            }

            jumpCharging = false;
        }

        if (!jumpCharging)
        {
            Vector3 originalScale = new Vector3(1f, 1f, 1f);
            gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, originalScale, ref velocity, 0.1f);
        }

        if (body.velocity.x < maxMagnitude)
        {
            float newMovementForce = collider.IsTouching(floor) ? movementForce : movementForce / 8;

            if (Input.GetKey("a"))
            {
                body.AddForce(new Vector2(-newMovementForce, -preventativeForce));
            }

            if (Input.GetKey("d"))
            {
                body.AddForce(new Vector2(newMovementForce, -preventativeForce));
            }
        }

        float impact = prevVelocity.magnitude - body.velocity.magnitude;

        if (impact > impactThreshold)
        {
            AudioSource audio = gameObject.GetComponent<AudioSource>();
            AudioMixer mixer = audio.outputAudioMixerGroup.audioMixer;

            audio.volume = impact / 5;

            float currentTime = Time.time * 100;
            float timeTakenToImpact = currentTime - timeOfLastImpact;
            float newPitch = 100 / (timeTakenToImpact > 100 ? 100 : timeTakenToImpact);
            newPitch = newPitch > 2 ? 1.9f : newPitch;

            mixer.SetFloat("pitch", Random.Range(-0.1f, 0.1f) + newPitch);
            timeOfLastImpact = currentTime;
            audio.PlayOneShot(clip);
        }

        prevVelocity = new Vector2(body.velocity.x, body.velocity.y);
    }

    Vector3 getDestinationScale()
    {
        float rotation = Mathf.Abs(gameObject.transform.rotation.eulerAngles.z - 180);

        if (rotation > 135 || rotation < 45)
        {
            return new Vector3(1.25f, 0.75f, 1.0f);
        }
        else
        {
            return new Vector3(0.75f, 1.25f, 1.0f);
        }
    }
}
