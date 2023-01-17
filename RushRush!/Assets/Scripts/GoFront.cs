using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFront : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float maxDelay;

    private float curDelay;

    private void Update()
    {
        curDelay += Time.deltaTime;
        if(curDelay > maxDelay)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
