using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    Grid grid;
    GameManager gameManager;

    [SerializeField] private Material highLightMaterial;

    [SerializeField] private Transform hightlight;
    [SerializeField] private HashSet<Transform> selection = new HashSet<Transform>();

    private Material hightlightMaterial;
    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject thing = null;
        
        if(grid.GridCellsRangeCheck(worldPosition))
        thing = grid.GetCellByPos(worldPosition).GetOccupiyingObject();
        
        Renderer _selectionRenderer = null;

        if (thing != null)
        {
            _selectionRenderer = thing.GetComponent<Renderer>();
            if (_selectionRenderer != null)
            {
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
                Debug.Log(hightlightMaterial);

                hightlight.GetComponent<Renderer>().material = hightlightMaterial;
                hightlight = null;
                hightlightMaterial = null;
            }
        }

        if(hightlight && Input.GetMouseButtonDown(0))
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
        }
    }

}
