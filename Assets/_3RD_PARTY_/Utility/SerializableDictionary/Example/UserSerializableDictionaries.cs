using OL.Game;
using System;

using UnityEngine;

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> { }

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary
    <string, Color[], ColorArrayStorage> { }

// OL.GAME

[Serializable]
public class MuscleGroupFloatDictionary : SerializableDictionary<EnemyBodyPartType, float> {

}