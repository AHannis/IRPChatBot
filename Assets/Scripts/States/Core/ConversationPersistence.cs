using UnityEngine;
using System.Collections.Generic;

public class ConversationPersistence : MonoBehaviour
{
    //main save key for entire conversation history
    public string saveKey =
        "ChatHistory";

    //used for future proofing save updates
    public int saveVersion = 1;

    //maximum stored messages before trimming old ones
    public int maxSavedEntries = 120;

    //saves individual message into persistent history
    public void SaveConversation(
        string message,
        bool isUser
    )
    {
        //prevents save corruption if separator appears in text
        message =
            message.Replace(
                "||",
                ""
            );

        string history =
            PlayerPrefs.GetString(
                saveKey,
                ""
            );

        string entry =
            (isUser ? "U:" : "A:")
            + message
            + "||";

        history += entry;

        //splits stored history into entries
        string[] messages =
            history.Split(
                "||"
            );

        List<string> cleaned =
            new List<string>();

        foreach (
            string msg
            in messages
        )
        {
            if (
                !string.IsNullOrEmpty(
                    msg
                )
            )
            {
                cleaned.Add(
                    msg
                );
            }
        }

        //keeps only newest messages to stop playerprefs being too big
        if (
            cleaned.Count
            > maxSavedEntries
        )
        {
            cleaned.RemoveRange(
                0,
                cleaned.Count
                - maxSavedEntries
            );
        }

        string rebuiltHistory =
            "";

        foreach (
            string msg
            in cleaned
        )
        {
            rebuiltHistory +=
                msg + "||";
        }

        PlayerPrefs.SetString(
            saveKey,
            rebuiltHistory
        );

        //stores save version for future compatibility
        PlayerPrefs.SetInt(
            "SaveVersion",
            saveVersion
        );

        //forces save immediately for crash safety
        PlayerPrefs.Save();
    }

    //conversation history reconstructed on startup
    //helps simulate long term memory 
    public void LoadConversation(
        ChatManager chat
    )
    {
        string history =
            PlayerPrefs.GetString(
                saveKey,
                ""
            );

        if (
            string.IsNullOrEmpty(
                history
            )
        )
        {
            return;
        }

        string[] messages =
            history.Split(
                "||"
            );

        foreach (
            string msg
            in messages
        )
        {
            if (
                string.IsNullOrEmpty(
                    msg
                )
            )
            {
                continue;
            }

            //extra safety 
            if (
                msg.Length < 3
            )
            {
                continue;
            }

            bool isUser =
                msg.StartsWith(
                    "U:"
                );

            string text =
                msg.Substring(2);

            //recreates message bubbles from saved history
            chat.uiManager.CreateMessage(
                text,
                isUser,
                chat
            );
        }
    }

    //persistent memory contributes to illusion of relationship continuity between sessions
    public void SavePlayerData(
        ChatManager chat
    )
    {
        PlayerPrefs.SetString(
            "UserName",
            chat.userName
        );

        PlayerPrefs.SetInt(
            "RelationshipLevel",
            chat.relationshipLevel
        );

        PlayerPrefs.SetString(
            "LastTopic",
            chat.lastTopic
        );

        PlayerPrefs.SetInt(
            "CurrentMood",
            (int)chat.currentMood
        );

        PlayerPrefs.SetInt(
            "UserAge",
            chat.userAge
        );

        PlayerPrefs.SetInt(
            "KnowsUserAge",
            chat.knowsUserAge ? 1 : 0
        );

        PlayerPrefs.SetString(
            "LifeStage",
            chat.lifeStage
        );

        PlayerPrefs.SetInt(
            "CompletedIntro",
            chat.completedIntro ? 1 : 0
        );

        //stores emotional continuity between sessions
        PlayerPrefs.SetString(
            "LastEmotion",
            chat.lastEmotion
        );

        PlayerPrefs.SetInt(
            "EmotionPersistence",
            chat.emotionPersistence
        );

       
        PlayerPrefs.SetFloat(
            "ChaosLevel",
            chat.chaosLevel
        );

        PlayerPrefs.Save();
    }

    //reloads player relationship state + emotional context
    public void LoadPlayerData(
        ChatManager chat
    )
    {
        chat.userName =
            PlayerPrefs.GetString(
                "UserName",
                ""
            );

        chat.relationshipLevel =
            PlayerPrefs.GetInt(
                "RelationshipLevel",
                0
            );

        chat.lastTopic =
            PlayerPrefs.GetString(
                "LastTopic",
                ""
            );

        chat.currentMood =
            (ChatManager.Mood)
            PlayerPrefs.GetInt(
                "CurrentMood",
                0
            );

        chat.userAge =
            PlayerPrefs.GetInt(
                "UserAge",
                -1
            );

        chat.knowsUserAge =
            PlayerPrefs.GetInt(
                "KnowsUserAge",
                0
            ) == 1;

        chat.lifeStage =
            PlayerPrefs.GetString(
                "LifeStage",
                ""
            );

        chat.completedIntro =
            PlayerPrefs.GetInt(
                "CompletedIntro",
                0
            ) == 1;

        //reloads emotional state memory
        chat.lastEmotion =
            PlayerPrefs.GetString(
                "LastEmotion",
                ""
            );

        chat.emotionPersistence =
            PlayerPrefs.GetInt(
                "EmotionPersistence",
                0
            );

        chat.chaosLevel =
            PlayerPrefs.GetFloat(
                "ChaosLevel",
                0f
            );
    }

    //clears saved conversation history
    public void ClearConversation()
    {
        PlayerPrefs.DeleteKey(
            saveKey
        );

        PlayerPrefs.Save();
    }
}