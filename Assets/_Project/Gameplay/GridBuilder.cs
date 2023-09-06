using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    #region classes
    
    [System.Serializable]
    public struct GridSettings
    {
        public int width;
        public int height;
        public float cellsize;
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;
        private PlacedCharacter placedCharacter;

        public GridObject(Grid<GridObject> g, int x, int z)
        {
            this.grid = g;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedCharacter(PlacedCharacter t)
        {
            placedCharacter = t;
            grid.TriggerOnValueChanged(this.x, this.z);
        }

        public void ClearPlacedCharacter()
        {
            placedCharacter = null;
            grid.TriggerOnValueChanged(this.x, this.z);
        }

        public PlacedCharacter GetPlacedCharacter()
        {
            return placedCharacter;
        }

        public bool CanPlace()
        {
            return placedCharacter == null;
        }

        public override string ToString()
        {
            return placedCharacter.name;
        }
    }

    #endregion

    [SerializeField] private GridSettings gridSettings;
    [SerializeField] private CharacterData placedCharacter;
    [SerializeField] private LayerMask clickLayer;
    
    private Grid<GridObject> grid;
    private BaseController controller;

    public void SetPlacedCharacter(CharacterData placedCharacter)
    {
        this.placedCharacter = placedCharacter;
    }

    private void OnEnable()
    {
        GameController.OnNextRound += GameController_OnNextRound;
    }

    private void OnDisable()
    {
        GameController.OnNextRound -= GameController_OnNextRound;
    }

    private void GameController_OnNextRound(object sender, BaseController.TeamTag e)
    {
        Invoke(nameof(ClearGrid), 1.5f);
    }

    private void Awake()
    {
        grid = new Grid<GridObject>(gridSettings.width, gridSettings.height, gridSettings.cellsize,
            transform.position, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z), true);
    }

    private void Update()
    {
        PlaceCharacter();
        //DestroyCharacter();
    }

    public void SetController(BaseController controller)
    {
        this.controller = controller;
    }

    private void PlaceCharacter()
    {
        if (placedCharacter == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXZ(GetMousePosition(), out int x, out int z);

            GridObject gridObject = grid.GetValue(x, z);
            
            if (gridObject == null) return;


            if (!gridObject.CanPlace())
            {
                return;
            }

            var built = Instantiate(placedCharacter.placedCharacter, grid.GetWorldPosition(x, z), transform.rotation);
            gridObject.SetPlacedCharacter(built);
            controller.AddUnit(built);
            built.SetController(controller);
            Debug.Log(gridObject.ToString());
            controller.SpendMoney(placedCharacter.cost);
            placedCharacter = null;
        }
    }

    private void DestroyCharacter()
    {
        if(Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = grid.GetValue(GetMousePosition());
            PlacedCharacter placedCharacter = gridObject.GetPlacedCharacter();
            if (placedCharacter == null) return;

            placedCharacter.DestroySelf();
            gridObject.ClearPlacedCharacter();
        }
    }

    public void PlaceCharacterInRandomPosition(CharacterData data)
    {
        placedCharacter = data;
        int randomX = Random.Range(0, gridSettings.width);
        int randomZ = Random.Range(0, gridSettings.height - 1);
        GridObject gridObject = grid.GetValue(randomX, randomZ);
        
        while (!gridObject.CanPlace())
        {
            randomX = Random.Range(0, gridSettings.width);
            randomZ = Random.Range(0, gridSettings.height);
            gridObject = grid.GetValue(randomX, randomZ);
        }

        var built = Instantiate(placedCharacter.placedCharacter, grid.GetWorldPosition(randomX, randomZ), transform.rotation);
        gridObject.SetPlacedCharacter(built);
        controller.AddUnit(built);
        built.SetController(controller);
        built.gameObject.SetActive(false);
        placedCharacter = null;
    }

    private void ClearGrid()
    {
        for (int i = 0; i < gridSettings.height; i++)
        {
            for(int j = 0; j < gridSettings.width; j++)
            {
                GridObject gridObject = grid.GetValue(j, i);
                if (gridObject == null) continue;
                PlacedCharacter placedCharacter = gridObject.GetPlacedCharacter();
                if (placedCharacter == null) continue;

                placedCharacter.DestroySelf();
                gridObject.ClearPlacedCharacter();
            }
        }
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, clickLayer))
            return hit.point;

        return Vector3.zero;
    }
}


