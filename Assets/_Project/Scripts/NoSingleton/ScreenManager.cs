using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (ScreenFader.Instance == null)
        {
            ScreenFader prefab = Resources.Load<ScreenFader>("ScreenFader");

            if (prefab == null)
            {
                Debug.LogError("ScreenFader prefab non trovato. Mettilo in Assets/Resources/ScreenFader.prefab");
                return;
            }

            Instantiate(prefab);
        }
    }

    public void LoadNextScene(string sceneName)
    {
        if (ScreenFader.Instance == null)
        {
            Debug.LogError("ScreenFader.Instance è null");
            return;
        }

        ScreenFader.Instance.FadeToBlackAndLoadScene(sceneName);
    }

}