using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WeaponInfo
{
    public string weaponName;
    public Sprite weaponImage;
    [TextArea] public string weaponDescription;
    public string weaponTrainingRoomName;
}
