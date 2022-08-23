using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace simple_math_3_game
{
    public class Manager : MonoBehaviour
    {
        readonly int[] minValue = new int[] { 3, 4, 5};
        public static int diffucultID;

        const float shiftDelay = 0.03f;
        private static Manager instance;

        public static Manager Instance
        {
            get
            {
                if(!instance)
                {
                    instance = FindObjectOfType<Manager>();
                }

                return instance;
            }
        }

        Cell[,] cells;

        public static int minSequence;

        [HideInInspector]
        public bool isShifting;

        [Space(10)]
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] ContentSizeFitter contentSizeFitter;

        [Space(10)]
        [SerializeField] GameObject cellPrefab;
        [SerializeField] Transform cellParent;

        [Space(10)]
        [SerializeField] Vector2Int size;

        [Space(10)]
        [SerializeField] Sprite[] icons;
        [SerializeField] Scores scores;

        public void Start_Game()
        {
            Clear_Board();

            scores.ResetScores();
            Create_Board();
        }

        public void SetDifficult(int i)
        {
            diffucultID = i;
            minSequence = minValue[diffucultID];
        }

        void Create_Board()
        {
            gridLayoutGroup.enabled = true;
            contentSizeFitter.enabled = true;
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = size.x;

            cells = new Cell[size.x, size.y];

            Sprite previousBelow = null;
            Sprite[] previousLeft = new Sprite[size.y];

            for (int x = 0; x < size.x; x++)
            {
                for(int y = 0; y < size.y; y++)
                {
                    Sprite randomSprite = GetRandomIcon(previousLeft[y], previousBelow);
                    previousLeft[y] = previousBelow = randomSprite;

                    Cell _cell = Instantiate(cellPrefab, cellParent).GetComponent<Cell>();
                    _cell.name = string.Format("[{0}:{1}]", x, y);
                    _cell.SetMainIcon(randomSprite);
                    cells[x, y] = _cell;
                }
            }

            Canvas.ForceUpdateCanvases();
            gridLayoutGroup.enabled = false;
            contentSizeFitter.enabled = false;
        }

        void Clear_Board()
        {
            foreach(Transform t in cellParent)
            {
                Destroy(t.gameObject);
            }

            cellParent.DetachChildren();
        }

        Sprite GetRandomIcon(Sprite previousLeft, Sprite previousBelow)
        {
            List<Sprite> possibleCharacters = new List<Sprite>();

            possibleCharacters.AddRange(icons);
            possibleCharacters.Remove(previousLeft);
            possibleCharacters.Remove(previousBelow);

            return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
        }

        Sprite GetNewSprite(int x, int y)
        {
            List<Sprite> possibleCharacters = new List<Sprite>();

            possibleCharacters.AddRange(icons);

            if (x > 0)
            {
                possibleCharacters.Remove(cells[x - 1, y].MySprite);
            }
            if (x < size.x - 1)
            {
                possibleCharacters.Remove(cells[x + 1, y].MySprite);
            }
            if (y > 0)
            {
                possibleCharacters.Remove(cells[x, y - 1].MySprite);
            }

            return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
        }

        public void FindNullTiles()
        {
            StopCoroutine(nameof(FindNullTiles_process));
            StartCoroutine(nameof(FindNullTiles_process));
        }

        IEnumerator ShiftTilesDown(int x, int yStart)
        {
            isShifting = true;
            List<Cell> renders = new List<Cell>();
            int nullCount = 0;

            for (int y = yStart; y < size.y; y++)
            {
                Cell _cell = cells[x, y];

                if (_cell.MySprite == null)
                {
                    nullCount++;
                }

                renders.Add(_cell);
            }

            for (int i = 0; i < renders.Count; i++)
            {
                scores.Score += 1;
                yield return new WaitForSeconds(shiftDelay);

                for (int k = 0; k < renders.Count; k++)
                {
                    renders[k].SetMainIcon(GetNewSprite(x, size.y - 1));
                }
            }

            isShifting = false;
        }

        IEnumerator FindNullTiles_process()
        {
            for(int x = 0; x < size.x; x++)
            {
                for(int y = 0; y < size.y; y++)
                {
                    if(cells[x,y].MySprite == null)
                    {
                        yield return StartCoroutine(ShiftTilesDown(x, y));
                        break;
                    }
                }
            }

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    cells[x, y].ClearAllMatches();
                }
            }
        }

        [System.Serializable]
        public class Scores
        {
            int score;

            public int Score
            {
                get
                {
                    return score;
                }

                set
                {
                    score = value;
                    scoresText.text = score.ToString();
                }
            }

            public Text scoresText;

            public void ResetScores()
            {
                Score = 0;
            }
        }
    }
}
