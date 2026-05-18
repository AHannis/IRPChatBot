using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUIManager : MonoBehaviour
{
    //main content parent for chat bubbles
    public Transform content;

    //prefabs for user + ai messages
    public GameObject userContainerPrefab;

    public GameObject aiContainerPrefab;

    //main scroll view
    public ScrollRect scrollRect;

    //optional message send sound
    public AudioSource messageAudio;

    //tracks if player is reading older messages
    bool userAtBottom = true;

    //stores previous message to avoid accidental duplicates
    string lastMessage = "";

    void Awake()
    {
        //starts chat at bottom
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition =
            0f;
    }

    //tracks scroll position
    public void SetupScrollListener()
    {
        scrollRect.onValueChanged.AddListener(
            OnScrollChanged
        );
    }

    //detects whether player is near bottom of chat
    void OnScrollChanged(
        Vector2 pos
    )
    {
        userAtBottom =
            scrollRect.verticalNormalizedPosition
            <= 0.05f;
    }

    //main chat bubble creation system
    //message timing + presentation help reinforce realism
    public void CreateMessage(
        string text,
        bool isUser,
        MonoBehaviour runner
    )
    {
        //small duplicate prevention safety
        if (
            text == lastMessage
            && Random.value < 0.08f
        )
        {
            return;
        }

        lastMessage =
            text;

        GameObject prefab =
            isUser
            ? userContainerPrefab
            : aiContainerPrefab;

        GameObject container =
            Instantiate(
                prefab,
                content
            );

        //gets message text component
        TextMeshProUGUI textComp =
            container.GetComponentInChildren<TextMeshProUGUI>();

        //instant message appearance like real texting apps
        textComp.text =
            text;

        //forces proper bubble resizing
        LayoutRebuilder
            .ForceRebuildLayoutImmediate(
                content
                .GetComponent<RectTransform>()
            );

        //optional subtle send sound
        if (
            messageAudio != null
        )
        {
            messageAudio.pitch =
                Random.Range(
                    0.96f,
                    1.04f
                );

            messageAudio.Play();
        }

        //smart autoscroll system
        runner.StartCoroutine(
            SmartScroll()
        );
    }

    //only scrolls if player already near bottom
    IEnumerator SmartScroll()
    {
        yield return null;

        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder
            .ForceRebuildLayoutImmediate(
                content
                .GetComponent<RectTransform>()
            );

        //prevents interrupting player reading old messages
        if (userAtBottom)
        {
            scrollRect.verticalNormalizedPosition =
                0f;
        }
    }

    //clears all message bubbles
    public void ClearMessages()
    {
        foreach (
            Transform child
            in content
        )
        {
            Destroy(
                child.gameObject
            );
        }

        lastMessage = "";
    }
}