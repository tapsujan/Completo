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
                                                  //amp  *       Sin(     time * freq)
            visual.localPosition = new Vector2(0, 0.125f * Mathf.Sin(Time.time *  3f));
            yield return null;
        }
    }
}
