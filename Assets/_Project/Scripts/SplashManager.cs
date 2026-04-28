using System.Collections;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float _splashDuration = 5f;
    [SerializeField] private Object _sceneAsset;

    public AudioManager AudioManager;

    private void Awake()
    {
        AudioManager = Resources.Load<AudioManager>("AudioManager");
        Instantiate(AudioManager);
    }

    private IEnumerator Start()
    {
        AudioManager.Instance.PlaySFX("SplashScreenSound");

        yield return new WaitForSeconds(_splashDuration);

        string nextScene = _sceneAsset.name;
        ScreenFader.Instance.FadeToBlackAndLoadScene(nextScene);
    }
}