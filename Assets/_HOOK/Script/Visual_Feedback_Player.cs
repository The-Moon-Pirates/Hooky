using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual_Feedback_Player : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        this.transform.localScale =new Vector3(MapValue(GetComponent<Rigidbody2D>().velocity.y, -40f, 0f, 0.6f, 1f), transform.localScale.y, transform.localScale.z);  
    }

    private static float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
