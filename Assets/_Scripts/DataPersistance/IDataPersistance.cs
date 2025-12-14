using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData gameData);

    void SaveData(GameData gameData);
}
