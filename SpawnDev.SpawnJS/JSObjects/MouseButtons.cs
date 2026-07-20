// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A number representing one or more buttons. For more than one button pressed simultaneously, the values are combined (e.g., 3 is primary + secondary).<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MouseEvent/buttons#value
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        /// <summary>
        /// 0: No button or un-initialized
        /// </summary>
        NoButton = 0,
        /// <summary>
        /// 1: Primary button (usually the left button)
        /// </summary>
        PrimaryButton = 1,
        /// <summary>
        /// 2: Secondary button (usually the right button)
        /// </summary>
        SecondaryButton = 2,
        /// <summary>
        /// 4: Auxiliary button (usually the mouse wheel button or middle button)
        /// </summary>
        AuxiliaryButton = 4,
        /// <summary>
        /// 8: 4th button (typically the "Browser Back" button)
        /// </summary>
        FourthButton = 8,
        /// <summary>
        /// 16 : 5th button (typically the "Browser Forward" button)
        /// </summary>
        FifthButton = 16,
    }
}
