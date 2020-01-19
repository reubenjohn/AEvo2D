using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersHUD : MonoBehaviour
{
    [SerializeField]
    public int LayerCount
    {
        get => transform.childCount;

        set
        {
            if (transform.childCount != value)
            {
                transform.DestroyAndDetachChildren();

                Instantiate(layer, transform).name = "Layer 0";
                for (int i = 1; i < value; i++)
                {
                    GameObject layerInstance = Instantiate(layer, transform);
                    layerInstance.name = "Layer " + i;
                    layerInstance.GetComponent<Tensor1DHUD>().inputTensor1DHUDTransform = transform.GetChild(i - 1).GetComponent<RectTransform>();
                }
            }
        }
    }
    public GameObject layer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
