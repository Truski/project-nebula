using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(FallTimer());
        }
    }

    private IEnumerator FallTimer()
    {
        GetComponent<EdgeCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        GetComponent<EdgeCollider2D>().enabled = true;
    }
}
