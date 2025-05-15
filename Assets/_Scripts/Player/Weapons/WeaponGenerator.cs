using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private SO_WeaponData data;

    private List<WeaponComponent> componentsAlreadyOnWeapon = new List<WeaponComponent>();

    private List<WeaponComponent> componentsAddedToWeapon = new List<WeaponComponent>();

    private List<Type> componentDependencies = new List<Type>();

    private void Start()
    {
        GenerateWeapon(data);
    }

    [ContextMenu("Test Generate")]
    private void TestGeneration()
    {
        GenerateWeapon(data);
    }

    public void GenerateWeapon(SO_WeaponData data)
    {
        weapon.SetData(data);

        componentsAlreadyOnWeapon.Clear();
        componentsAddedToWeapon.Clear();
        componentDependencies.Clear();

        componentsAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();

        componentDependencies = data.GetAllDependencies();

        foreach (var dependency in componentDependencies)
        {
            if (componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency)) 
                continue;

            var weaponComponent = componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);

            if (weaponComponent == null)
            {
                weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
            }

            weaponComponent.Init();

            componentsAddedToWeapon.Add(weaponComponent);
        }

        var componentsToRemove = componentsAlreadyOnWeapon.Except(componentsAddedToWeapon);

        foreach (var component in componentsToRemove)
        {
            Destroy(component);
        }
    }
}
