using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrolling : MonoBehaviour
{

    [Range(-1, 1)]
    public float speed = 0.5f;
    float offset;
    Material mat;
    

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            mat = renderer.material;
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
        InvokeRepeating("BGMoving",0.2f,0.2f);
    }
    public void BGMoving()
    {
        offset = Time.deltaTime * speed;
        Debug.Log("offset = " + offset);
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
        mat.DOOffset(new Vector2(0, offset), 0.3f);
    }
    private void Update()
    {
        Debug.Log("Time = "+Time.deltaTime);
        
    }
}
