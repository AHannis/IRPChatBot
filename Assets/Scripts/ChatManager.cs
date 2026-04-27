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
    bool didNameJoke = false;
    bool postNameFollowUp = false;

    void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChanged);

        LoadConversation();

        userName = PlayerPrefs.GetString("UserName", "");

        if (!PlayerPrefs.HasKey("HasChattedBefore"))
        {
            SendAIImmediate("hey kid! it's uncle bob. i'm just re-adding everyone's numbers because i swapped sims... i dunno, i don't understand this modern technology. what do you want me to save you as?");
            PlayerPrefs.SetInt("HasChattedBefore", 1);
            waitingForName = true;
        }
        else
        {
            SendAIImmediate("there you are. thought you disappeared on me :)");
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
        if (scrollRect.verticalNormalizedPosition <= 0.05f)
        {
            userAtBottom = true;
        }
        else
        {
            userAtBottom = false;
        }
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

        if (postNameFollowUp)
        {
            postNameFollowUp = false;

            yield return new WaitForSeconds(Random.Range(0.6f, 1.2f));

            CreateMessage(RandomChoice(
                "so how have you been anyway",
                "what have you been up to lately",
                "feels like i haven't seen you in ages"
            ), false);
        }
        else
        {
            if (Random.value < 0.2f)
            {
                yield return new WaitForSeconds(Random.Range(0.6f, 1.2f));

                CreateMessage(RandomChoice(
                    "actually ignore me i read that wrong",
                    "nah wait i get you now",
                    "i'm overthinking that aren't i",
                    "you know what i mean anyway"
                ), false);
            }
        }
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

            if (Random.value < 0.3f)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
            }
        }

        typingUI.SetActive(false);
    }

    string HandleConversation(string input)
    {
        string lower = input.ToLower();

        if (waitingForName && string.IsNullOrEmpty(userName))
        {
            userName = ExtractName(input);
            PlayerPrefs.SetString("UserName", userName);

            waitingForName = false;
            didNameJoke = false;
            postNameFollowUp = true;

            return "alright " + userName + ". saved. try not to go changing it every five minutes";
        }

        if (lower.Contains("actually call me") && !didNameJoke)
        {
            didNameJoke = true;
            return "okay i'll call you actually :)";
        }

        if ((lower.Contains("my name is") || lower.Contains("call me")) && didNameJoke)
        {
            string newName = ExtractName(input);
            userName = newName;
            PlayerPrefs.SetString("UserName", userName);

            didNameJoke = false;

            return "oh c'mon it was a joke. ofc i'll put your name in my phonebook as " + userName;
        }

        if (postNameFollowUp)
        {
            return RandomChoice(
                "we're all busy i get it, just don't forget to drop in sometime",
                "i know how it is, everyone's got their own stuff going on",
                "life gets in the way doesn't it, still good to hear from you though"
            );
        }

        return GenerateReply(input);
    }

    string ExtractName(string input)
    {
        input = input.ToLower();

        if (input.Contains("call me"))
        {
            return CleanName(input.Substring(input.IndexOf("call me") + 7));
        }

        if (input.Contains("name is"))
        {
            return CleanName(input.Substring(input.IndexOf("name is") + 7));
        }

        return CleanName(input);
    }

    string CleanName(string raw)
    {
        raw = raw.Trim();

        string[] parts = raw.Split(' ');

        if (parts.Length == 0)
        {
            return "kid";
        }

        string name = parts[0];

        name = name.Replace(".", "").Replace(",", "").Replace("!", "").Replace("?", "");

        return char.ToUpper(name[0]) + name.Substring(1);
    }

    string GenerateReply(string input)
    {
        input = input.ToLower();

        string prefix = "";

        if (!string.IsNullOrEmpty(userName))
        {
            prefix = userName + ", ";
        }

        if (input.Contains("work"))
        {
            return prefix + RandomChoice(
                "yeah work never really changes does it. same thing different day",
                "feels like all anyone does lately is work and sleep",
                "i remember when work stayed at work, now it just follows you home"
            );
        }

        if (input.Contains("weather"))
        {
            return prefix + RandomChoice(
                "weather's been weird lately hasn't it",
                "i swear it was freezing this morning and now it's warm again",
                "can't even dress properly for it anymore, changes every five minutes"
            );
        }

        if (input.Contains("tired") || input.Contains("stress"))
        {
            return prefix + RandomChoice(
                "yeah i can hear that in how you're saying it",
                "sounds like you've got a lot going on there",
                "that's not easy to deal with, you alright?"
            );
        }

        if (Random.value < 0.15f)
        {
            return prefix + RandomChoice(
                "hang on i might've read that wrong",
                "wait what do you mean by that exactly",
                "you've lost me a bit there i'm not gonna lie"
            );
        }

        if (recentMessages.Count > 0 && Random.value < 0.25f)
        {
            string memory = recentMessages[Random.Range(0, recentMessages.Count)];
            return prefix + "earlier you said \"" + memory + "\" — did anything come of that?";
        }

        if (Random.value < 0.2f)
        {
            return prefix + RandomChoice(
                "you know what, i was thinking about that earlier actually",
                "funny you say that, reminds me of something",
                "i get what you mean though, makes sense when you think about it"
            );
        }

        return prefix + RandomChoice(
            "yeah that sounds about right",
            "i get what you're saying",
            "makes sense when you put it like that",
            "fair enough, can't argue with that"
        );
    }

    string RandomChoice(params string[] options)
    {
        return options[Random.Range(0, options.Length)];
    }

    void CreateMessage(string text, bool isUser)
    {
        GameObject prefab;

        if (isUser)
        {
            prefab = userContainerPrefab;
        }
        else
        {
            prefab = aiContainerPrefab;
        }

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
        string entry;

        if (isUser)
        {
            entry = "U:" + message + "||";
        }
        else
        {
            entry = "A:" + message + "||";
        }

        PlayerPrefs.SetString(saveKey, PlayerPrefs.GetString(saveKey) + entry);
    }

    void LoadConversation()
    {
        string history = PlayerPrefs.GetString(saveKey, "");

        if (string.IsNullOrEmpty(history))
        {
            return;
        }

        string[] messages = history.Split("||");

        foreach (string msg in messages)
        {
            if (string.IsNullOrEmpty(msg))
            {
                continue;
            }

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

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        recentMessages.Clear();

        userName = "";
        waitingForName = true;
        didNameJoke = false;
        postNameFollowUp = false;

        SendAIImmediate("hey kid! it's uncle bob. i'm just re-adding everyone's numbers because i swapped sims... i dunno, i don't understand this modern technology. what do you want me to save you as?");
    }
}