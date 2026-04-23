using UnityEngine;

public class SacredTreeController : MonoBehaviour, ILiveCheckable
{
    [SerializeField] private LifeController _lifeController;

    public bool isAlive = true;

    public bool isAliveState()
    {
        return isAlive;
    }

    private void Awake()
    {
        if (_lifeController == null) _lifeController = GetComponent<LifeController>();
    }

    private void OnEnable()
    {
        if (_lifeController != null) _lifeController.OnDefeated += OnDefeated;
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnDefeated -= OnDefeated;
    }


    public void OnDefeated()
    {
        isAlive = false;
        AudioManager.Instance.PlaySFX("DeathSound");

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is NULL!!!");
            return;
        }
        GameManager.Instance.GameOver();
    }
}
