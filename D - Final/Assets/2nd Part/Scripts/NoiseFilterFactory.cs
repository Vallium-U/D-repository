using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2nd_Part
{
    public static class NoiseFilterFactory
    {
        public static INoiseFilter createNoiseFilter(NoiseSettings settings)
        {
            switch (settings.filterType)
            {
                case NoiseSettings.FilterType.Simple:
                    return new SimpleNoiseFilter(settings.simpleNoiseSettings);
                case NoiseSettings.FilterType.Ridgid:
                    return new RidgidNoiseFilter(settings.ridgidNoiseSettings);
            }

            return null;
        }
    }
}