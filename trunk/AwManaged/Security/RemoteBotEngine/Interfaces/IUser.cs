using AwManaged.Storage.Interfaces;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IUser : IHaveAuthorization
    {
        string Name { get; }
        string Password { get; }
        string LastLogonDate { get; }
        string Email { get; }
        string NameToLower { get; }
        string EmailToLower { get; }
        bool IsLockedOut { get; }
        int MaxConnections { get; }
        /// <summary>
        /// Registers the user using the specified storage client.
        /// </summary>
        /// <returns></returns>
        RegistrationResult RegisterUser<TConnectionInterface>(IStorageClient<TConnectionInterface> storage);
        /// <summary>
        /// Determines whether the specified user is authenticated.
        /// </summary>
        /// <typeparam name="TConnectionInterface">The type of the connection interface.</typeparam>
        /// <param name="storage">The storage.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user is authenticated; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthenticated<TConnectionInterface>(IStorageClient<TConnectionInterface> storage);
    }
}