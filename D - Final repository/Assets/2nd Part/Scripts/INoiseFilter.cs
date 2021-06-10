using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2nd_Part
{
    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }
}