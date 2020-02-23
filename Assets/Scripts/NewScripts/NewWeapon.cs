using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeapon : MonoBehaviour
{
    public List<GameObject> spawnPointList = new List<GameObject>();
    public GameObject bullet;
    public Camera myCamera;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            Debug.Log("Pressed primary button.");
        }
    }

    public void Fire()
    {
        SetBulletsToSpawnPoints();
    }
    public void SetBulletsToSpawnPoints()
    {
        foreach(GameObject spawnPoint in spawnPointList)
        {
            GameObject newBullet = GameObject.Instantiate(bullet);
            newBullet.transform.position = spawnPoint.transform.position;
            Debug.Log(newBullet.transform.localScale);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            ray.origin = spawnPoint.transform.position;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                Debug.Log(hit.point);
                newBullet.GetComponent<NewBullet>().SetDistinationVector(hit.point);
            }
                //newBullet.GetComponent<NewBullet>().SetDistinationVector(laserEnd);
            }
    }
}
