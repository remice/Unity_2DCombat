using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTowardMouse : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
