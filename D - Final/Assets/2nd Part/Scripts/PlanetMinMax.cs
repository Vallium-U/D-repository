using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _2nd_Part
{

    public class PlanetMinMax
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public PlanetMinMax()
        {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public void AddVal(float v)
        {
            if (v > Max)
            {
                Max = v;
            }

            if (v < Min)
            {
                Min = v;
            }
        }

    }
}