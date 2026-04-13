using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _inputHandler;

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

}
