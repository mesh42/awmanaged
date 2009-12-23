using System.Collections.Generic;

/// <summary>
/// The real plug-in interface we use to communicate across app-domains
/// </summary>
public interface IRun
{
    void Initialize(IDictionary<string, object> Variables);
    object Run(string StartMethod, params object[] Parameters);
    void Dispose(IDictionary<string, object> Variables);
}
