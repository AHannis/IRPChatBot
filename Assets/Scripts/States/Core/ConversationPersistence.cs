using UnityEngine;

public class ConversationPersistence : MonoBehaviour
{
    public string saveKey =
        "ChatHistory";

    public void SaveConversation(
        string message,
        bool isUser
    )
    {
        string entry =
            (isUser ? "U:" : "A:")
            + message
            + "||";

        PlayerPrefs.SetString(
            saveKey,
            PlayerPrefs.GetString(saveKey)
            + entry
        );
    }

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
            history.Split("||");

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

            bool isUser =
                msg.StartsWith(
                    "U:"
                );

            string text =
                msg.Substring(2);

            chat.uiManager.CreateMessage(
                text,
                isUser,
                chat
            );
        }
    }

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

        PlayerPrefs.Save();
    }

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
    }

    public void ClearConversation()
    {
        PlayerPrefs.DeleteKey(
            saveKey
        );
    }
}