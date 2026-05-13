using System.Collections.Generic;
using UnityEngine;

public class MemorySystem : MonoBehaviour
{
    Dictionary<string, string> memories =
        new Dictionary<string, string>();

    public void Remember(
        string key,
        string value
    )
    {
        if (memories.ContainsKey(key))
        {
            memories[key] = value;
        }
        else
        {
            memories.Add(
                key,
                value
            );
        }

        PlayerPrefs.SetString(
            "MEMORY_" + key,
            value
        );
    }

    public bool HasMemory(
        string key
    )
    {
        return memories.ContainsKey(
            key
        );
    }

    public string Recall(
        string key
    )
    {
        if (
            memories.ContainsKey(
                key
            )
        )
        {
            return memories[key];
        }

        return "";
    }

    public void SaveMemories()
    {
        foreach (
            KeyValuePair<string, string> pair
            in memories
        )
        {
            PlayerPrefs.SetString(
                "MEMORY_" + pair.Key,
                pair.Value
            );
        }

        PlayerPrefs.Save();
    }

    public void LoadMemories()
    {
        memories.Clear();

        string[] knownKeys =
        {
            "favoriteGame",
            "favoriteFood"
        };

        foreach (string key in knownKeys)
        {
            string value =
                PlayerPrefs.GetString(
                    "MEMORY_" + key,
                    ""
                );

            if (
                !string.IsNullOrEmpty(
                    value
                )
            )
            {
                memories[key] = value;
            }
        }
    }

    public void ClearMemories()
    {
        memories.Clear();
    }
}