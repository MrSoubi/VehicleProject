using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneMenu : MonoBehaviour
{
    [MenuItem("Level Loader/00_ Load Main Menu")]
    public static void LoadMainMenu()
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene("Assets/Game/99_Scenes/Menus/MainMenu.unity", OpenSceneMode.Single);
    }


    [MenuItem("Level Loader/01_ Load Arena 1")]
    public static void LoadArena1()
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene("Assets/Game/99_Scenes/Arenas/Basic_Arena.unity", OpenSceneMode.Single);
    }

    [MenuItem("Level Loader/02_ Load Arena 2")]
    public static void LoadArena2()
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene("Assets/Game/99_Scenes/Arenas/BasicLevel.unity", OpenSceneMode.Single);
    }

    [MenuItem("Level Loader/03_ Load Arena 3")]
    public static void LoadArena3()
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene("Assets/Game/99_Scenes/Arenas/Bomb_Arena.unity", OpenSceneMode.Single);
    }
}