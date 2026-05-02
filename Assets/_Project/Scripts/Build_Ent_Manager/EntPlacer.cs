using UnityEngine;
using UnityEngine.InputSystem;

public class EntPlacer : MonoBehaviour
{
    public static EntPlacer Instance { get; private set; }

    [SerializeField] private PlayerInput _inputHandler;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _terrainMask;

    private SO_EntData _selectedEntData;
    private GameObject _currentGhostEntPrefab;

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

    public bool IsEntPlacingMode()
    {
        if (GameManager.Instance == null)
        {
            return false;
        }
        if (GameManager.Instance.CurrentState == GameState.EntPlacing)
        {
            return true;
        }

        return false;
    }

    public void EnterEntPlacingMode()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.EnterEntPlacing();

        if (_inputHandler != null) _inputHandler.SwitchCurrentActionMap("EntPlacingMode");
    }

    public void ExitEntPlacingMode()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.ExitEntPlacing();

        if (_inputHandler != null) _inputHandler.SwitchCurrentActionMap("Gameplay");
    }

    public void OnLeftMouseClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;
        if (!IsEntPlacingMode()) return;
        if (_currentGhostEntPrefab == null) return;
        if (_selectedEntData == null) return;

        if (!canPlaceHere)
        {
            AudioManager.Instance.PlaySFX("WrongPlace");
            return;
        }

        bool isEntPlaced = EntPlacement(currentPlacementPosition);

        if (isEntPlaced)
        {
            AudioManager.Instance.PlaySFX("EntEvocation");
            CancelPlacement(); // Ent piazzato ora esco dalla modalità Build Mode !!!
        }
        else
        {
            //Debug.Log("You can't place Ent !!!!");
        }
    }

    public void OnRightMouseClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;

        if (!IsEntPlacingMode()) return;

        CancelPlacement();
    }

    public void StartPlacement(SO_EntData EntData)
    {
        if (BuildManager.Instance != null && BuildManager.Instance.IsBuildMode())
        {
            BuildManager.Instance.CancelPlacement();
        }

        if (_currentGhostEntPrefab != null)
        {
            Destroy(_currentGhostEntPrefab);
            _currentGhostEntPrefab = null;
        }

        _selectedEntData = EntData;
        if (_selectedEntData == null) return;

        if (_selectedEntData.ghostPrefab != null)
        {
            _currentGhostEntPrefab = Instantiate(_selectedEntData.ghostPrefab);
            _currentGhostEntPrefab.SetActive(false);
        }

        EnterEntPlacingMode();
    }

    private bool EntPlacement(Vector3 Position)
    {
        if (_selectedEntData == null) return false;

        if (!GameManager.Instance.CanSpendCoins(_selectedEntData.goldPrice) || !GameManager.Instance.CanSpendBlueGems(_selectedEntData.blueGemsPrice))
        {
            Debug.Log("You don't have money & Blue gems needed !!!!");
            return false;
        }

        GameManager.Instance.SpendCoins(_selectedEntData.goldPrice);
        GameManager.Instance.SpendBlueGems(_selectedEntData.blueGemsPrice);

        GameObject clonedEnt = Instantiate(_selectedEntData.EntPrefab);
        clonedEnt.transform.position = Position;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentPlacementPosition, placementRadius);
    }

    private bool CanPlaceEnt(Vector3 position)
    {
        if (_selectedEntData == null) return false;

        if (!GameManager.Instance.CanSpendCoins(_selectedEntData.goldPrice) || !GameManager.Instance.CanSpendBlueGems(_selectedEntData.blueGemsPrice))
        {
            Debug.Log("You don't have money & Blue gems needed !!!!");
            return false;
        }

        bool isPlacementBlocked = Physics.CheckSphere(position, placementRadius, _layerObstaclesMask, QueryTriggerInteraction.Ignore);
        if (isPlacementBlocked)
        {
            return false;  // you can't place Ent here !!
        }

        return true; // ok you can place Ent here !!
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.CurrentState != GameState.EntPlacing) return;
        if (_selectedEntData == null) return;
        if (_currentGhostEntPrefab == null) return;

        UpdateGhostEntPosition();
    }

    private void UpdateGhostEntPosition()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, _terrainMask, QueryTriggerInteraction.Ignore))
        {
            currentPlacementPosition = hit.point;

            if (_currentGhostEntPrefab != null)
            {
                _currentGhostEntPrefab.SetActive(true);
                _currentGhostEntPrefab.transform.SetPositionAndRotation(currentPlacementPosition, Quaternion.identity);

                canPlaceHere = CanPlaceEnt(currentPlacementPosition);

                if (canPlaceHere)
                {
                    foreach (Renderer r in _currentGhostEntPrefab.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.green;
                    }
                }
                else
                {
                    foreach (Renderer r in _currentGhostEntPrefab.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.red;
                    }
                }

            }
        }
        else
        { // Raycast don't hit valid terrainMask !!!
            if (_currentGhostEntPrefab != null)
            {
                _currentGhostEntPrefab.SetActive(false);
            }
        }
    }

    public void CancelPlacement()
    {
        _selectedEntData = null;

        if (_currentGhostEntPrefab != null)
        {
            Destroy(_currentGhostEntPrefab);
            _currentGhostEntPrefab = null;
        }

        ExitEntPlacingMode();
    }
}
