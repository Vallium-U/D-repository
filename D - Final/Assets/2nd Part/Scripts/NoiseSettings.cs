using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _2nd_Part;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType{Simple, Ridgid };
    public FilterType filterType;

    [ConditionalHide("filterType",0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType",1)]
    public RidgidNoiseSettings ridgidNoiseSettings;
    
    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1,9)]
        public int numberOfLayers = 1;

        public float persistance = 0.5f;
        public float baseRoughness = 2;
        public float roughness = 1;
        public Vector3 centre;
        public float minVal;
    }
    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }

}
