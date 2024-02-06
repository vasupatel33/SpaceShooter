using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrolling : MonoBehaviour
{
    float speed = 0.03f; // Adjust the speed here

    private Material mat;
    private float accumulatedTime = 0f;
    private Vector2 lastOffset = Vector2.zero; // Track the last applied offset

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            mat = renderer.material;
            StartCoroutine(UpdateTextureOffset());
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }
    private IEnumerator UpdateTextureOffset()
    {
        while (true)
        {
            //float deltaTime = Time.deltaTime;
            accumulatedTime += Time.deltaTime * speed;

            Vector2 newOffset = new Vector2(0, accumulatedTime);
            if (newOffset != lastOffset) // Only update if the offset has changed
            {
                mat.SetTextureOffset("_MainTex", newOffset);
                mat.DOOffset(newOffset, 0.3f);
                lastOffset = newOffset;
            }

            // Wait for the next frame
            yield return null;
        }
    }
}
