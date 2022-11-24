using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float distance = 10.0f;
    public bool followTarget = true;

    // Update is called once per frame
    void Update()
    {
        if (followTarget) {
            this.transform.position = target.transform.position + Vector3.Cross(target.transform.up, target.transform.forward) * distance;
            this.transform.LookAt(target.transform);
        }
    }

    public void moveToLookAt(GameObject tgt)
    {
        followTarget = false;
        StartCoroutine(smoothMoveTo(tgt, 3.0f));
    }

    IEnumerator smoothMoveTo(GameObject tgt, float moveSpeed)
    {
        if (tgt == null)
            yield return null;

        target = tgt;

        Vector3 startingPos = this.transform.position;
        Vector3 direction = Vector3.Cross(-tgt.transform.up, tgt.transform.forward);

        Quaternion startRotation = tgt.transform.rotation;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

        float elapsedTime = 0;

        while (elapsedTime < moveSpeed)
        {
            this.transform.rotation = Quaternion.LerpUnclamped(startRotation, toRotation, (elapsedTime / moveSpeed));
            this.transform.position = Vector3.LerpUnclamped(startingPos, tgt.transform.position + Vector3.Cross(tgt.transform.up, tgt.transform.forward) * distance, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        followTarget = true;
    }

}
