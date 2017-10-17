using UnityEditor.SceneManagement;
using UnityEditor;

[InitializeOnLoad]
public class AutoSave
{
    static AutoSave()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
    }

    static void OnPlaymodeStateChanged()
    {
        if(EditorApplication.isPlaying == false)
        {
            EditorSceneManager.SaveOpenScenes();
        }
    }
	
}
