using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using Random = UnityEngine.Random;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    [Serializable]
    public class ItemDrop
    {
        public Item item;

        [Range(0f, 100f)]
        public float dropChance = 100f;
    }

    public ItemDrop[] itemDrops;

    public IEnumerable<Item> GetDrops()
    {
        foreach (var itemDrop in itemDrops)
        {
            var roll = Random.Range(0f, 100f);
            if (roll <= itemDrop.dropChance)
                yield return itemDrop.item;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(LootTable))]
    public class EditorLootTable : Editor
    {
        private LootTable m_LootTable;
        private string m_Result;

        private void OnEnable()
        {
            m_LootTable = target as LootTable;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Test Drop"))
            {
                if (m_LootTable == null)
                    return;

                m_Result = string.Empty;

                var itemDrops = m_LootTable.GetDrops();
                foreach (var itemDrop in itemDrops)
                    m_Result += itemDrop.ToString() + '\n';

                m_Result = m_Result.TrimEnd('\n');

                if (m_Result == string.Empty)
                    m_Result = "No Drops";
            }

            if (m_Result != null)
                EditorGUILayout.TextArea(m_Result);
        }
    }

#endif
}
