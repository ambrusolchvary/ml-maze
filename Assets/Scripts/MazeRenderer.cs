using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeRenderer : MonoBehaviour {

    [SerializeField]
    [Range(1, 50)]
    public int width = 10;

    [SerializeField]
    [Range(1, 50)]
    public int height = 10;

    [SerializeField]
    private float size = 1f; // a cella merete

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private WallState[,] maze = null;

    public WallState[,] GetMaze() {
        return maze;
    }

    // Start is called before the first frame update
    void Start() {
        maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }


    private void Draw( WallState[,] maze ) {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);

        for(int i = 0;i < width;++i) {
            for(int j = 0;j < height;++j) {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if(cell.HasFlag(WallState.UP)) {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if(cell.HasFlag(WallState.LEFT)) {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                /* Ha minden cellanak kirajzoljuk csak a bal es a felso oldalon levo falat, akkor a negyszog alaku teruletunkben
                 minden cellanak ki lesz rajzolva mindegyik fala, kizarolag az a legalso sorban es a legjobboldali oszlopban levo cellaknak
                fognak hianyozni az also avagy a jobboldali fala, emiatt az also es jobb oldali falakat csak akkor rajzoltatjuk ki,
                hogyha az adott cella a jobboldali oszlopban vagy a legalso sorban szerepel a labirintusunkban.*/

                if(i == width - 1) // i = width-1 := utolso oszlopot reprezentalja
                {
                    if(cell.HasFlag(WallState.RIGHT)) {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if(j == 0) // j = 0 := elso sort reprezentalja
                {
                    if(cell.HasFlag(WallState.DOWN)) {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }

        }

    }
    public void Reset() {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
        maze = null;
        maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    // Update is called once per frame
    void Update() {

    }

    /*
    public void OnDestroy() {
        if(gameObject != null) {
            Destroy(gameObject);
        }
    }
    */
}
