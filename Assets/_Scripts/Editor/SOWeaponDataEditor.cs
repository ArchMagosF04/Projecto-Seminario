using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using System.Linq;

[CustomEditor(typeof(SO_WeaponData))]
public class SOWeaponDataEditor : Editor
{
    private static List<Type> dataCompTypes = new List<Type>();

    private SO_WeaponData dataSO;

    private void OnEnable()
    {
        dataSO = target as SO_WeaponData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var item in dataCompTypes)
        {
            if (GUILayout.Button(item.Name))
            {
                var comp = Activator.CreateInstance(item) as ComponentData;

                if (comp == null) return;

                dataSO.AddData(comp);
            }
        }
    }

    [DidReloadScripts]
    private static void OnRecompile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var filterTypes = types.Where(type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass);

        dataCompTypes = filterTypes.ToList();
    }
}
