using System.Collections.Generic;

public interface ISensor
{
    float[] GetReceptors();
    IEnumerable<string> GetLabels();
    void OnRefresh();
    void OnReset();
}