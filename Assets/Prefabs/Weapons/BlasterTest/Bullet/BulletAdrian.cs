using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAdrian : MonoBehaviour
{
    public float speed;

    public GameObject muzzleFlash;

    public GameObject hitBulletPrefab;

    [System.NonSerialized]
    
    private Vector3 velocity;

    private void Start()
    {

        if (muzzleFlash != null)
        {
            var muzzleFlashEffect = Instantiate(muzzleFlash, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    public void Init(Vector3 orgin, Vector3 target)
    {
        Vector3 dir = (target - orgin).normalized;
        velocity = dir * speed;

    }
        
    private void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        transform.position += velocity * Time.deltaTime;
        velocity += Physics.gravity * Time.deltaTime;

        // raycast from oldpos to transform.position & check for any objects hit.
    }

    
    

}
