using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerPlacementFree : MonoBehaviour
{
    public Camera playerCam;
    public int rotationSpeed = 100;
    public List<GameObject> towers;

    private bool lmbPressed = false;
    private bool buttonRotateLeft = false, buttonRotateRight = false;
    private List<Bounds> towerBounds = new List<Bounds>();
    private bool towersIntersect = false;
    private RaycastHit hit;
    private NavMeshHit hitNav;
    private Vector3 fwd;
    private GameObject towerPreview;
    private GameObject selectedTower;

    // Raycast on layer 8
    private int layerMask = 1 << 8;
    private float maxDistance = 100;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject tower in towers)
        {
            if (tower.GetComponent<Tower>() == null)
            {
                tower.AddComponent<Tower>();
            }
        }
        selectedTower = towers[0];
        towerPreview = Instantiate(selectedTower);
        towerPreview.GetComponent<Tower>().SetPreviewMode(true);
    }

    // Update is called once per frame
    void Update()
    {
        lmbPressed = Input.GetMouseButtonDown(0);
        //buttonRotateLeft = Input.GetAxis("Rotate Tower") > 0;
        //buttonRotateRight = Input.GetAxis("Rotate Tower") < 0;
    }

    void FixedUpdate()
    {
        fwd = playerCam.transform.TransformDirection(Vector3.forward);

        towersIntersect = false;

        foreach (var towerBound in towerBounds)
        {
            if (towerBound.Intersects(towerPreview.GetComponent<Collider>().bounds))
            {
                towersIntersect = true;
            }
        }

        if (Physics.Raycast(playerCam.transform.position, fwd, out hit, maxDistance, layerMask))
        {
            towerPreview.SetActive(true);
            towerPreview.transform.position = hit.point;

            if (buttonRotateLeft)
            {
                towerPreview.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            }
            else if (buttonRotateRight)
            {
                towerPreview.transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
            }

            if (NavMesh.SamplePosition(hit.point, out hitNav, 0.2f, NavMesh.AllAreas))
            {
                towerPreview.GetComponent<Tower>().SetPlaceable(true);
                if (towersIntersect)
                {
                    towerPreview.GetComponent<Tower>().SetPlaceable(false);
                }

                if (lmbPressed && !towersIntersect && BuildTower())
                {
                    GameObject towerInstance = Instantiate(selectedTower, hit.point, towerPreview.transform.rotation);
                    towerBounds.Add(towerInstance.GetComponent<Collider>().bounds);
                    lmbPressed = false;
                }
            }
            else
            {
                towerPreview.GetComponent<Tower>().SetPlaceable(false);
            }
        }
        else
        {
            towerPreview.SetActive(false);
        }
    }

    private bool BuildTower()
    {
        return FindObjectOfType<TowerBuilding>().BuildTower(selectedTower);
    }
}
