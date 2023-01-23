using UnityEngine;
using System.Collections;

/// <summary>
/// Monobehaviour class(abstract) implements interface:InputInterface
/// For serializing
/// </summary>
public abstract class OutputRequestHandlerComponent:MonoBehaviour,OutputRequestHandler
{
    public abstract Task RequestOutput(object args);    
}


/// <summary>
/// 
/// </summary>
public interface OutputRequestHandler
{
    /// <summary>
    /// input request.
    /// </summary>
    /// <returns>task to prepare input</returns>
    Task RequestOutput(object args);
}