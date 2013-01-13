using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BasiK.BasiKLauncher
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct XInputState
    {
        [FieldOffset(0)]
        public int PacketNumber;

        [FieldOffset(4)]
        public XInputGamepad Gamepad;

        public void Copy(XInputState source)
        {
            PacketNumber = source.PacketNumber;
            Gamepad.Copy(source.Gamepad);
        }

        public override int GetHashCode()
        {
            return PacketNumber.GetHashCode() + Gamepad.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || (!(obj is XInputState)))
                return false;
            XInputState source = (XInputState)obj;

            return ((PacketNumber == source.PacketNumber)
                && (Gamepad.Equals(source.Gamepad)));
        }
    }

}
