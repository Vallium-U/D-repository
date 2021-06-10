using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _2nd_Part
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : UnityEditor.Editor
    {
        private Planet planet;
        Editor shapeEditor;
        Editor colourEditor;
        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (check.changed)
                {
                    planet.GeneratePlanet();
                }
            }

            if (GUILayout.Button("Generate Planet"))
            {
                planet.GeneratePlanet();
            }

            UpdateSettingsEditor(planet.planetShapeSettings, planet.OnShapeSettingUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
            UpdateSettingsEditor(planet.planetColorSettings, planet.OnColourSettingUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
        }

        void UpdateSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
        {
            if (settings!=null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings,null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed)
                        {
                            onSettingsUpdated?.Invoke();
                        }  
                    }
                }
            }
        }

        private void OnEnable()
        {
            planet = (Planet) target;
        }
    }
}
