using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbacusSettings : ScriptableObject
{
    public int BindingFlags;
    // examples:
    public float DefaultTimeStep = 1f;
    [Space]
    public bool PeriodicDumping = false;
    public float DumpAfter = 30f;
    [Space]
    public bool DumpAsJSON = false;

    public bool PrettyPrint = false;
}
