using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignPieceToSquare(Transform newTransform)
    {
        transform.position = new Vector3(newTransform.position.x, newTransform.position.y, newTransform.position.z);
    }
}
