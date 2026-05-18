using System.Collections.Generic;
using UnityEngine;

public class MemorySystem : MonoBehaviour
{
    //stores active loaded memories in runtime
    Dictionary<string, string> memories =
        new Dictionary<string, string>();

    //tracks memory importance/strength
    Dictionary<string, int> memoryStrength =
        new Dictionary<string, int>();

    //prefix used for playerprefs storage
    string memoryPrefix =
        "MEMORY_";

    //stores list of all dynamic memory keys
    string memoryListKey =
        "MEMORY_KEYS";

    //allows memories to be overwritten
    public bool overwriteExistingMemories =
        true;

    //stores memory + persists it between sessions
    //helps simulate long term relationship continuity
    public void Remember(
        string key,
        string value
    )
    {
        if (
            string.IsNullOrWhiteSpace(
                key
            )
            || string.IsNullOrWhiteSpace(
                value
            )
        )
        {
            return;
        }

        //prevents broken save formatting
        value =
            value.Replace(
                "|",
                ""
            );

        if (
            memories.ContainsKey(
                key
            )
        )
        {
            if (
                overwriteExistingMemories
            )
            {
                memories[key] =
                    value;
            }

            memoryStrength[key]++;
        }
        else
        {
            memories.Add(
                key,
                value
            );

            //new memories start weak
            memoryStrength.Add(
                key,
                1
            );
        }

        //stores memory permanently
        PlayerPrefs.SetString(
            memoryPrefix + key,
            value
        );

        //stores memory strength separately
        PlayerPrefs.SetInt(
            memoryPrefix
            + key
            + "_strength",
            memoryStrength[key]
        );

        //stores timestamp for fake long term memory illusion
        PlayerPrefs.SetString(
            memoryPrefix
            + key
            + "_time",
            System.DateTime.Now
            .ToString()
        );

        SaveMemoryKeyList();

        PlayerPrefs.Save();
    }

    //checks if memory exists
    public bool HasMemory(
        string key
    )
    {
        return memories.ContainsKey(
            key
        );
    }

    //retrieves stored memory
    //similar to eliza style callbacks where old details
    //are reused to increase perceived intelligence
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
            //slightly strengthens recalled memories
            memoryStrength[key]++;

            return memories[key];
        }

        return "";
    }

    //returns memory strength value
    public int GetMemoryStrength(
        string key
    )
    {
        if (
            memoryStrength.ContainsKey(
                key
            )
        )
        {
            return memoryStrength[key];
        }

        return 0;
    }

    //removes stored memory
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

        if (
            memoryStrength.ContainsKey(
                key
            )
        )
        {
            memoryStrength.Remove(
                key
            );
        }

        PlayerPrefs.DeleteKey(
            memoryPrefix + key
        );

        PlayerPrefs.DeleteKey(
            memoryPrefix
            + key
            + "_strength"
        );

        PlayerPrefs.DeleteKey(
            memoryPrefix
            + key
            + "_time"
        );

        SaveMemoryKeyList();
    }

    //saves every active memory
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

        foreach (
            KeyValuePair<string, int> pair
            in memoryStrength
        )
        {
            PlayerPrefs.SetInt(
                memoryPrefix
                + pair.Key
                + "_strength",
                pair.Value
            );
        }

        SaveMemoryKeyList();

        PlayerPrefs.Save();
    }

    //loads dynamic memories automatically
    //instead of relying on hardcoded 
    public void LoadMemories()
    {
        memories.Clear();

        memoryStrength.Clear();

        string keyList =
            PlayerPrefs.GetString(
                memoryListKey,
                ""
            );

        if (
            string.IsNullOrEmpty(
                keyList
            )
        )
        {
            return;
        }

        string[] keys =
            keyList.Split('|');

        foreach (
            string key
            in keys
        )
        {
            if (
                string.IsNullOrWhiteSpace(
                    key
                )
            )
            {
                continue;
            }

            string value =
                PlayerPrefs.GetString(
                    memoryPrefix + key,
                    ""
                );

            //save corruption safety
            if (
                string.IsNullOrWhiteSpace(
                    value
                )
            )
            {
                continue;
            }

            memories[key] =
                value;

            memoryStrength[key] =
                PlayerPrefs.GetInt(
                    memoryPrefix
                    + key
                    + "_strength",
                    1
                );
        }
    }

    //stores all memory keys for dynamic loading
    void SaveMemoryKeyList()
    {
        string combined =
            "";

        foreach (
            string key
            in memories.Keys
        )
        {
            combined +=
                key + "|";
        }

        PlayerPrefs.SetString(
            memoryListKey,
            combined
        );
    }

    //returns all stored memory keys
    public List<string> GetAllMemoryKeys()
    {
        return new List<string>(
            memories.Keys
        );
    }

    //retrieves random memory for callbacks
    public string GetRandomMemory()
    {
        List<string> keys =
            GetAllMemoryKeys();

        if (
            keys.Count == 0
        )
        {
            return "";
        }

        string randomKey =
            keys[
                Random.Range(
                    0,
                    keys.Count
                )
            ];

        return Recall(
            randomKey
        );
    }

    //returns memories matching category prefixes
    //example: slang topic, emotion
    public List<string> GetMemoriesByCategory(
        string category
    )
    {
        List<string> results =
            new List<string>();

        foreach (
            KeyValuePair<string, string> pair
            in memories
        )
        {
            if (
                pair.Key.StartsWith(
                    category + "_"
                )
            )
            {
                results.Add(
                    pair.Value
                );
            }
        }

        return results;
    }

    //simulates human forgetting for weak memories
    public void ForgetWeakMemories()
    {
        List<string> toForget =
            new List<string>();

        foreach (
            KeyValuePair<string, int> pair
            in memoryStrength
        )
        {
            if (
                pair.Value <= 1
                && Random.value < 0.03f
            )
            {
                toForget.Add(
                    pair.Key
                );
            }
        }

        foreach (
            string key
            in toForget
        )
        {
            RemoveMemory(
                key
            );
        }
    }

    //completely wipes memory system
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

            PlayerPrefs.DeleteKey(
                memoryPrefix
                + key
                + "_strength"
            );

            PlayerPrefs.DeleteKey(
                memoryPrefix
                + key
                + "_time"
            );
        }

        PlayerPrefs.DeleteKey(
            memoryListKey
        );

        memories.Clear();

        memoryStrength.Clear();

        PlayerPrefs.Save();
    }
}