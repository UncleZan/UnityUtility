using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class ProjectFoldersBuilder : ScriptableWizard
{
    private string SFGUID;
    public bool projectFolders = false, sceneFolders = false;
    public List<Folder> folders = new List<Folder>() {
        new Folder("3rd-Party", new List<string>()),
        new Folder("Animations", new List<string>()),
        new Folder("Artwork", new List<string>()),
        new Folder("Audio", new List<string>() { "Music", "SFX" }),
        new Folder("Materials", new List<string>()),
        new Folder("Models", new List<string>()),
        new Folder("Plugins", new List<string>()),
        new Folder("Prefabs", new List<string>()),
        new Folder("Resources", new List<string>()),
        new Folder("Textures", new List<string>()),
        new Folder("Sandbox", new List<string>()),
        new Folder("Scenes", new List<string>() { "Levels", "Other" }),
        new Folder("Scripts", new List<string>() { "Classes", "Editor", "Interfaces", "States" }),
        new Folder("Sprites", new List<string>())
    };

    public List<Folder> scene = new List<Folder>() {
        new Folder("Management", new List<string>()),
        new Folder("GUI", new List<string>()),
        new Folder("Camera", new List<string>()),
        new Folder("Lights", new List<string>()),
        new Folder("World", new List<string>(){ "Terrain", "Props", "Character" }),
        new Folder("_Dynamic", new List<string>()) };

    [MenuItem("Edit/Set Game Structure...")]
    static void CreateWizard()
    {
        DisplayWizard("Set Game Structure", typeof(ProjectFoldersBuilder));
    }

    //Called when the window first appears
    void OnEnable()
    {

    }
    
    //Create button click
    void OnWizardCreate()
    {
        //create all the folders required in a project
        //primary and sub folders
        #region ProjectFolders
        if (projectFolders)
        {
            Debug.Log("FOLDERS CREATED");
            foreach (Folder folder in folders)
            {
                if (!AssetDatabase.IsValidFolder("Assets/" + folder.parent))
                    AssetDatabase.CreateFolder("Assets", folder.parent);
            }

            AssetDatabase.Refresh();

            for (int i = 0; i < folders.Count; i++)
            {
                foreach (string sf in folders[i].subs)
                {
                    if (!AssetDatabase.IsValidFolder("Assets/" + folders[i].parent + "/" + sf))
                        AssetDatabase.CreateFolder("Assets/" + folders[i].parent, sf);
                }
            }
        }
        #endregion

        #region SceneHierarchy
        if (sceneFolders)
        {
            Debug.Log("SCENE CREATED");
            GameObject newGo;
            GameObject subGameObject;
            for (int i = 0; i < scene.Count; i++)
            {
                newGo = GameObject.Find(scene[i].parent);
                if (newGo == null)
                    newGo = new GameObject(scene[i].parent);

                foreach (string sf in scene[i].subs)
                {
                    if (newGo.transform.Find(sf) == null)
                    {
                        subGameObject = new GameObject(sf);
                        subGameObject.transform.SetParent(newGo.transform);
                    }
                }
            }
        }        
        #endregion
    }

    //Runs whenever something changes in the editor window...
    //void OnWizardUpdate()
    //{

    //}

    void ClearSubFolders()
    {
        //if (folder.Count > 0) folder.Clear();
    }
}

[System.Serializable]
public struct Folder{

    public Folder(string name, List<string> subNames)
    {
        parent = name;
        subs = subNames;
    }
    public string parent;
    public List<string> subs;
}

[System.Serializable]
public struct SceneContainer
{

    public SceneContainer(string name, List<string> subNames)
    {
        parent = name;
        subs = subNames;
    }
    public string parent;
    public List<string> subs;
}


