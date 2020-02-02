using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown (0))
        {
            ScreenMouseRay();
        }
    } 
public void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
 
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);
 
        Collider2D[] col = Physics2D.OverlapPointAll(v);
 
        if(col.Length > 0){
            foreach(Collider2D c in col)
            {
                Debug.Log("Collided with: " + c.GetComponent<Collider2D>().gameObject.name);
                
            }
        }
    }
}
