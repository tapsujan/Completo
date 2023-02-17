using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    Transform visual;
    void Start()
    {
        visual = transform.GetChild(0).transform;
        StartCoroutine(PickupFloatingAnimation());
    }
    public void StartPickupFloatingAnimationCoroutine()
    {
        StartCoroutine(PickupFloatingAnimation());
    }
    private IEnumerator PickupFloatingAnimation()
    {
        while (true)
        {
            visual.localPosition = new Vector2(0, 0.25f * Mathf.Sin(Time.time));
            yield return null;
        }
    }
}
