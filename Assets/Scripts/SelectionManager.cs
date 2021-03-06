﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    Grid grid;
    GameManager gameManager;

    [SerializeField] private Material highLightMaterial = null;

    [SerializeField] private Transform hightlight;
    [SerializeField] private HashSet<Transform> selection = new HashSet<Transform>();
    [SerializeField] private Transform currentSelection;

    private Material hightlightMaterial;

    [SerializeField] private GameObject pointer;


    [SerializeField] private bool canSelect = true;
    public bool CanSelect { get { return canSelect; } set { canSelect = value; } }

    [SerializeField] private bool deselectAfterFullMove = true;
    public bool DeselectAfterFullMove { get { return deselectAfterFullMove; } set { deselectAfterFullMove = value; } }


    public Transform GetCurSelection()
    {
        return currentSelection;
    }

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {

        if (pointer != null)
        {
            pointer = Instantiate(pointer);
            pointer.SetActive(false);
        }

    }

    public bool IsHighlighting()
    {
        if (hightlight != null) return true; else return false;
    }

    public void ChangeCurSelection(Transform selection)
    {
        currentSelection = selection;
    }

    public void RemoveSelection(Transform _selection)
    {
        if (currentSelection == _selection) currentSelection = null;
        selection.Remove(_selection);
    }

    void Update()
    {

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject thing = null;

        if(grid.GridCellsRangeCheck(worldPosition))
        thing = grid.GetCellByPos(worldPosition).GetOccupiyingObject();

        bool selectable = false;
        if (thing != null) { selectable = thing.GetComponent<DynamicObject>().selectable; }


        Renderer _selectionRenderer = null;

        //HighLighting
        {
            if (thing != null && selectable)
            {
                _selectionRenderer = thing.GetComponent<Renderer>();
                if (_selectionRenderer != null)
                {
                    grid.ClearPath();
                    if (hightlight != null && thing.transform != hightlight)
                    {
                        hightlight.GetComponent<Renderer>().material = hightlightMaterial;
                        hightlight = null;
                        hightlightMaterial = null;
                    }

                    hightlight = thing.transform;

                    if (hightlightMaterial == null)
                    { hightlightMaterial = _selectionRenderer.material; }

                    _selectionRenderer.material = highLightMaterial;
                }
            }
            else
            {
                if (hightlight != null)
                {
                    //Debug.Log(hightlightMaterial);

                    hightlight.GetComponent<Renderer>().material = hightlightMaterial;
                    hightlight = null;
                    hightlightMaterial = null;
                }
            }
        }

        //Show pointer
        if (pointer != null)
        {
            if (hightlight)
            {
                pointer.SetActive(true);
                pointer.transform.position = new Vector3(thing.transform.position.x, thing.transform.position.y + 1.0f, 0f);
            }
            else
            {
                pointer.SetActive(false);
            }
        }


        //Selection
        if (hightlight && Input.GetMouseButtonDown(0) && canSelect && gameManager.Targeting == false)
        {
            if(selection.Contains(hightlight))
            {
                selection.Remove(hightlight);
                Debug.Log("Removed || Selection number: " + selection.Count);
            }
            else
            {
                selection.Add(hightlight);
                Debug.Log("Added || Selection number: " + selection.Count);
            }

            if (currentSelection == hightlight) currentSelection = null;
            else currentSelection = hightlight;

            grid.ClearPath();
        }
    }

}
