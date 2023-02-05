
    using System.Collections;  
    using System.Collections.Generic;  
    using UnityEngine;  
    using UnityEngine.SceneManagement; 

    public class ChangeScene: MonoBehaviour {  

       // 1 references
        public int gameStartScene;

       // 0 references
        public void Scene1() {  
            SceneManager.LoadScene("MazeGen");  
        }  
        
    }   
