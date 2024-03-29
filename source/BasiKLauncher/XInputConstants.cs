﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BasiK.BasiKLauncher
{
    [Flags]
    public enum ButtonFlags : int
    {
        XINPUT_GAMEPAD_DPAD_UP = 0x0001,
        XINPUT_GAMEPAD_DPAD_DOWN = 0x0002,
        XINPUT_GAMEPAD_DPAD_LEFT = 0x0004,
        XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008,
        XINPUT_GAMEPAD_START = 0x0010,
        XINPUT_GAMEPAD_BACK = 0x0020,
        XINPUT_GAMEPAD_LEFT_THUMB = 0x0040,
        XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080,
        XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100,
        XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200,
        XINPUT_GAMEPAD_A = 0x1000,
        XINPUT_GAMEPAD_B = 0x2000,
        XINPUT_GAMEPAD_X = 0x4000,
        XINPUT_GAMEPAD_Y = 0x8000,
    };

    [Flags]
    public enum ControllerSubtypes
    {
        XINPUT_DEVSUBTYPE_UNKNOWN = 0x00,
        XINPUT_DEVSUBTYPE_WHEEL = 0x02,
        XINPUT_DEVSUBTYPE_ARCADE_STICK = 0x03,
        XINPUT_DEVSUBTYPE_FLIGHT_STICK = 0x04,
        XINPUT_DEVSUBTYPE_DANCE_PAD = 0x05,
        XINPUT_DEVSUBTYPE_GUITAR = 0x06,
        XINPUT_DEVSUBTYPE_GUITAR_ALTERNATE = 0x07,
        XINPUT_DEVSUBTYPE_DRUM_KIT = 0x08,
        XINPUT_DEVSUBTYPE_GUITAR_BASS = 0x0B,
        XINPUT_DEVSUBTYPE_ARCADE_PAD = 0x13
    };


    public class XInputConstants
    {
        public const int XINPUT_DEVTYPE_GAMEPAD = 0x01;
        public const int XINPUT_DEVSUBTYPE_GAMEPAD = 0x01;

        public enum CapabilityFlags
        {
            XINPUT_CAPS_VOICE_SUPPORTED = 0x0004,
            //For Windows 8 only
            XINPUT_CAPS_FFB_SUPPORTED = 0x0001,
            XINPUT_CAPS_WIRELESS = 0x0002,
            XINPUT_CAPS_PMD_SUPPORTED = 0x0008,
            XINPUT_CAPS_NO_NAVIGATION = 0x0010,
        };

        public const int XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849;
        public const int XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689;
        public const int XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30;

        public const int XINPUT_FLAG_GAMEPAD = 0x00000001;

    }
}
