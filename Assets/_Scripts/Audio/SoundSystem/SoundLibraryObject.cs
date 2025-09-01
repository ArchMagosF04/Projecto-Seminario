using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSoundLibrary", menuName = "Audio Library/Sound Dictionary", order = 0)]
public class SoundLibraryObject : ScriptableObject
{
    [SerializeField] private SoundData[] soundData;
    public Dictionary<string, SoundData> Library = new Dictionary<string, SoundData>();

    public SoundData nullData;

    public void Initialize()
    {
        foreach (var data in soundData)
        {
            if (!Library.ContainsKey(data.Id))
            {
                Library.Add(data.Id, data);
            }            
        }
    }

    public SoundData GetSound(string name)
    {
        if (Library.TryGetValue(name, out SoundData data))
        {
            return data;
        }

        else return nullData;
    }
}
