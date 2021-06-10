using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _2nd_Part
{
    [System.Serializable]
    public class RandomValue<T>
    {
        [SerializeField]
        protected T min;
        [SerializeField]
        protected T max;
        [HideInInspector]
        public T lastValue;

        public RandomValue(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        public virtual T PickRandomValue()
        {
            return min;
        }
    }

    [System.Serializable]
    public class RandomInt : RandomValue<int>
    {
        public RandomInt(int min, int max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override int PickRandomValue()
        {
            return lastValue = Random.Range(min, max + 1);
        }
    }
    
    [System.Serializable]
    public class RandomFloat : RandomValue<float>
    {
        public RandomFloat(float min, float max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override float PickRandomValue()
        {
            return lastValue = Random.Range(min, max);
        }
    }

    [System.Serializable]
    public class RandomColor : RandomValue<Color>
    {
        public RandomColor(Color min, Color max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override Color PickRandomValue()
        {
            float r = Random.Range(min.r, max.r);
            float g = Random.Range(min.g, max.g);
            float b = Random.Range(min.b, max.b);
            float a = Random.Range(min.a, max.a);

            return lastValue = new Color(r, g, b, a);
        }
    }

    [System.Serializable]
    public class RandomSimpleNoiseSettings : RandomValue<NoiseSettings.SimpleNoiseSettings>
    {
        public RandomSimpleNoiseSettings(NoiseSettings.SimpleNoiseSettings min, NoiseSettings.SimpleNoiseSettings max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override NoiseSettings.SimpleNoiseSettings PickRandomValue()
        {
            NoiseSettings.SimpleNoiseSettings settings = new NoiseSettings.SimpleNoiseSettings();
            settings.baseRoughness = Random.Range(min.baseRoughness, max.baseRoughness);
            settings.centre = RandomXT.RandomVector3(min.centre, max.centre);
            settings.numberOfLayers = Random.Range(min.numberOfLayers, max.numberOfLayers);
            settings.minVal = Random.Range(min.minVal, max.minVal);
            settings.persistance = Random.Range(min.persistance, max.persistance);
            settings.roughness = Random.Range(min.roughness, max.roughness);
            settings.strength = Random.Range(min.strength, max.strength);

            return lastValue = settings;
        }

        public NoiseSettings.SimpleNoiseSettings PickRandomValueClamped()
        {
            NoiseSettings.SimpleNoiseSettings settings = PickRandomValue();

            // -- clamp persistance to avoid gigantic juts 
            if (settings.persistance * 4 < settings.strength)
                settings.persistance = settings.strength * 0.5f;

            // -- clamp minvalue to always be less than strength + persistance to avoid more juts
            if (settings.strength + settings.persistance < settings.minVal)
                settings.minVal = (settings.strength + settings.persistance) * Random.Range(0.75f, 0.95f);

            return lastValue = settings;
        }
    }
    
    [System.Serializable]
    public class RandomRidgidNoiseSettings : RandomValue<NoiseSettings.RidgidNoiseSettings>
    {
        public RandomRidgidNoiseSettings(NoiseSettings.RidgidNoiseSettings min, NoiseSettings.RidgidNoiseSettings max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override NoiseSettings.RidgidNoiseSettings PickRandomValue()
        {
            NoiseSettings.RidgidNoiseSettings settings = new NoiseSettings.RidgidNoiseSettings();
            settings.baseRoughness = Random.Range(min.baseRoughness, max.baseRoughness);
            settings.centre = RandomXT.RandomVector3(min.centre, max.centre);
            settings.numberOfLayers = Random.Range(min.numberOfLayers, max.numberOfLayers);
            settings.minVal = Random.Range(min.minVal, max.minVal);
            settings.persistance = Random.Range(min.persistance, max.persistance);
            settings.roughness = Random.Range(min.roughness, max.roughness);
            settings.strength = Random.Range(min.strength, max.strength);
            settings.weightMultiplier = Random.Range(min.weightMultiplier, max.weightMultiplier);

            return lastValue = settings;
        }

        public NoiseSettings.RidgidNoiseSettings PickRandomValueClamped()
        {
            NoiseSettings.RidgidNoiseSettings settings = PickRandomValue();

            // -- clamp persistance to avoid gigantic juts 
            if (settings.persistance * 4 < settings.strength)
                settings.persistance = settings.strength * 0.5f;

            // -- clamp minvalue to always be less than strength + persistance to avoid more juts
            if (settings.strength + settings.persistance < settings.minVal)
                settings.minVal = (settings.strength + settings.persistance) * Random.Range(0.75f, 0.95f);

            return lastValue = settings;
        }
    }
    
    [System.Serializable]
    public class RandomVector2 : RandomValue<Vector2>
    {
        public RandomVector2(Vector2 min, Vector2 max) : base(min, max)
        {
            this.min = min;
            this.max = max;
        }

        public override Vector2 PickRandomValue()
        {
            return lastValue = RandomXT.RandomVector2(min, max);
        }
    }
}