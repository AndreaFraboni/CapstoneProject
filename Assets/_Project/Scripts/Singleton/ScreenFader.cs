using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private static ScreenFader _instance;

    public static ScreenFader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ScreenFader>(FindObjectsInactive.Include); // se gameobject in scena disattivato lui lo cerca e lo attiva
                if (_instance != null)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            Debug.Log($"{gameObject.name} registered as instance for {nameof(ScreenFader)}");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.Log($"{gameObject.name} tried to register ad instance for {nameof(ScreenFader)} but there is already {_instance.gameObject.name}");
            Destroy(gameObject);
        }
    }

    public void StartFadeToOpaque()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
    }

    public void FadeToTransparent()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
    }



}
