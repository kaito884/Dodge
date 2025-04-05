using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    private Transform player;
    private float timer = 0;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        player = FindObjectOfType<PlayerMov>().transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = player.position - gun.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

        timer += Time.fixedDeltaTime;
        float offsetY = Mathf.Sin(timer * frequency) * amplitude;
        transform.position = startPosition + new Vector3(0f, offsetY, 0f);
        print(offsetY);

    }
}
