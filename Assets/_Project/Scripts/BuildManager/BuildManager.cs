using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] private PlayerInput _inputHandler;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _terrainMask;

    private SO_TowerData _selectedTowerData;
    private GameObject _currentGhostTower;

    [SerializeField] private LayerMask _layerObstaclesMask;
    [SerializeField] private float placementRadius = 1f;

    private bool canPlaceHere;
    private Vector3 currentPlacementPosition;

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
        if (!IsBuildMode()) return;
        if (_currentGhostTower == null) return;
        if (_selectedTowerData == null) return;

        if (!canPlaceHere)
        {
            AudioManager.Instance.PlaySFX("WrongPlace");
            Debug.Log("You can't place tower here !!!!");
            return;
        }

        if (!GameManager.Instance.CanAddOtherTower(1))
        {
            AudioManager.Instance.PlaySFX("WrongPlace");
            Debug.Log("You can't place tower !!!!");
            CancelPlacement();
            return;
        }

        bool isTowerPlaced = TowerPlacement(currentPlacementPosition);
        if (isTowerPlaced)
        {
            AudioManager.Instance.PlaySFX("TowerPlaced");
            CancelPlacement(); // torre piazzata ora esco dalla modalità Build Mode !!!
        }
        else
        {
            Debug.Log("You can't place tower !!!!");
        }
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

    private bool TowerPlacement(Vector3 Position)
    {
        if (_selectedTowerData == null) return false;
        if (!GameManager.Instance.CanSpendCoins(_selectedTowerData.goldPrice))
        {
            Debug.Log("You don't have money !!!!");
            return false;
        }

        GameManager.Instance.SpendCoins(_selectedTowerData.goldPrice);

        GameObject clonedTower = Instantiate(_selectedTowerData.towerPrefab);
        clonedTower.gameObject.GetComponent<Tower>().Initialize(_selectedTowerData, _selectedTowerData.bulletdata); ;
        clonedTower.transform.position = Position;



        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentPlacementPosition, placementRadius);
    }

    private bool CanPlaceTower(Vector3 position)
    {
        if (_selectedTowerData == null) return false;

        if (!GameManager.Instance.CanSpendCoins(_selectedTowerData.goldPrice))
        {
            Debug.Log("You don't have money !!!!");
            return false;
        }

        bool isPlacementBlocked = Physics.CheckSphere(position, placementRadius, _layerObstaclesMask, QueryTriggerInteraction.Ignore);
        if (isPlacementBlocked)
        {
            return false;  // you can't place tower here !!
        }

        return true; // ok you can place tower here !!
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.TowerPlacing) return;
        if (_selectedTowerData == null) return;
        if (_currentGhostTower == null) return;

        UpdateGhostTowerPosition();
    }

    private void UpdateGhostTowerPosition()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, _terrainMask, QueryTriggerInteraction.Ignore))
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
        else
        { // Raycast don't hit valid terrainMask !!!
            if (_currentGhostTower != null)
            {
                _currentGhostTower.SetActive(false);
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
