using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    private static ScreenFader _instance;

    public static ScreenFader Instance
    {
        get { return _instance; }
    }

    public CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private Action _onFadeComplete;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
        _canvasGroup.blocksRaycasts = false; // attiva click su quello che c'è sotto
        _canvasGroup.interactable = false;   // attiva input
    }

    public void StartFadeToOpaque(Action onFadeComplete = null)
    {
        StopAllCoroutines();
        _onFadeComplete = onFadeComplete;
        StartCoroutine(FadeCoroutine(0, 1, _fadeDuration, onFadeComplete));
    }

    public void StartFadeToTransparent(Action onFadeComplete = null)
    {
        StopAllCoroutines();
        _onFadeComplete = onFadeComplete;
        StartCoroutine(FadeCoroutine(1, 0, _fadeDuration, onFadeComplete));
    }

    public void FadeToBlackAndLoadScene(string sceneName)
    {
        StartFadeToOpaque(() =>
        {
            StartCoroutine(Load(sceneName));
        });
    }

    private IEnumerator Load(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return null;

        StartFadeToTransparent();
    }

    private IEnumerator FadeCoroutine(float startValue, float endValue, float duration, Action callback)
    {
        _canvasGroup.alpha = startValue;

        _canvasGroup.blocksRaycasts = true; // blocca click su quello che c'è sotto 
        _canvasGroup.interactable = true;  // blocca input

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, timer / duration);
            yield return null;
        }

        _canvasGroup.alpha = endValue;

        if (endValue <= Mathf.Epsilon)
        {
            _canvasGroup.blocksRaycasts = false; // riattiva click su quello che c'è sotto
            _canvasGroup.interactable = false;   // riattiva input
        }

        callback?.Invoke();
    }
}