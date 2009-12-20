namespace AwManaged.Interfaces
{
    public interface IPropertyBag<TParent>
    {
        /// <summary>
        /// Gets or sets the parent object this property bag belongs to.
        /// </summary>
        /// <value>The parent.</value>
        TParent Parent { get; set; }
        object GetProperty(string key);
        void SetProperty(string key, object value);

    }
}