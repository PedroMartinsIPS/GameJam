using UnityEngine;

public class ButtonFloat : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition =
            startPos +
            new Vector3(
                0,
                Mathf.Sin(Time.time * 2f) * 3f,
                0
            );
    }
}