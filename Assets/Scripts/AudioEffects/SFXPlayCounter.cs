using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayCounter : MonoBehaviour
{
    public float counter;

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
    }
}
