using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    //stores persistent user identity
    //helps reinforce conversational continuity
    public string userName = "";

    //optional nickname used in casual conversation
    public string preferredNickname = "";

    //tracks overall relationship progression
    public int relationshipLevel = 0;

    //tracks user age for contextual responses
    public int userAge = -1;

    //prevents repeated age questioning
    public bool knowsUserAge = false;

    //stores detected life stage
    public string lifeStage = "";

    //stores emotional state between sessions
    public ChatManager.Mood currentMood =
        ChatManager.Mood.Neutral;

    //tracks total conversations started
    public int totalConversations = 0;

    //tracks emotionally supportive interactions
    public int totalComfortInteractions = 0;

    //tracks positive relationship moments
    public int totalPositiveInteractions = 0;

    //stores last interaction date
    public string lastInteractionDate = "";

    //tracks how many consecutive days user returned
    public int consecutiveDaysTalked = 0;

    //stores how long user has been known
    public int totalDaysKnown = 0;

    //tracks personality drift toward chaos/playfulness
    public float chaosAffinity = 0f;

    //tracks general trust/familiarity
    public float trustLevel = 0f;

    //future proofing for save updates
    public int playerDataVersion = 1;

    //loads persistent relationship state
    //similar to eliza style continuity where remembering details
   
    public void LoadPlayerData()
    {
        userName =
            PlayerPrefs.GetString(
                "UserName",
                ""
            );

        preferredNickname =
            PlayerPrefs.GetString(
                "PreferredNickname",
                ""
            );

        relationshipLevel =
            Mathf.Clamp(
                PlayerPrefs.GetInt(
                    "RelationshipLevel",
                    0
                ),
                0,
                100
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

        totalConversations =
            PlayerPrefs.GetInt(
                "TotalConversations",
                0
            );

        totalComfortInteractions =
            PlayerPrefs.GetInt(
                "ComfortInteractions",
                0
            );

        totalPositiveInteractions =
            PlayerPrefs.GetInt(
                "PositiveInteractions",
                0
            );

        lastInteractionDate =
            PlayerPrefs.GetString(
                "LastInteractionDate",
                ""
            );

        consecutiveDaysTalked =
            PlayerPrefs.GetInt(
                "ConsecutiveDaysTalked",
                0
            );

        totalDaysKnown =
            PlayerPrefs.GetInt(
                "TotalDaysKnown",
                0
            );

        chaosAffinity =
            PlayerPrefs.GetFloat(
                "ChaosAffinity",
                0f
            );

        trustLevel =
            PlayerPrefs.GetFloat(
                "TrustLevel",
                0f
            );

        playerDataVersion =
            PlayerPrefs.GetInt(
                "PlayerDataVersion",
                1
            );

        //automatically derives life stage if missing
        DetectLifeStage();

        UpdateInteractionTracking();
    }

    //stores all relationship variables permanently
    public void SavePlayerData()
    {
        PlayerPrefs.SetString(
            "UserName",
            userName
        );

        PlayerPrefs.SetString(
            "PreferredNickname",
            preferredNickname
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

        PlayerPrefs.SetInt(
            "TotalConversations",
            totalConversations
        );

        PlayerPrefs.SetInt(
            "ComfortInteractions",
            totalComfortInteractions
        );

        PlayerPrefs.SetInt(
            "PositiveInteractions",
            totalPositiveInteractions
        );

        PlayerPrefs.SetString(
            "LastInteractionDate",
            DateTime.Now
            .ToShortDateString()
        );

        PlayerPrefs.SetInt(
            "ConsecutiveDaysTalked",
            consecutiveDaysTalked
        );

        PlayerPrefs.SetInt(
            "TotalDaysKnown",
            totalDaysKnown
        );

        PlayerPrefs.SetFloat(
            "ChaosAffinity",
            chaosAffinity
        );

        PlayerPrefs.SetFloat(
            "TrustLevel",
            trustLevel
        );

        PlayerPrefs.SetInt(
            "PlayerDataVersion",
            playerDataVersion
        );

        PlayerPrefs.Save();
    }

    //automatically determines life stage from age
    public void DetectLifeStage()
    {
        if (
            userAge < 0
        )
        {
            return;
        }

        if (
            userAge < 18
        )
        {
            lifeStage =
                "school";

            return;
        }

        if (
            userAge <= 25
        )
        {
            lifeStage =
                "youngAdult";

            return;
        }

        lifeStage =
            "adult";
    }

    //tracks returning users + continuity illusion
    public void UpdateInteractionTracking()
    {
        string today =
            DateTime.Now
            .ToShortDateString();

        if (
            string.IsNullOrEmpty(
                lastInteractionDate
            )
        )
        {
            lastInteractionDate =
                today;

            totalDaysKnown = 1;

            consecutiveDaysTalked = 1;

            return;
        }

        DateTime previousDate;

        if (
            DateTime.TryParse(
                lastInteractionDate,
                out previousDate
            )
        )
        {
            TimeSpan difference =
                DateTime.Now.Date
                - previousDate.Date;

            //returned next day
            if (
                difference.Days == 1
            )
            {
                consecutiveDaysTalked++;
            }
            //same day
            else if (
                difference.Days == 0
            )
            {
            }
            //streak broken
            else
            {
                consecutiveDaysTalked = 1;
            }

            totalDaysKnown +=
                Mathf.Max(
                    difference.Days,
                    0
                );
        }

        lastInteractionDate =
            today;
    }

    //slowly fades emotional states over time
    public void DecayMood()
    {
        if (
            currentMood
            == ChatManager.Mood.Neutral
        )
        {
            return;
        }

        if (
            UnityEngine.Random.value
            < 0.08f
        )
        {
            currentMood =
                ChatManager.Mood.Neutral;
        }
    }

    //fully resets persistent player state
    public void ResetPlayerData()
    {
        userName = "";

        preferredNickname = "";

        relationshipLevel = 0;

        userAge = -1;

        knowsUserAge = false;

        lifeStage = "";

        currentMood =
            ChatManager.Mood.Neutral;

        totalConversations = 0;

        totalComfortInteractions = 0;

        totalPositiveInteractions = 0;

        consecutiveDaysTalked = 0;

        totalDaysKnown = 0;

        chaosAffinity = 0f;

        trustLevel = 0f;

        lastInteractionDate = "";
    }
}