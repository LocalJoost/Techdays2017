using System;

namespace DemoServices.DataObjects
{
    public class SpatialText
    {
        public SpatialText()
        {
        }

        public SpatialText(string vtext, float vx, float vy, float vz)
        {
            Text = vtext;
            X = vx;
            Y = vy;
            Z = vz;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public string Text { get; set; }
    }
}