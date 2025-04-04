using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private Vector2 initVelocity;
    [SerializeField] private bool randomize;
    [SerializeField] private float maxRandom;
    [SerializeField] private float minRandom;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acelerateSpeed;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
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
}
