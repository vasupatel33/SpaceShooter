using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{               
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 startPos, endPos;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0;
            Vector3 newPosition = Vector3.Lerp(this.transform.position, currentPos, 1f);
            newPosition.x = Mathf.Clamp(newPosition.x, -2f, 2f);

            this.transform.DOMove(newPosition,0.5f);
        }
    }
}
