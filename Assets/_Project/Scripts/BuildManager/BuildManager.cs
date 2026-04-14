using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] private PlayerInput _inputHandler;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask terrainMask;

    private SO_TowerData _selectedTowerData;
    private GameObject _currentGhostTower;

    [SerializeField] private LayerMask _layerObstaclesMask;
    [SerializeField] private float placementRadius = 1f;
    private Vector3 currentPlacementPosition;
    private bool canPlaceHere;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_cam == null) _cam = Camera.main;
    }

    public bool IsBuildMode()
    {
        if (GameManager.Instance == null)
        {
            return false;
        }
        if (GameManager.Instance.CurrentState == GameManager.GameState.TowerPlacing)
        {
            return true;
        }

        return false;
    }

    public void EnterBuildMode()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.EnterTowerPlacing();

        _inputHandler.SwitchCurrentActionMap("BuildMode");
    }

    public void ExitBuildMode()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.ExitTowerPlacing();

        _inputHandler.SwitchCurrentActionMap("Gameplay");
    }

    public void OnLeftMouseClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;



    }

    public void OnRightMouseClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;

        if (!IsBuildMode()) return;

        CancelPlacement();
    }

    public void StartPlacement(SO_TowerData towerData)
    {
        if (_currentGhostTower != null)
        {
            Destroy(_currentGhostTower);
            _currentGhostTower = null;
        }

        _selectedTowerData = towerData;
        if (_selectedTowerData == null) return;

        if (_selectedTowerData.ghostPrefab != null)
        {
            _currentGhostTower = Instantiate(_selectedTowerData.ghostPrefab);
            _currentGhostTower.SetActive(false);
        }

        EnterBuildMode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentPlacementPosition, placementRadius);
    }

    private bool CanPlaceTower(Vector3 position)
    {
        if (!GameManager.Instance.CanSpendCoins(_selectedTowerData.goldPrice))
        {
            Debug.Log("You don't have money !!!!");
            return false;
        }

        bool isPlacementBlocked = Physics.CheckSphere(position, placementRadius, _layerObstaclesMask, QueryTriggerInteraction.Ignore);
        if (isPlacementBlocked)
        {
            return false;  // you can't build tower here !!
        }

        return true; // ok you can build tower here !!
    }

    private void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is NULL!!!");
            return;
        }

        if (GameManager.Instance.CurrentState != GameManager.GameState.TowerPlacing) return;

        if (_selectedTowerData == null)
        {
            Debug.LogError("_selectedTowerData is NULL !!!");
            return;
        }

        if (_currentGhostTower == null)
        {
            Debug.LogError("_currentGhostTower is NULL !!!");
            return;
        }

        UpdateGhostTowerPosition();
    }

    private void UpdateGhostTowerPosition()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, terrainMask, QueryTriggerInteraction.Ignore))
        {
            currentPlacementPosition = hit.point;

            if (_currentGhostTower != null)
            {
                _currentGhostTower.SetActive(true);
                _currentGhostTower.transform.SetPositionAndRotation(currentPlacementPosition, Quaternion.identity);

                canPlaceHere = CanPlaceTower(currentPlacementPosition);

                if (canPlaceHere)
                {
                    foreach (Renderer r in _currentGhostTower.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.green;
                    }
                }
                else
                {
                    foreach (Renderer r in _currentGhostTower.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.red;
                    }
                }

            }
        }
    }

    private void CancelPlacement()
    {
        _selectedTowerData = null;

        if (_currentGhostTower != null)
        {
            Destroy(_currentGhostTower);
            _currentGhostTower = null;
        }

        ExitBuildMode();
    }
}
