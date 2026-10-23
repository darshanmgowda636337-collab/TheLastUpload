using System.Collections;
using UnityEngine;
using TMPro;

public class TitleGlitchEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private float glitchInterval = 3f;

    private Color originalColor;
    private Coroutine glitchRoutine;

    void Start()
    {
        if (title == null)
            title = GetComponent<TMP_Text>();

        if (title == null)
        {
            Debug.LogError("TitleGlitchEffect: TMP title reference missing.");
            enabled = false;
            return;
        }

        originalColor = title.color;
        glitchRoutine = StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(glitchInterval);

            // Brief digital flicker, without changing layout or text.
            for (int i = 0; i < 3; i++)
            {
                title.alpha = 0.45f;
                yield return new WaitForSeconds(0.045f);

                title.alpha = 1f;
                yield return new WaitForSeconds(0.055f);
            }
        }
    }

    void OnDisable()
    {
        if (title != null)
            title.alpha = 1f;

        if (glitchRoutine != null)
            StopCoroutine(glitchRoutine);
    }
}