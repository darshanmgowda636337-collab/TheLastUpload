using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MainMenuIntro : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer introVideo;
    public CanvasGroup titleGroup;
    public CanvasGroup buttonGroup;
    public Button playButton;
    public AudioSource audioSource;

    [Header("Timing")]
    public float titleFadeDuration = 1.5f;
    public float buttonDelay = 0.7f;
    public float buttonFadeDuration = 0.5f;

    private AudioClip introBeep;
    private AudioClip clickBeep;
    private bool introFinished;

    void Start()
    {
        Debug.Log("MainMenuIntro started.");
        if (introVideo == null || titleGroup == null ||
            buttonGroup == null || playButton == null)
        {
            Debug.LogError(
                "MainMenuIntro: Inspector references are missing!"
            );
            enabled = false;
            return;
        }

        titleGroup.alpha = 0f;
        buttonGroup.alpha = 0f;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;

        introBeep = MakeBeep(660f, 0.12f);
        clickBeep = MakeBeep(950f, 0.09f);

        introVideo.isLooping = false;
        introVideo.loopPointReached += OnVideoFinished;
        playButton.onClick.AddListener(OnPlayClicked);

        if (!introVideo.isPlaying)
            introVideo.Play();
    }

    void OnVideoFinished(VideoPlayer source)
{
    Debug.Log("Intro video finished event received.");

    if (introFinished) return;

    introFinished = true;
    StartCoroutine(RevealMenu());
}
    IEnumerator RevealMenu()
    {
        Debug.Log("RevealMenu started.");
        if (audioSource != null)
            audioSource.PlayOneShot(introBeep, 0.25f);

        yield return StartCoroutine(
            Fade(titleGroup, 0f, 1f, titleFadeDuration)
        );
        Debug.Log("Title fade completed. Alpha = " + titleGroup.alpha);

        TMP_Text title = titleGroup.GetComponent<TMP_Text>();

        if (title != null)
        {
            for (int i = 0; i < 4; i++)
            {
                title.alpha = 0.35f;
                yield return new WaitForSeconds(0.045f);
                title.alpha = 1f;
                yield return new WaitForSeconds(0.055f);
            }
        }

        yield return new WaitForSeconds(buttonDelay);

        yield return StartCoroutine(
    Fade(buttonGroup, 0f, 1f, buttonFadeDuration)
);

Debug.Log("Button fade completed. Alpha = " + buttonGroup.alpha);

buttonGroup.interactable = true;
buttonGroup.blocksRaycasts = true;
    }

    IEnumerator Fade(
        CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        group.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            group.alpha = Mathf.Lerp(
                from, to, Mathf.Clamp01(elapsed / duration)
            );

            yield return null;
        }

        group.alpha = to;
    }

    void OnPlayClicked()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(clickBeep, 0.5f);

        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.15f);

        SceneManager.LoadScene("MainScene");
    }

    AudioClip MakeBeep(float frequency, float duration)
    {
        int sampleRate = 44100;
        int count = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[count];

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = 1f - (float)i / count;

            samples[i] = Mathf.Sin(
                2f * Mathf.PI * frequency * t
            ) * envelope * 0.18f;
        }

        AudioClip clip = AudioClip.Create(
            "CyberUIBeep", count, 1, sampleRate, false
        );

        clip.SetData(samples, 0);
        return clip;
    }

    void OnDestroy()
    {
        if (introVideo != null)
            introVideo.loopPointReached -= OnVideoFinished;

        if (playButton != null)
            playButton.onClick.RemoveListener(OnPlayClicked);
    }
}