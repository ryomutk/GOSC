using UnityEngine;
using System.Collections;

/// <summary>
/// Monobehaviour class(abstract) implements interface:InputInterface
/// For serializing
/// </summary>
public abstract class InputRequestHandlerComponent:MonoBehaviour,InputRequestHandler
{
    public abstract Task RequestInput();    
}


/// <summary>
/// 
/// </summary>
public interface InputRequestHandler
{
    /// <summary>
    /// input request.
    /// </summary>
    /// <returns>task to prepare input</returns>
    Task RequestInput();
}