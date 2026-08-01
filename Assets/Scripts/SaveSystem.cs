using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";
    public static readonly string FILE_EXT = ".json";

    public static void save(string filename, string dataToSave)
    {
        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);

        File.WriteAllText(SAVE_FOLDER + filename + FILE_EXT, dataToSave);
    }

    public static string load(string filename)
    {
        string path = SAVE_FOLDER + filename + FILE_EXT;

        if (File.Exists(path))
            return File.ReadAllText(path);

        return null;
    }
}
