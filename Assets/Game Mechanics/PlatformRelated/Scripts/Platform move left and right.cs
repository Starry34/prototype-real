using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformmoveleftandright : MonoBehaviour
{
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float delay = 10.0f;

    [SerializeField] GameObject platform;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        platform.transform.position = pointA.transform.position;
        targetPosition = pointB.transform.position;
        StartCoroutine(MovePlatform());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPosition - platform.transform.position).sqrMagnitude > 0.01f)
            {
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            targetPosition = targetPosition == pointA.transform.position ? pointB.transform.position : pointA.transform.position;

            yield return new WaitForSeconds(delay);
        }
    }
}
