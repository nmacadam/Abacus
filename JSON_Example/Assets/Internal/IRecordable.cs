using System;
using UnityEngine;

namespace Abacus.Internal
{
    /// <summary>
    /// Implements methods for recordability, but without any hard data type
    /// </summary>
    public interface IRecordable
    {
        string GetVariableName();
        Type GetValueType();
        //string[] Dump();
        object Dump();
        void Record();
        void SetSource(Component component);
        void Enable();
        void Disable();

        float TimeStep { get; }
        bool IsEnabled { get; }
    }
}