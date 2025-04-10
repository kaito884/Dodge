using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private Vector2 initVelocity;
    [SerializeField] private bool randomize;
    [SerializeField] private float maxRandom;
    [SerializeField] private float minRandom;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acelerateSpeed;
    [SerializeField] private Vector2 impulseDirection;
    [SerializeField] private LayerMask impulseLayerMask;

    CinemachineImpulseSource impulse;
    private Rigidbody2D body;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        impulse = FindObjectOfType<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        if (!randomize)
            body.velocity = initVelocity;
        else
        {
            body.velocity = new Vector2(Random.Range(minRandom, maxRandom), Random.Range(minRandom, maxRandom));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
        else
        {
            body.velocity += body.velocity.normalized * acelerateSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & impulseLayerMask) != 0)
        {
            impulse.GenerateImpulse(impulseDirection);
            SoundManager.Instance.PlaySE(SESoundData.SE.SpikeBall, audioSource);
        }
    }
}
