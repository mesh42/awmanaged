namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IAuthorization<TObject> where TObject : IHaveAuthorization
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        string Role { get; }
        /// <summary>
        /// Gets or sets the authorizable templated object.
        /// </summary>
        /// <value>The authorizable.</value>
        TObject Authorizable { get; }
        /// <summary>
        /// Adds the authorization.
        /// </summary>
        void AddAuthorization();

        bool HasAuthorization();
        /// <summary>
        /// Removes the authorization.
        /// </summary>
        void RemoveAuthorization();
    }
}