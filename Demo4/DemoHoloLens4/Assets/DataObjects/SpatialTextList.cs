using System.Collections.Generic;

namespace DemoServices.DataObjects
{
    public class SpatialTextList
    {
        public SpatialTextList()
        {
            texts = new List<SpatialText>();
        }
        public List<SpatialText> texts;

        public bool FromCache { get; set; }

    }

}
