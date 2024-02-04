using Newtonsoft.Json;

namespace MINUX.Backend.Worker.DataAccess;

public class JsonContext
{
}

public class JsonSet<TEntity> where TEntity : class
{
    /// <summary>
    /// Путь к директории файла
    /// </summary>
    private static readonly string PathDirectory =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/InformationSystem";

    /// <summary>
    /// Путь к файлу
    /// </summary>
    private static readonly string PathFile = $"{PathDirectory}/";

    public void AddAsync(TEntity entity)
    {

    }

    public TEntity? FindAsync()
    {
        return null;
    }

    public async Task<IEnumerable<TEntity?>> ToList()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathFile)!);

            if (!File.Exists(PathFile))
            {
                var newList = new List<TEntity>();
                CreateNewFile(newList);
                return newList;
            }

            return JsonConvert.DeserializeObject<List<TEntity>>(await File.ReadAllTextAsync(PathFile));
        }
        catch (Exception)
        {
            return new List<TEntity>();
        }
    }

    private static void CreateNewFile<T>(List<T>? items = null)
    {
        using var fileStream = File.Create(PathFile);

        if (items != null)
        {
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(items));
        }
    }
}
