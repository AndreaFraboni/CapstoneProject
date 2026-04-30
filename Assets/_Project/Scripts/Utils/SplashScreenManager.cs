using System.Collections;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    public AudioManager AudioManager;

    [SerializeField] private string _nextSceneAssetName;
    public float _splashDuration = 5f;

    private void Awake()
    {
        AudioManager = Resources.Load<AudioManager>("AudioManager");
        Instantiate(AudioManager);
    }

    private IEnumerator Start()
    {
        AudioManager.Instance.PlaySFX("SplashScreenSound");

        yield return new WaitForSeconds(_splashDuration);

        if (_nextSceneAssetName != null)
        {
            ScreenManager.Instance.LoadNextScene(_nextSceneAssetName);
        }
        else
        {
            Debug.LogError("Next Scene Asset is not assigned !!!!!");
        }
    }

}
