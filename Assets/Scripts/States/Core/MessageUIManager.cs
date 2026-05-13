using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUIManager : MonoBehaviour
{
    public Transform content;

    public GameObject userContainerPrefab;

    public GameObject aiContainerPrefab;

    public ScrollRect scrollRect;

    bool userAtBottom = true;

    public void SetupScrollListener()
    {
        scrollRect.onValueChanged.AddListener(
            OnScrollChanged
        );
    }

    void OnScrollChanged(
        Vector2 pos
    )
    {
        userAtBottom =
            scrollRect.verticalNormalizedPosition
            <= 0.05f;
    }

    public void CreateMessage(
        string text,
        bool isUser,
        MonoBehaviour runner
    )
    {
        GameObject prefab =
            isUser
            ? userContainerPrefab
            : aiContainerPrefab;

        GameObject container =
            Instantiate(
                prefab,
                content
            );

        TextMeshProUGUI textComp =
            container.GetComponentInChildren<TextMeshProUGUI>();

        textComp.text = text;

        runner.StartCoroutine(
            SmartScroll()
        );
    }

    IEnumerator SmartScroll()
    {
        yield return null;

        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        if (userAtBottom)
        {
            scrollRect.verticalNormalizedPosition =
                0f;
        }
    }

    public void ClearMessages()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}