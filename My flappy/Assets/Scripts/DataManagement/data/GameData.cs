using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    //these are the stuff I want to save
   public int highScore;
    // the values defined in constructor will be default values
   public GameData(){
    highScore = 0;
   }
}
