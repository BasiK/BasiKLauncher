using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BasiK.BasiKLauncher
{
    /// <summary>
    /// XInput dll import definitions.
    /// </summary>
    internal static class XInput
    {
        // TODO : check if we can always use this xinput dll

        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputGetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputState pState        // [out] Receives the current state
        );


        /*
        [DllImport("xinput1_4.dll")]
        internal static extern int XInputGetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputState pState        // [out] Receives the current state
        );
         * */

    }

}
