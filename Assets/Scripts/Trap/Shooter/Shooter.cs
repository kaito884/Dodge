using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrehub;
    [SerializeField] float shotInterval;
    [SerializeField] float delay;
    [SerializeField] float bulletVelocity;
    [SerializeField] bool isRandomInter;
    [SerializeField] bool isRandomDelay;
    [SerializeField] float maxRandInter;
    [SerializeField] float minRandInter;
    [SerializeField] float maxRandDelay;
    [SerializeField] float minRandDelay;

    private AudioSource audioSource;
    private ParticleSystem particle;

    private float timer = 0;

    private void Start()
    {
        if (isRandomDelay) delay = Random.Range(minRandDelay, maxRandDelay);
        if (isRandomInter) shotInterval = Random.Range(minRandInter, maxRandInter);
        timer -= delay;
        audioSource = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= shotInterval)
        {
            timer = 0;
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrehub);

        bullet.transform.position = transform.position;

        Rigidbody2D body = bullet.GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z) * bulletVelocity, Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z) * bulletVelocity);

        audioSource.Play();
        particle.Play();
    }
}
