using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronHUD : MonoBehaviour
{
    public RectTransform inputLayerTransform;
    public RectTransform connectionsTransform;
    public GameObject connection;
    public Color fromColor = Color.blue, toColor = Color.red;

    public float activation = 0.5f;
    public string label = "-";
    public float[] weights;

    private Image neuronImage;
    private Text activationText;
    private Text labelText;

    // Start is called before the first frame update
    void Start()
    {
        neuronImage = GetComponent<Image>();
        activationText = transform.Find("Activation").GetComponent<Text>();
        labelText = transform.Find("Label").GetComponent<Text>();
        ResetWhenReady();
    }

    bool IsParentLayoutGroupReady()
    {
        return inputLayerTransform != null && inputLayerTransform.position.x != connectionsTransform.position.x;
    }

    public void ResetWhenReady()
    {
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        while (!IsParentLayoutGroupReady())
            yield return null;
        yield return null;

        connectionsTransform.DestroyAndDetachChildren();

        Vector3 rectPosition = connectionsTransform.position;
        foreach (Transform inputTransform in inputLayerTransform)
        {
            Vector2 displacement = rectPosition - (inputTransform.position + Vector3.right * 20f);
            float degrees = Mathf.Rad2Deg * Mathf.Atan(displacement.y / displacement.x);

            RectTransform line = Instantiate(connection, connectionsTransform).GetComponent<RectTransform>();
            line.sizeDelta = new Vector2(displacement.magnitude, line.rect.height);
            line.rotation = Quaternion.Euler(0, 0, degrees);
        }
    }

    // Update is called once per frame
    void Update()
    {
        neuronImage.color = Color.Lerp(fromColor, toColor, activation.SignedToUnsignUnitFraction());
        activationText.text = activation.ToString("0.00");
        labelText.text = label;
        if (weights.Length > 0)
        {
            Image[] lines = connectionsTransform.GetComponentsInChildren<Image>();
            for (int i = 0; i < lines.Length; i++)
            {
                Color color = Color.Lerp(fromColor, toColor, inputLayerTransform.GetChild(i).GetComponent<NeuronHUD>().activation.SignedToUnsignUnitFraction());
                color.a = weights[i];
                lines[i].color = color;
            }
        }
    }
}
