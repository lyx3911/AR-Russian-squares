using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game3D : MonoBehaviour
{   
    static int gridWidth = 10;
    static int gridHeight = 20;

    public int score = 0; //游戏分数D
    public bool isGameOver = false; //是否GameOver

    //记录每个方块是否被占 3维版本的为3维数组
    public static Transform[,,] grid = new Transform[gridWidth, gridWidth, gridHeight];

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

    //判断是否堆满，gameover， 没有超出返回false， 超出返回true
    public bool CheckIsAboveGrid(Tetromino3D tetromino){
        foreach(Transform mino in tetromino.transform){
            Vector3 pos = Round(mino.position);
            if(pos.y>gridHeight) return true;
        }
        return false;
    }

    //判断某层是否是满的
    public bool IsFullRowAt(int y){
        for(int x=0; x<gridWidth; x++){
            for(int z=0; z<gridWidth; z++){
                if(grid[x,z,y]==null) return false;
            }
        }
        return true;;
    }
    //删除某层的元素，y从0开始计算
    public void DeleteMinoAtRow(int y){
        for(int x=0; x<gridWidth; x++){
            for(int z=0;z<gridWidth; z++){
                Destroy(grid[x,z,y].gameObject);
                grid[x,z,y] = null;
            }
        }
    }

    //将第y行往下移动,y从0开始算
    public void MoveRowDown(int y){
        if(y<=0) return;
        for(int x=0; x<gridWidth; x++){
            for(int z=0;z<gridWidth; z++){
                if(grid[x,z,y]!=null){
                    grid[x,z, y-1] = grid[x,z,y];
                    grid[x,z,y] = null;
                    grid[x,z,y-1].position += new Vector3(0,-1,0); //更新方块位置
                }
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

    public void UpdateGrid(Tetromino3D tetromino){
        for(int y=0; y<gridHeight; y++){
            for(int x=0; x<gridWidth; x++){
                for(int z=0;z<gridWidth; z++){
                    if(grid[x,z,y]!=null){
                        //如果图形本来就在这个位置,要进行更新,删除原来的位置，填入新的位置
                        if(grid[x,z,y].parent==tetromino.transform) grid[x,z,y]=null; 
                    }
                }
            }
        }
        //添加新的位置
        foreach (Transform mino in tetromino.transform){
            Vector3 pos = Round(mino.position);
            if(pos.y<=gridHeight){
                grid[(int)pos.x+4,(int)pos.z+4,(int)pos.y-1] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos){
        if(pos.y>gridHeight) return null;
        else return grid[(int)pos.x+4, (int)pos.z+4, (int)pos.y-1];
    }

    public void SpawnNextTetromino(){
        //添加的方块是ImageTarget的组件
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector3(0,20,0), Quaternion.identity);
        GameObject ImgTar = GameObject.Find("MyImageTarget");
        nextTetromino.transform.parent = ImgTar.transform;
    }

    public bool CheckIsInsideGrid(Vector3 pos){
        return ((int)pos.x >-gridWidth/2 && (int)pos.x<gridWidth/2+1 && (int)pos.y>0 && (int)pos.z >-gridWidth/2 && (int)pos.z<gridWidth/2+1 );
    }
    public Vector3 Round(Vector3 pos){
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }

    string GetRandomTetromino() {

        //随机载入3D版的模型

        int randomTetromino = Random.Range(1,16);

        string randomTetrominoName = "Prefabs/3DSquare/3DCube1";

        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "Prefabs/3DSquare/3DCube2";
                break;
            case 2:
                randomTetrominoName = "Prefabs/3DSquare/3DCube3";
                break;
            case 3:
                randomTetrominoName = "Prefabs/3DSquare/3DL1";
                break;
            case 4:
                randomTetrominoName = "Prefabs/3DSquare/3DL2";
                break;
            case 5:
                randomTetrominoName = "Prefabs/3DSquare/3DL3";
                break;
            case 6:
                randomTetrominoName = "Prefabs/3DSquare/3DLong1";
                break;
            case 7:
                randomTetrominoName = "Prefabs/3DSquare/3DLong2";
                break;

            case 8:
                randomTetrominoName = "Prefabs/3DSquare/3DLong3";
                break;

            case 9:
                randomTetrominoName = "Prefabs/3DSquare/3DT1";
                break;

            case 10:
                randomTetrominoName = "Prefabs/3DSquare/3DT2";
                break;

            case 11:
                randomTetrominoName = "Prefabs/3DSquare/3DT3";
                break;

            case 12:
                randomTetrominoName = "Prefabs/3DSquare/3DZ1";
                break;
            
            case 13:
                randomTetrominoName = "Prefabs/3DSquare/3DZ2";
                break;
            
            case 14:
                randomTetrominoName = "Prefabs/3DSquare/3DZ3";
                break;
            
            case 15:
                randomTetrominoName = "Prefabs/3DSquare/3DCube1";
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
