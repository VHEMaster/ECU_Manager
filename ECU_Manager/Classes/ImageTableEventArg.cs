using System;

namespace ECU_Manager.Classes
{
    public class ImageTableEventArg : EventArgs
    {
        public int Index { get; set; }
        public float Value { get; set; }
    }
}