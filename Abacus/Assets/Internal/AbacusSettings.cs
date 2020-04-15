using Abacus.Internal.Utilities;
using UnityEngine;

namespace Abacus.Internal
{
    /// <summary>
    /// Stores the settings of the Abacus plugin.
    /// Modifying it's contents should be handled by the Abacus Settings Window
    /// </summary>
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