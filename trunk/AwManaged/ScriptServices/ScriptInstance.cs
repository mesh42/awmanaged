using SharedMemory;using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace AwManaged.ScriptServices
{
    /// <summary>
    /// Factory class to create objects exposing IRun
    /// </summary>
    public class ScriptInstance : MarshalIndefinite
    {
        private const BindingFlags bfi = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;

        /// <summary>
        /// Factory method to create an instance of the type whose name is specified,
        /// using the named assembly file and the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="assemblyFile">The name of a file that contains an assembly where the type named typeName is sought.</param>
        /// <param name="typeName">The name of the preferred type.</param>
        /// <param name="constructArgs">An array of arguments that match in number, order, and type the parameters of the constructor to invoke, or null for default constructor.</param>
        /// <returns>
        /// The return value is the created object represented as IRun.
        /// </returns>
        public IRun Create(string assemblyFile, string typeName, object[] constructArgs)
        {
            // ReSharper disable PossibleNullReferenceException
            return (IRun)Activator.CreateInstanceFrom(
            assemblyFile, typeName, false, bfi, null, constructArgs,
            null, null, null).Unwrap();
            // ReSharper restore PossibleNullReferenceException
        }
        /// <summary>
        /// Creates an instance of the specified type within specified appdomain.
        /// this can be desired when global application variables need to be used by the interpreted script.
        /// </summary>
        /// <param name="appdomain">The appdomain.</param>
        /// <param name="assemblyFile">The assembly file.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="constructArgs">The construct args.</param>
        /// <returns></returns>
        public IRun Create(AppDomain appdomain, string assemblyFile, string typeName, object[] constructArgs)
        {
            // ReSharper disable PossibleNullReferenceException
            return (IRun)Activator.CreateInstanceFrom(appdomain,
            assemblyFile, typeName, false, bfi, null, constructArgs,
            null, null, null).Unwrap();
            // ReSharper restore PossibleNullReferenceException
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="constructArgs">The construct args.</param>
        /// <returns></returns>
        public IRun Create(Type type, object[] constructArgs)
        {
            return  (IRun) ((ObjectHandle) Activator.CreateInstance(type, constructArgs)).Unwrap();
        }
    }
}