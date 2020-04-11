using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public float Noise;
    public float X;

    // Update is called once per frame
    void Update()
    {
        X += .1f;
        Noise = Mathf.PerlinNoise(X, 0);
    }
}
