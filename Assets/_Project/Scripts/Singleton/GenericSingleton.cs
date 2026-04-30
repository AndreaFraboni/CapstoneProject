using UnityEngine;

public abstract class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    //private static bool isApplicationQuit = false;

    protected virtual bool ShouldBeDestroyOnLoad() => false;
    
    public static T Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
                //if (_instance == null)
                //{
                //    Debug.LogError($"Nessuna istanza di {typeof(T).Name} trovata in scena!");
                //}
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            if (!ShouldBeDestroyOnLoad())
                DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    //protected virtual void OnApplicationQuit()
    //{
    //    isApplicationQuit = true;
    //}


}