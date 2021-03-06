﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TowerPlacement : MonoBehaviour
    {
        public Camera playerCam;
        public int rotationSpeed = 100;
        public List<GameObject> towers;
        public List<GameObject> _placedTowers;

        private bool lmbPressed = false;
        private bool buttonRotateLeft = false, buttonRotateRight = false;
        private RaycastHit hit;
        private NavMeshHit hitNav;
        private Vector3 fwd;
        private GameObject towerPreview;
        private GameObject selectedTower;
        private int slotNumber = -1;
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private Phase currentPhase;
        private bool phaseSwitch = true, buildingPhaseActive = true;
        private bool _firstPlacement = true;
        public GameObject tutorialUI;

        private Player _player;

        // Raycast on layer 8
        private int layerMask = 1 << 8;
        // Range of ray
        public float maxDistance = 100;


        // Use this for initialization
        void Start()
        {
            foreach (GameObject tower in towers)
            {
                if (tower.GetComponent<Tower>() == null)
                {
                    tower.AddComponent<Tower>();
                }
                if (tower.GetComponent<NavMeshObstacle>() == null)
                {
                    tower.AddComponent<NavMeshObstacle>();
                }
                tower.GetComponent<NavMeshObstacle>().carving = true;
            }

            _player = FindObjectOfType<Player>();
        }

        void SelectTowerToPlace(int towerNumber)
        {
            if (towers.Count > towerNumber)
            {
                selectedTower = towers[towerNumber];
                if (towerPreview != null)
                {
                    previousPosition = towerPreview.transform.position;
                    previousRotation = towerPreview.transform.rotation;
                    Destroy(towerPreview);
                }
                towerPreview = Instantiate(selectedTower, previousPosition, previousRotation);
                towerPreview.GetComponent<Tower>().SetPreviewMode(true);
                towerPreview.SetActive(false);
            }
            else
            {
                selectedTower = null;
                Destroy(towerPreview);
            }
        }

        void Update()
        {
            currentPhase = gameObject.GetComponentInChildren<Game>()._currentPhase;
            if (currentPhase == Phase.Building)
            {
                buildingPhaseActive = true;
                phaseSwitch = true;
            }
            else
            {
                buildingPhaseActive = false;
            }

            if (phaseSwitch)
            {
                lmbPressed = Input.GetMouseButtonDown(0);
                buttonRotateLeft = Input.GetAxis("Rotate Tower") > 0;
                buttonRotateRight = Input.GetAxis("Rotate Tower") < 0;

                if (Input.GetButtonDown("Tower Slot 1"))
                {
                    slotNumber = 0;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 2"))
                {
                    slotNumber = 1;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 3"))
                {
                    slotNumber = 2;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 4"))
                {
                    slotNumber = 3;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 5"))
                {
                    slotNumber = 4;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 6"))
                {
                    slotNumber = 5;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 7"))
                {
                    slotNumber = 6;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 8"))
                {
                    slotNumber = 7;
                    _player.IsInDefaultMode = false;
                }
                else if (Input.GetButtonDown("Tower Slot 9"))
                {
                    slotNumber = 8;
                    _player.IsInDefaultMode = false;

                }
                else if (Input.GetButtonDown("defaultMode"))
                {
                    slotNumber = 999;
                    _player.IsInDefaultMode = true;
                }

                if (!buildingPhaseActive)
                {
                    slotNumber = 999;
                    phaseSwitch = false;
                }

                if (slotNumber != -1)
                {
                    SelectTowerToPlace(slotNumber);
                    slotNumber = -1;
                }

                if (selectedTower != null)
                {
                    fwd = playerCam.transform.TransformDirection(Vector3.forward);

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
                            towerPreview.GetComponent<Tower>().SetPlaceable(1);
                            if (TowerBuildingAllowed())
                            {
                                towerPreview.GetComponent<Tower>().SetPlaceable(0);
                            }
                            else
                            {
                                towerPreview.GetComponent<Tower>().SetPlaceable(2);
                            }

                            if (lmbPressed && TowerBuildingAllowed())
                            {
                                GameObject towerInstance = Instantiate(selectedTower, hit.point, towerPreview.transform.rotation);
                                BuildTower(towerInstance);
                                towerInstance.GetComponentInChildren<Animator>().SetTrigger("Build");
                                lmbPressed = false;
                            }
                        }
                        else
                        {
                            towerPreview.GetComponent<Tower>().SetPlaceable(1);
                        }
                    }
                    else
                    {
                        towerPreview.SetActive(false);
                    }
                }
            }
        }

        private bool TowerBuildingAllowed()
        {
            return FindObjectOfType<TowerBuilding>().TowerBuildingAllowed(selectedTower);
        }

        private void BuildTower(GameObject towerInstance)
        {
            if (_firstPlacement == true)
            {
                GameObject[] tutorialTowers = GameObject.FindGameObjectsWithTag("TutorialTower");
                tutorialUI.GetComponent<Tutorial>().FadeOut();
                tutorialUI.GetComponentInChildren<Text>().text = "Puh, das sollte erstmal helfen. Drücke G, um in die nächste Phase zu kommen.";
                tutorialUI.GetComponent<Tutorial>().FadeIn();
                FindObjectOfType<Game>()._readyButtonEnabled = true;
                foreach (GameObject tutorialTower in tutorialTowers)
                {
                    Destroy(tutorialTower);
                }
                _firstPlacement = false;
            }

            FindObjectOfType<TowerBuilding>().BuildTower(towerInstance);
            _placedTowers.Add(towerInstance);
        }
    }
}
