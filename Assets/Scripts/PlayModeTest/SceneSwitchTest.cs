using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneSwitchTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ScenesInBuildSettings() {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        Assert.IsTrue(totalScenes != SceneManager.GetSceneByPath("Scenes/MainScenes/Main_Menu").buildIndex, "Checking if Main_Menu is in included in the build settings");
        Assert.IsTrue(totalScenes != SceneManager.GetSceneByPath("Scenes/MainScenes/Level_1").buildIndex, "Checking if Level_1 is in included in the build settings");
        Assert.IsTrue(totalScenes != SceneManager.GetSceneByPath("Scenes/MainScenes/Level_2").buildIndex, "Checking if Level_2 is in included in the build settings");
        Assert.IsTrue(totalScenes != SceneManager.GetSceneByPath("Scenes/MainScenes/Level_3").buildIndex, "Checking if Level_3 is in included in the build settings");
        Assert.IsTrue(totalScenes != SceneManager.GetSceneByPath("Scenes/MainScenes/You_Win_Screen").buildIndex, "Checking if You_Win_Screen is in included in the build settings");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator SceneSwitchTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
