using UnityEditor;
using UnityEngine;

public class BrainHUD : MonoBehaviour
{
    private IBrainViewable selectedBrain;

    public void Update()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.TryGetComponent<Creature>(out Creature newSelection))
            {
                IBrainViewable newlySelectedBrain = newSelection.GetComponentInChildren<IBrainViewable>();
                if (newlySelectedBrain != selectedBrain)
                {
                    newlySelectedBrain.ResetHUD(gameObject);
                    selectedBrain = newlySelectedBrain;
                }
                selectedBrain.UpdateHUD(gameObject);
            }
        }
    }
}

public interface IBrainViewable
{
    void ResetHUD(GameObject parent);
    bool UpdateHUD(GameObject parent);
}
