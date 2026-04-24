using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsoFader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenFader.Instance.StartFadeToOpaque();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
