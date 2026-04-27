using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypingDotsImage : MonoBehaviour
{
    public Image dotImage;

    public Sprite frame1;
    public Sprite frame2;
    public Sprite frame3;

    public int loops = 1;

    void OnEnable()
    {
        loops = Random.Range(1, 4); // 1–3 cycles based on "thinking"
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < loops; i++)
        {
            dotImage.sprite = frame1;
            yield return new WaitForSeconds(0.35f);

            dotImage.sprite = frame2;
            yield return new WaitForSeconds(0.35f);

            dotImage.sprite = frame3;
            yield return new WaitForSeconds(0.35f);
        }
    }
}