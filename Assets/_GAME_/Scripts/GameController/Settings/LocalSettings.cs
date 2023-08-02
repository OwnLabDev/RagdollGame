using UnityEngine;

using MoreMountains.Tools;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "LocalSettings",
        menuName = "ScriptableObjects/LocalSettings")]
    public class LocalSettings : ScriptableObject {
        [Header("Static settings"), Space(10)]
        [NaughtyAttributes.Expandable]
        public LevelSettings LevelSettings = default;

        [Header("Dynamic settings"), Space(10)]
        [NaughtyAttributes.Expandable, NaughtyAttributes.ReadOnly]
        public LocalEnvironmentSettings EnvironmentSettings = default;
    }
}