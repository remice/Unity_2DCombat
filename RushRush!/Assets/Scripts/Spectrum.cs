using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrum : MonoBehaviour
{
    public GameObject[] spectrumPool;

    private bool onSpectrum = false;
    private float curSpectrumDelay = 0;
    private float maxSpectrumDelay = 0.04f;
    private int spectrumIndex = 0;


    private void Update()
    {
        curSpectrumDelay += Time.deltaTime;
        if(onSpectrum && curSpectrumDelay >= maxSpectrumDelay)
        {
            curSpectrumDelay = 0;
            PlaySpectrum();
        }
    }

    public void OnSpectrum()
    {
        onSpectrum = true;
        Invoke("OffSpectrum", 0.3f);
    }

    private void OffSpectrum()
    {
        onSpectrum = false;
    }

    private void PlaySpectrum()
    {
        spectrumPool[spectrumIndex].SetActive(true);
        spectrumPool[spectrumIndex].transform.position = transform.position - new Vector3(0, 0, -5);
        spectrumPool[spectrumIndex].transform.rotation = transform.rotation;
        spectrumPool[spectrumIndex].transform.localScale = transform.localScale;

        spectrumIndex++;
        if (spectrumIndex >= 6) spectrumIndex = 0;
    }
}
