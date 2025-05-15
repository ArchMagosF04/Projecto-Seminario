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

    private bool showForceUpdateButtons;
    private bool showAddComponentButtons;

    private void OnEnable()
    {
        dataSO = target as SO_WeaponData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Number of Attacks"))
        {
            foreach (var item in dataSO.ComponentData)
            {
                item.InitializeAttackData(dataSO.NumberOfAttacks);
;            }
        }

        showAddComponentButtons = EditorGUILayout.Foldout(showAddComponentButtons, "Add Components Buttons");

        if (showAddComponentButtons)
        {
            foreach (var item in dataCompTypes)
            {
                if (GUILayout.Button(item.Name))
                {
                    var comp = Activator.CreateInstance(item) as ComponentData;

                    if (comp == null) return;

                    comp.InitializeAttackData(dataSO.NumberOfAttacks);

                    dataSO.AddData(comp);
                }
            }
        }

        showForceUpdateButtons = EditorGUILayout.Foldout(showForceUpdateButtons, "Force Updates Buttons");

        if (showForceUpdateButtons)
        {
            if (GUILayout.Button("Force Update Component Names"))
            {
                foreach (var item in dataSO.ComponentData)
                {
                    item.SetComponentName();
                }
            }

            if (GUILayout.Button("Force Update Attack Names"))
            {
                foreach (var item in dataSO.ComponentData)
                {
                    item.SetAttackDataNames();
                }
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
