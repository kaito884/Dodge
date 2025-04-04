using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrehub;
    [SerializeField] float shotInterval;
    [SerializeField] float delay;
    [SerializeField] float bulletVelocity;
        
    private AudioSource audioSource;

    private float timer = 0;

    private void Start()
    {
        timer -= delay;
        audioSource = GetComponent<AudioSource>();
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
    }
}
