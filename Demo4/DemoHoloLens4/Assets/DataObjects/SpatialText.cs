using System;

namespace DemoServices.DataObjects
{
    [Serializable]
    public class SpatialText
    {
        public SpatialText()
        {
        }

        public SpatialText(string vtext, float vx, float vy, float vz)
        {
            text = vtext;
            x = vx;
            y = vy;
            z = vz;
        }

        public float x ;

        public float y;

        public float z;

        public string text;
    }
}