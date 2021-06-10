using UnityEngine;


namespace _2nd_Part
{
    public static class RandomXT
    {
        public static Vector2 RandomVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        public static Vector3 RandomUnitVector3()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        public static bool RandomBool()
        {
            return Random.Range(0f, 1f) > 0.5f;
        }

        public static Gradient RandomGradient(Color[] colors)
        {
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[colors.Length];
            GradientColorKey[] colorKeys = new GradientColorKey[colors.Length];

            float time = 0f;
            float increment = 1f / (float) colors.Length;
            for (int i = 0; i < colors.Length; ++i)
            {
                alphaKeys[i] = new GradientAlphaKey(colors[i].a, time);
                colorKeys[i] = new GradientColorKey(colors[i], time);
                time += increment;
            }

            Gradient gradient = new Gradient();
            gradient.alphaKeys = alphaKeys;
            gradient.colorKeys = colorKeys;
            return gradient;
        }
    }
}