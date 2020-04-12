namespace Abacus
{
    /// <summary>
    /// Records data for temporal types
    /// </summary>
    public interface ITemporal
    {
        string DisplayType { get; }
        string GetVariableName();
        object Dump();
    }
}