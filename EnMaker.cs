using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnMaker : MonoBehaviour
{
    public GameObject myPrefab;
    //public GameObject rootObj;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject duplicate = Instantiate(rootObj);
        // Instantiate at position (0, 0, 0) and zero rotation.
        //Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        for (int i = 0; i <4; i++)
        {
            Instantiate(myPrefab, new Vector3(i * 8.0F, 3.45f, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
