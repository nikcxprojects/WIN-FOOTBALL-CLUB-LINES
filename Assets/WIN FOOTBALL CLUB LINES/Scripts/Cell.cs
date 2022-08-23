using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace simple_math_3_game
{
    public class Cell : MonoBehaviour
    {
        bool matchFound = false;

        private static Cell previousSelected = null;

        private static Vector2[] HorizontalAxis
        {
            get
            {
                return new Vector2[] { Vector2.left, Vector2.right };
            }
        }

        private static Vector2[] VerticalAxis
        {
            get
            {
                return new Vector2[] { Vector2.up, Vector2.down };
            }
        }

        private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        Cell MyCellComponent
        {
            get
            {
                return GetComponent<Cell>();
            }
        }

        public Sprite MySprite
        {
            get
            {
                return main.sprite;
            }
        }

        bool isSelected;

        public Image main;

        public GameObject outline;

        private void OnMouseDown()
        {
            if(main.sprite == null || Manager.Instance.isShifting)
            {
                return;
            }

            if(isSelected)
            {
                Deselect();
            }
            else if(previousSelected == null)
            {
                Select();
            }
            else
            {
                if(GetAllAdjacentTiles().Contains(previousSelected.gameObject))
                {
                    Swap_Sprite(previousSelected);

                    previousSelected.ClearAllMatches();

                    previousSelected.Deselect();

                    ClearAllMatches();
                }
                else
                {
                    previousSelected.Deselect();

                    Select();
                }
            }
        }

        GameObject GetAdjacent(Vector2 castDir)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, castDir);

            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.gameObject != gameObject)
                {
                    return hits[i].collider.gameObject;
                }
            }

            return null;
        }

        List<GameObject> GetAllAdjacentTiles()
        {
            List<GameObject> adjacentTiles = new List<GameObject>();

            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
            }

            return adjacentTiles;
        }

        List<Cell> FindMatch(Vector2 castDir)
        {
            Cell prevCell = MyCellComponent;

            List<Cell> matchingTiles = new List<Cell>();

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, castDir);

            for(int i = 0; i < hits.Length; i++)
            {
                Cell cell = hits[i].collider.GetComponent<Cell>();

                if(cell.MySprite == prevCell.MySprite)
                {
                    if (cell != MyCellComponent)
                    {
                        matchingTiles.Add(cell);

                        prevCell = cell;
                    }
                }
                else
                {
                    break;
                }
            }

            return matchingTiles;
        }

        void ClearMatch(Vector2[] paths)
        {
            List<Cell> matchingTiles = new List<Cell>() { MyCellComponent };

            for (int i = 0; i < paths.Length; i++) 
            { 
                matchingTiles.AddRange(FindMatch(paths[i])); 
            }

            if (matchingTiles.Count >= Manager.minSequence)
            {
                for (int i = 0; i < matchingTiles.Count; i++)
                {
                    matchingTiles[i].SetMainIcon(null);
                }

                matchFound = true;
            }
        }

        void Select()
        {
            isSelected = true;

            outline.SetActive(true);

            previousSelected = MyCellComponent;
        }

        void Deselect()
        {
            isSelected = false;
            outline.SetActive(false);
            previousSelected = null;
        }

        public void ClearAllMatches()
        {
            if(MySprite == null)
            {
                return;
            }

            ClearMatch(HorizontalAxis);
            ClearMatch(VerticalAxis);

            if(matchFound)
            {
                matchFound = false;

                Manager.Instance.FindNullTiles();
            }
        }

        public void Swap_Sprite(Cell cell)
        {
            if(MySprite == cell.MySprite)
            {
                return;
            }

            Sprite tempSprite = cell.MySprite;

            cell.SetMainIcon(MySprite);

            SetMainIcon(tempSprite);
        }

        public void SetMainIcon(Sprite sprite)
        {
            main.sprite = sprite;
            gameObject.SetActive(sprite != null);
        }
    }
}
