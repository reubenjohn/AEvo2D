public interface ISensor
{
    float[] GetReceptors();
    void OnRefresh();
    void OnReset();
}