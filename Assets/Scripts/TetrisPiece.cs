using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private Material originalMat;
    public Material changedMat;
    private Renderer r;
    public bool ChangeColor;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        originalMat = r.material;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMyColor();
    }

    public void ChangeMyColor()
    {
        if (ChangeColor)
        {
            r.material = changedMat;
            ChangeColor = false;
        }
        else r.material = originalMat;

    }

    public void ChangeMyLayerMask(string lm)
    {
        gameObject.layer = LayerMask.NameToLayer(lm);
    }

}
