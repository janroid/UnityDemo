using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject black;
    public int width = 10;
    public int height = 4;

    // Start is called before the first frame update
    void Start()
    {
        for (int y=0; y<height; ++y)
       {
           for (int x=4; x<width; ++x)
           {
               Instantiate(black, new Vector3(x,y,-9), Quaternion.identity);
           }
       }       
    }

}
