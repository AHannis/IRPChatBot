using System.Collections.Generic;
using UnityEngine;

public class MemorySystem : MonoBehaviour
{
    Dictionary<string, string> memories =
        new Dictionary<string, string>();

    string memoryPrefix =
        "MEMORY_";

    public void Remember(
        string key,
        string value
    )
    {
        if (
            memories.ContainsKey(
                key
            )
        )
        {
            memories[key] =
                value;
        }
        else
        {
            memories.Add(
                key,
                value
            );
        }

        PlayerPrefs.SetString(
            memoryPrefix + key,
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

    public void RemoveMemory(
        string key
    )
    {
        if (
            memories.ContainsKey(
                key
            )
        )
        {
            memories.Remove(
                key
            );
        }

        PlayerPrefs.DeleteKey(
            memoryPrefix + key
        );
    }

    public void SaveMemories()
    {
        foreach (
            KeyValuePair<string, string> pair
            in memories
        )
        {
            PlayerPrefs.SetString(
                memoryPrefix
                + pair.Key,
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
            "favoriteFood",
            "favoriteCharacter",
            "favoriteSong",
            "favoriteMovie",
            "favoriteShow",
            "favoriteColour",
            "favoriteAnimal",
            "slang_skibidi",
            "slang_rizz",
            "slang_gyatt"
        };

        foreach (
            string key
            in knownKeys
        )
        {
            string value =
                PlayerPrefs.GetString(
                    memoryPrefix + key,
                    ""
                );

            if (
                !string.IsNullOrEmpty(
                    value
                )
            )
            {
                memories[key] =
                    value;
            }
        }
    }

    public List<string> GetAllMemoryKeys()
    {
        return new List<string>(
            memories.Keys
        );
    }

    public void ClearMemories()
    {
        foreach (
            string key
            in new List<string>(
                memories.Keys
            )
        )
        {
            PlayerPrefs.DeleteKey(
                memoryPrefix + key
            );
        }

        memories.Clear();
    }
}