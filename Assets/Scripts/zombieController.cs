using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public GameObject flipModel;
    public AudioClip[] idleSounds;
    public float idleSoundTime;
    AudioSource enemyMovementAS;
    float nextIdleSound = 0f;
    public float detectionTime;
    float startRun;
    bool firstDetection;
    public float runSpeed;
    public float walkSpeed;
    public bool facingRight = true;
    float moveSpeed;
    bool running;
    Rigidbody myRB;
    Animator myAnim;
    Transform detectedPlayer;
    bool Detected;
    // Use this for initialization
    void Start()
    {
        myRB = GetComponentInParent<Rigidbody>();
        myAnim = GetComponentInParent<Animator>();
        enemyMovementAS = GetComponent<AudioSource>();
        running = false;
        Detected = false;
        firstDetection = false;
        moveSpeed = walkSpeed;
        if (Random.Range(0, 10) > 5) Flip();
    }
    void FixedUpdate()
    {
        if (Detected)
        {
            if (detectedPlayer.position.x < transform.position.x && facingRight) Flip();
            else if (detectedPlayer.position.x > transform.position.x && !facingRight) Flip();
            if (!firstDetection)
            {
                startRun = Time.time + detectionTime;
                firstDetection = true;
            }
        }
        if (Detected && !facingRight) myRB.velocity = new Vector3((moveSpeed * -1), myRB.velocity.y, 0);
        else if (Detected && facingRight) myRB.velocity = new Vector3(moveSpeed, myRB.velocity.y, 0);
        if (!running && Detected)
        {
            if (startRun < Time.time)
            {
                moveSpeed = runSpeed;
                myAnim.SetTrigger("run");
                running = true;
            }
        }
        if (!running)
        {
            if (Random.Range(0, 10) > 5 && nextIdleSound < Time.time)
            {
                AudioClip tempClip = idleSounds[Random.Range(0, idleSounds.Length)];
                enemyMovementAS.clip = tempClip;
                enemyMovementAS.Play();
                nextIdleSound = idleSoundTime + Time.time;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Detected)
        {
            Detected = true;
            detectedPlayer = other.transform;
            myAnim.SetBool("detected", Detected);
            if (detectedPlayer.position.x < transform.position.x && facingRight) Flip();
            else if (detectedPlayer.position.x > transform.position.x && !facingRight) Flip();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            firstDetection = false;
            if (running)
            {
                myAnim.SetTrigger("run");
                moveSpeed = walkSpeed;
                running = false;
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = flipModel.transform.localScale;
        theScale.z *= -1;
        flipModel.transform.localScale = theScale;
    }
}
