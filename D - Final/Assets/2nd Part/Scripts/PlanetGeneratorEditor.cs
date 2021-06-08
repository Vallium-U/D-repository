// using System;
// using UnityEditor;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace _2nd_Part
// {
//     [CustomEditor(typeof(PlanetGenerator))]
//     public class PlanetGeneratorEditor : UnityEditor.Editor
//     {
//         private PlanetGenerator[] planetGenerator;
//         Editor planetEditor;
//         public override void OnInspectorGUI()
//         {
//             using (var check = new EditorGUI.ChangeCheckScope())
//             {
//                 base.OnInspectorGUI();
//             }
//
//             for (int i = 0; i < planetGenerator.Length; i++)
//             {
//                 UpdateSettingsEditor(planetGenerator[i].settings[i], null, ref planetGenerator[i].settingsFoldout, ref planetEditor);
//             }
//
//             
//         }
//         void UpdateSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
//         {
//             if (settings!=null)
//             {
//                 foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
//                 using (var check = new EditorGUI.ChangeCheckScope())
//                 {
//                     if (foldout)
//                     {
//                         CreateCachedEditor(settings,null, ref editor);
//                         editor.OnInspectorGUI();
//                         if (check.changed)
//                         {
//                             onSettingsUpdated?.Invoke();
//                         }  
//                     }
//                 }
//             }
//         }
//         
//         private void OnEnable()
//         {
//             planetGenerator = new[] {(PlanetGenerator) target};
//         }
//     }
//  }