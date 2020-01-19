using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ChemicalBagViewer : MonoBehaviour
{
    public ChemicalBag chemicalBag { get; private set; }

    void Start()
    {
        chemicalBag = GetComponent<ChemicalBag>();
    }
}

[CustomEditor(typeof(ChemicalBagViewer))]
public class ChemicalBagViewerEditor : Editor
{
    private Dictionary<Substance, float> max = new Dictionary<Substance, float>();

    public override void OnInspectorGUI()
    {
        ChemicalBag chemicalBag = ((ChemicalBagViewer)target).chemicalBag;

        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Substances");
        EditorGUILayout.BeginVertical();
        if (Application.isPlaying && chemicalBag != null && chemicalBag.IsInitialized)
        {
            foreach (var substance in chemicalBag.Keys.OrderBy(substance => -chemicalBag[substance]))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(substance.ToString());
                float mass = chemicalBag[substance];
                if (!max.ContainsKey(substance) || mass > .9 * max[substance] || mass < max[substance] * .05f)
                    max[substance] = 1.25f * mass;
                EditorGUILayout.Slider(mass, 0f, max[substance]);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    void OnSceneGUI()
    {
        if (Application.isPlaying)
            Repaint();
    }
}