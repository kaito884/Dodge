using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float liveTime;

    [SerializeField] private TagCheck playerHit;
    [SerializeField] private TagCheck groundHit;

    private float timer = 0;

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
        
        timer += Time.deltaTime;
        if (playerHit.IsHit() || groundHit.IsHit() || timer > liveTime) 
            Destroy(this.gameObject);

    }
}
