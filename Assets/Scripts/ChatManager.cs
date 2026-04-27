using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public Transform content;
    public GameObject userContainerPrefab;
    public GameObject aiContainerPrefab;

    public ScrollRect scrollRect;
    public TMP_InputField inputField;

    public GameObject typingUI;
    public Image dot1;
    public Image dot2;
    public Image dot3;

    string saveKey = "ChatHistory";

    List<string> recentMessages = new List<string>();

    string userName = "";
    bool userAtBottom = true;

    bool waitingForName = false;
    bool inNameLoop = false;
    bool reconnecting = false;

    int nameChangeCount = 0;
    int convoState = 0;

    void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChanged);

        LoadConversation();

        userName = PlayerPrefs.GetString("UserName", "");

        if (!PlayerPrefs.HasKey("HasChattedBefore"))
        {
            SendAIImmediate("hey kid! it's uncle bob. i'm just re-adding everyone's numbers because i swapped sims... what do you want me to save you as?");
            PlayerPrefs.SetInt("HasChattedBefore", 1);
            waitingForName = true;
        }
        else
        {
            SendAIImmediate("haven't heard from you in a while, you okay?");
            reconnecting = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSendButton();
        }
    }

    void OnScrollChanged(Vector2 pos)
    {
        userAtBottom = scrollRect.verticalNormalizedPosition <= 0.05f;
    }

    public void OnSendButton()
    {
        string message = inputField.text;

        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        SendUserMessage(message);

        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void SendUserMessage(string message)
    {
        CreateMessage(message, true);
        SaveConversation(message, true);

        recentMessages.Add(message);

        if (recentMessages.Count > 6)
        {
            recentMessages.RemoveAt(0);
        }

        StartCoroutine(SendAIResponse(message));
    }

    IEnumerator SendAIResponse(string userInput)
    {
        yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));

        string reply = HandleConversation(userInput);

        int loops = Mathf.Clamp((reply.Length / 20) + Random.Range(0, 3), 2, 10);

        yield return StartCoroutine(AnimateDots(loops));

        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

        CreateMessage(reply, false);
        SaveConversation(reply, false);

        yield return StartCoroutine(SmartScroll());
    }

    IEnumerator AnimateDots(int loops)
    {
        typingUI.SetActive(true);

        for (int i = 0; i < loops; i++)
        {
            dot1.enabled = true;
            dot2.enabled = false;
            dot3.enabled = false;
            yield return new WaitForSeconds(0.3f);

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = false;
            yield return new WaitForSeconds(0.3f);

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = true;
            yield return new WaitForSeconds(0.3f);

            dot1.enabled = false;
            dot2.enabled = false;
            dot3.enabled = false;
        }

        typingUI.SetActive(false);
    }

    string HandleConversation(string input)
    {
        string lower = input.ToLower();

        if (reconnecting)
        {
            reconnecting = false;

            if (ContainsAny(lower, "good", "fine", "okay"))
            {
                return "good, just making sure. what have you been up to?";
            }

            if (ContainsAny(lower, "tired", "meh", "bored"))
            {
                return "yeah you sound it. been a long few days has it?";
            }

            return "alright, just checking in. what have you been up to?";
        }

        if (waitingForName && string.IsNullOrEmpty(userName))
        {
            userName = CleanName(input);
            PlayerPrefs.SetString("UserName", userName);

            waitingForName = false;
            inNameLoop = true;
            nameChangeCount = 0;

            return "alright " + userName + ". saved.";
        }

        if (inNameLoop && ContainsAny(lower, "call me", "jk", "changed", "nah"))
        {
            string newName = ExtractNameFlexible(input);
            nameChangeCount++;

            if (nameChangeCount < 5)
            {
                return "alright " + newName + " then";
            }
            else
            {
                userName = CleanName(newName);
                PlayerPrefs.SetString("UserName", userName);

                inNameLoop = false;
                convoState = 1;

                return "okay i'm saving it as " + userName;
            }
        }

        if (inNameLoop && ContainsAny(lower, "stick", "keep", "that one", "this one"))
        {
            inNameLoop = false;
            convoState = 1;

            return "alright, sticking with " + userName + ". how are you anyway?";
        }

        if (inNameLoop && IsLikelyName(input))
        {
            userName = CleanName(input);
            PlayerPrefs.SetString("UserName", userName);

            inNameLoop = false;
            convoState = 1;

            return "alright " + userName + ". locking that in now";
        }

        if (inNameLoop)
        {
            return "you changing it again or sticking with that one?";
        }

        if (ContainsAny(lower, "call me"))
        {
            string newName = ExtractNameFlexible(input);

            userName = CleanName(newName);
            PlayerPrefs.SetString("UserName", userName);

            return "okay i'll call you " + userName + ". how are you anyway?";
        }

        if (convoState == 5)
        {
            if (ContainsFuzzy(lower, "chocolate", "cake", "biscuits", "cookies", "crisps"))
            {
                convoState = 6;
                return "i'm sure i can get that in for you";
            }

            if (ContainsFuzzy(lower, "poo", "glass", "dog"))
            {
                convoState = 6;
                return "alright enough messing about";
            }

            return "what do you fancy?";
        }

        return GenerateReply(input);
    }

    // ---------- SPELLING FIX ----------

    int LevenshteinDistance(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;

                dp[i, j] = Mathf.Min(
                    dp[i - 1, j] + 1,
                    dp[i, j - 1] + 1,
                    dp[i - 1, j - 1] + cost
                );
            }
        }

        return dp[a.Length, b.Length];
    }

    bool ContainsFuzzy(string input, params string[] words)
    {
        string[] parts = input.Split(' ');

        foreach (string p in parts)
        {
            foreach (string w in words)
            {
                if (p.Contains(w) || LevenshteinDistance(p, w) <= 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ---------- HELPERS ----------

    bool IsLikelyName(string input)
    {
        string t = input.Trim();

        if (t.Length > 12) return false;
        if (t.Contains(" ")) return false;

        if (ContainsAny(t.ToLower(), "ok", "yeah", "yes", "no"))
        {
            return false;
        }

        return true;
    }

    bool ContainsAny(string input, params string[] words)
    {
        foreach (string w in words)
        {
            if (input.Contains(w)) return true;
        }
        return false;
    }

    string GenerateReply(string input)
    {
        return RandomChoice(
            "i get what you're saying",
            "yeah that makes sense",
            "fair enough",
            "say more"
        );
    }

    string ExtractNameFlexible(string input)
    {
        string lower = input.ToLower();

        if (lower.Contains("call me"))
        {
            int i = lower.IndexOf("call me");
            return input.Substring(i + 7).Trim();
        }

        string[] parts = input.Split(' ');
        return parts[parts.Length - 1];
    }

    string CleanName(string raw)
    {
        raw = raw.Trim();
        string[] parts = raw.Split(' ');
        string name = parts[0];

        return char.ToUpper(name[0]) + name.Substring(1);
    }

    string RandomChoice(params string[] options)
    {
        return options[Random.Range(0, options.Length)];
    }

    void CreateMessage(string text, bool isUser)
    {
        GameObject prefab = isUser ? userContainerPrefab : aiContainerPrefab;

        GameObject container = Instantiate(prefab, content);
        TextMeshProUGUI textComp = container.GetComponentInChildren<TextMeshProUGUI>();

        textComp.text = text;

        StartCoroutine(SmartScroll());
    }

    IEnumerator SmartScroll()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        if (userAtBottom)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    void SaveConversation(string message, bool isUser)
    {
        string entry = (isUser ? "U:" : "A:") + message + "||";
        PlayerPrefs.SetString(saveKey, PlayerPrefs.GetString(saveKey) + entry);
    }

    void LoadConversation()
    {
        string history = PlayerPrefs.GetString(saveKey, "");

        if (string.IsNullOrEmpty(history)) return;

        string[] messages = history.Split("||");

        foreach (string msg in messages)
        {
            if (string.IsNullOrEmpty(msg)) continue;

            bool isUser = msg.StartsWith("U:");
            string text = msg.Substring(2);

            CreateMessage(text, isUser);
        }
    }

    public void SendAIImmediate(string message)
    {
        CreateMessage(message, false);
        SaveConversation(message, false);
    }

    public void ResetChat()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        StopAllCoroutines();

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        recentMessages.Clear();

        userName = "";
        waitingForName = true;
        inNameLoop = false;
        reconnecting = false;
        nameChangeCount = 0;
        convoState = 0;

        SendAIImmediate("hey kid! it's uncle bob. i'm just re-adding everyone's numbers because i swapped sims... what do you want me to save you as?");
    }
}