using UnityEditor;

[CustomPropertyDrawer(typeof(MuscleGroupFloatDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer {
}
