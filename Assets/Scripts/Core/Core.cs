using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement { get; private set; }
    public CollisionSenses CollisionSenses { get; private set; }
    public Combat Combat { get; private set; }
    public Stats Stats { get; private set; }

    private List<ILogicUpdate> coreComponents = new List<ILogicUpdate>();

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();
        Combat = GetComponentInChildren<Combat>();
        Stats = GetComponentInChildren<Stats>();

        if (!Movement || !CollisionSenses || !Combat || !Stats) Debug.Log("Missing Core Component");
    }

    public void LogicUpdate()
    {
        foreach (ILogicUpdate component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(ILogicUpdate component)
    {
        if (!coreComponents.Contains(component)) coreComponents.Add(component);
    }
}
