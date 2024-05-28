using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceDataPersistence 
{
 void LoadData(GameData GData);
 void SaveData(ref GameData GData);
}
