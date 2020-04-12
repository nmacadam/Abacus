using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abacus.Internal
{
    public class AbacusSettings : SingletonScriptableObject<AbacusSettings>
    {
        // examples:
        public float DefaultTimeStep = 1f;
        public int BindingFlags;
        [Space]
        public bool PeriodicDumping = false;
        public float DumpAfter = 30f;
        [Space]
        public bool DumpAsJSON = false;

        public bool FormatOutput = false;

        public string WritePath;

        private void OnEnable()
        {
            if (WritePath == string.Empty) WritePath = Application.persistentDataPath;
        }
    }
}