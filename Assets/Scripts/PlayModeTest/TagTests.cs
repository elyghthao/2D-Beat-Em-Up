using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TagTests
{
    [UnityTest]
    public IEnumerator CheckPlayer() {
        string[] scenes = {"Scenes/MainScenes/Level_1", "Scenes/MainScenes/Level_2", "Scenes/MainScenes/Level_3"};
        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(scenes[0]);
        while (!sceneLoader.isDone) yield return null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 1, "Checking if there is one object with the player tag in " + scenes[0]
            + ". \nFound: " + players + " in array length of " + players.Length);
        
        sceneLoader = SceneManager.LoadSceneAsync(scenes[1]);
        while (!sceneLoader.isDone) yield return null;
        players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 1, "Checking if there is one object with the player tag in " + scenes[1]
            + ". \nFound: " + players + " in array length of " + players.Length);
        
        sceneLoader = SceneManager.LoadSceneAsync(scenes[2]);
        while (!sceneLoader.isDone) yield return null;
        players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 1, "Checking if there is one object with the player tag in " + scenes[2]
            + ". \nFound: " + players + " in array length of " + players.Length);
    }

    [UnityTest]
    public IEnumerator CheckGround() {
        string[] scenes = {"Scenes/MainScenes/Level_1", "Scenes/MainScenes/Level_2", "Scenes/MainScenes/Level_3"};
        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(scenes[0]);
        while (!sceneLoader.isDone) yield return null;
        GameObject[] grounds = FindGameObjectsInLayer(10);
        Assert.IsTrue(grounds.Length == 1, "Checking if there is one object with the ground layer in " + scenes[0]
            + ". \nFound: " + grounds + " in array length of " + grounds.Length);
        
        sceneLoader = SceneManager.LoadSceneAsync(scenes[1]);
        while (!sceneLoader.isDone) yield return null;
        grounds = FindGameObjectsInLayer(10);
        Assert.IsTrue(grounds.Length == 1, "Checking if there is one object with the ground layer in " + scenes[1]
            + ". \nFound: " + grounds + " in array length of " + grounds.Length);
        
        sceneLoader = SceneManager.LoadSceneAsync(scenes[2]);
        while (!sceneLoader.isDone) yield return null;
        grounds = FindGameObjectsInLayer(10);
        Assert.IsTrue(grounds.Length == 1, "Checking if there is one object with the ground layer in " + scenes[2]
            + ". \nFound: " + grounds + " in array length of " + grounds.Length);
    }
    
    /// <summary>
    /// Function provided by Hellium on Stack Overflow: https://gamedev.stackexchange.com/questions/136323/how-do-i-get-objects-using-layer-name.
    /// Finds all objects of specified layer.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    GameObject[] FindGameObjectsInLayer(int layer){
        var goArray = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++){
            if (goArray[i].layer == layer){
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0){
            return null;
        }
        return goList.ToArray();
    }
}
