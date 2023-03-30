using Dungeon;
using UnityEngine;
using UnityEditor;

namespace Editor {
    [CustomEditor(typeof(AbstractDungeonGenerator), true)]
    public class RandomDungeonGeneratorEditor : UnityEditor.Editor {
        private AbstractDungeonGenerator generator;

        private void Awake() {
            generator = (AbstractDungeonGenerator) target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate")) {
                generator.generateDungeon();
            }
        }
    }
}
