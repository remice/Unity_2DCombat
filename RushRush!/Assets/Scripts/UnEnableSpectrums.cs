using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEnableSpectrums : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DelayUnEnalbe", 0.14f);
    }

    private void DelayUnEnalbe()
    {
        gameObject.SetActive(false);
    }
}
