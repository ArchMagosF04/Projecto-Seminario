using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> coreComponents = new List<CoreComponent>();

    public SoundLibraryObject SoundLibrary { get; private set; }

    private void Awake()
    {

    }

    public void SetSoundLibrary(SoundLibraryObject library) => SoundLibrary = library;

    public void LogicUpdate()
    {
        foreach (CoreComponent component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)
    {
        if (!coreComponents.Contains(component)) coreComponents.Add(component);
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = coreComponents.OfType<T>().FirstOrDefault();

        if (comp) return comp;

        comp = GetComponentInChildren<T>();

        if (comp) return comp;

        if (comp == null) Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
