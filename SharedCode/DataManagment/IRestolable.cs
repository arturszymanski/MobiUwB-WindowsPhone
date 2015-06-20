namespace SharedCode.DataManagment
{
    public interface IRestolable<T> where 
        T : new()
    {
        T GetDefaults();
    }
}
