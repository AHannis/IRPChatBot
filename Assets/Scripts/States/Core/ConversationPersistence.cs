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

        if (string.IsNullOrEmpty(history))
        {
            return;
        }

        string[] messages =
            history.Split("||");

        foreach (string msg in messages)
        {
            if (string.IsNullOrEmpty(msg))
            {
                continue;
            }

            bool isUser =
                msg.StartsWith("U:");

            string text =
                msg.Substring(2);

            chat.uiManager.CreateMessage(
                text,
                isUser,
                chat
            );
        }
    }

    public void ClearConversation()
    {
        PlayerPrefs.DeleteKey(
            saveKey
        );
    }
}