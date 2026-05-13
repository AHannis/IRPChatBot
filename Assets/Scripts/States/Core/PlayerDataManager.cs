using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public string userName = "";

    public int relationshipLevel = 0;

    public int userAge = -1;

    public bool knowsUserAge = false;

    public string lifeStage = "";

    public ChatManager.Mood currentMood =
        ChatManager.Mood.Neutral;

    public void LoadPlayerData()
    {
        userName =
            PlayerPrefs.GetString(
                "UserName",
                ""
            );

        relationshipLevel =
            PlayerPrefs.GetInt(
                "RelationshipLevel",
                0
            );

        userAge =
            PlayerPrefs.GetInt(
                "UserAge",
                -1
            );

        knowsUserAge =
            PlayerPrefs.GetInt(
                "KnowsUserAge",
                0
            ) == 1;

        lifeStage =
            PlayerPrefs.GetString(
                "LifeStage",
                ""
            );

        currentMood =
            (ChatManager.Mood)
            PlayerPrefs.GetInt(
                "CurrentMood",
                0
            );
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString(
            "UserName",
            userName
        );

        PlayerPrefs.SetInt(
            "RelationshipLevel",
            relationshipLevel
        );

        PlayerPrefs.SetInt(
            "UserAge",
            userAge
        );

        PlayerPrefs.SetInt(
            "KnowsUserAge",
            knowsUserAge ? 1 : 0
        );

        PlayerPrefs.SetString(
            "LifeStage",
            lifeStage
        );

        PlayerPrefs.SetInt(
            "CurrentMood",
            (int)currentMood
        );

        PlayerPrefs.Save();
    }

    public void ResetPlayerData()
    {
        userName = "";

        relationshipLevel = 0;

        userAge = -1;

        knowsUserAge = false;

        lifeStage = "";

        currentMood =
            ChatManager.Mood.Neutral;
    }
}