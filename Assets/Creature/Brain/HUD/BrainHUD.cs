using UnityEditor;
using UnityEngine;

public class BrainHUD : MonoBehaviour
{
    private IBrainViewable selectedBrain;

    public void Update()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.TryGetComponent<IBrainViewable>(out IBrainViewable newSelection))
            {
                if (newSelection != selectedBrain)
                {
                    newSelection.ResetHUD(gameObject);
                    selectedBrain = newSelection;
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
