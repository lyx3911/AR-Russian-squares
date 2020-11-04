using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{   
    static int gridWidth = 10;
    static int gridHeight = 20;

    public int score = 0; //游戏分数
    public bool isGameOver = false; //是否GameOver

    //记录每个方块是否被占
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino();
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(score);
    }

    //判断是否超出格子，gameover， 没有超出返回false， 超出返回true
    public bool CheckIsAboveGrid(Tetromino tetromino){
        foreach(Transform mino in tetromino.transform){
            Vector3 pos = Round(mino.position);
            if(pos.y>gridHeight) return true;
        }
        return false;
    }

    //判断某行是否是满的
    public bool IsFullRowAt(int y){
        for(int x=0; x<gridWidth; x++){
            if(grid[x,y]==null) return false;
        }
        return true;;
    }
    //删除某行的元素，y从0开始计算
    public void DeleteMinoAtRow(int y){
        for(int x=0; x<gridWidth; x++){
            Destroy(grid[x,y].gameObject);
            grid[x,y] = null;
        }
        Crystal(0);
        Crystal(2);
        Crystal(1);
        Crystal(3);

        FindObjectOfType<Particle>().fireworks();

		SimpleSampleCharacterControl[] peoples = FindObjectsOfType(typeof(SimpleSampleCharacterControl)) as SimpleSampleCharacterControl[];
		foreach (SimpleSampleCharacterControl people in peoples) {
			people.m_jumpInput = true;
		}

    }

    public void Crystal(int y){
        string crystal_color = "Prefabs/crystal_yellow";
        Vector3 crystal_position = new Vector3(-10,0,0);
        switch (y)
        {
            case 1:
                crystal_color = "Prefabs/crystal_blue";
                crystal_position = new Vector3(-12,0,-2);
                break;
            case 2:
                crystal_color = "Prefabs/crystal_yellow";
                crystal_position = new Vector3(15,0,0);
                break;
            case 3:
                crystal_color = "Prefabs/crystal_blue";
                crystal_position = new Vector3(17,0,-2);
                break;
        
            default:
                break;
        }
        GameObject newcrystal = (GameObject)Instantiate(Resources.Load(crystal_color, typeof(GameObject)), crystal_position, Quaternion.identity);
    }
    
    //将第y行往下移动,y从0开始算
    public void MoveRowDown(int y){
        if(y<=0) return;
        for(int x=0; x<gridWidth; x++){
            if(grid[x,y]!=null){
                grid[x, y-1] = grid[x,y];
                grid[x,y] = null;
                grid[x,y-1].position += new Vector3(0,-1,0); //更新方块位置
            }
        }
    }

    //将y行以上的所有行往下移动一格,包括y
    public void MoveAllRowsDown(int y){
        for(int i=y; i<gridHeight ;i++)
            MoveRowDown(i);
    }

    public void CheckandDeleteRow(){
        for(int y=0; y<gridHeight; y++){
            if(IsFullRowAt(y)){
                DeleteMinoAtRow(y);
                MoveAllRowsDown(y+1);
                --y;
                score += 10; //加分
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino){
        for(int y=0; y<gridHeight; y++){
            for(int x=0; x<gridWidth; x++){
                if(grid[x,y]!=null){
                    //如果图形本来就在这个位置,要进行更新,删除原来的位置，填入新的位置
                    if(grid[x,y].parent==tetromino.transform) grid[x,y]=null; 
                }
            }
        }
        //添加新的位置
        foreach (Transform mino in tetromino.transform){
            Vector3 pos = Round(mino.position);
            if(pos.y<=gridHeight){
                grid[(int)pos.x+4,(int)pos.y-1] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos){
        if(pos.y>gridHeight) return null;
        else return grid[(int)pos.x+4, (int)pos.y-1];
    }

    public void SpawnNextTetromino(){
        //添加的方块是ImageTarget的组件
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector3(0,20,0), Quaternion.identity);
        GameObject ImgTar = GameObject.Find("MyImageTarget");
        nextTetromino.transform.parent = ImgTar.transform;
    }

    public bool CheckIsInsideGrid(Vector3 pos){
        return ((int)pos.x >-gridWidth/2 && (int)pos.x<gridWidth/2+1 && (int)pos.y>0);
    }
    public Vector2 Round(Vector3 pos){
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }

    string GetRandomTetromino() {
        int randomTetromino = Random.Range(1,8);

        string randomTetrominoName = "Prefabs/Tetromino_T";

        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "Prefabs/Tetromino_Cube";
                break;
            case 2:
                randomTetrominoName = "Prefabs/Tetromino_J";
                break;
            case 3:
                randomTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 4:
                randomTetrominoName = "Prefabs/Tetromino_Long";
                break;
            case 5:
                randomTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 6:
                randomTetrominoName = "Prefabs/Tetromino_T";
                break;
            case 7:
                randomTetrominoName = "Prefabs/Tetromino_Z";
                break;
        
            default:
                break;
        }
        return randomTetrominoName;
    }
    
    void GameOverSence()
    {
        Application.LoadLevel("GameOver");
    }

    public void GameOver(){
        //GameOver时有一个倒塌的效果,等待一下再切换场景  
        // Debug.Log("call GameOver function");
        isGameOver = true;              
        Invoke("GameOverSence",10);
    }

}
