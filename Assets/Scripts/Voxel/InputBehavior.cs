using UnityEngine;
using UnityEngine.Events;
public class InputBehavior : MonoBehaviour
{
    [SerializeField] private UnityEvent<RaycastHit> OnTryToPlaceBlock;
    [SerializeField] private UnityEvent<RaycastHit> OnTryToRemoveBlock;

    void Update()
    {
        if (GameStateManager.CurrentState.StateName != "Game")
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                OnTryToPlaceBlock?.Invoke(hit);
            else if (Input.GetKeyDown(KeyCode.Mouse1))
                OnTryToRemoveBlock?.Invoke(hit);
        }

    }


}
