public interface ISerializable
{
    //string path { get; set; }
}

[System.Serializable]
public abstract class Serializable : ISerializable
{
    //public string path { get; set; }
}

public class Serializable<T> : Serializable where T : Serializable, new()
{
    private static T _instance;
    public static T instance => _instance ?? (_instance = FileManager.Open<T>());

    public static void Save() { instance.Save(); }
}
