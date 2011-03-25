namespace FluentSiteMap.Testing
{
    /// <summary>
    /// The result of a call to <see cref="ContainsStateExtensions.ContainsState(object,object)"/>.
    /// </summary>
    public class ContainsStateResult
    {
        /// <summary>
        /// Gets whether or not the state of the expected object was contained within the actual object.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// If <see cref="Success"/> is false, contains the reason why the states don't match.
        /// </summary>
        public string FailReason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsStateResult"/> class 
        /// that represents a successful state comparison (<see cref="Success"/> is true).
        /// </summary>
        public ContainsStateResult()
        {
            Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsStateResult"/> class 
        /// that represents a failed state comparison (<see cref="Success"/> is false) 
        /// with the associated location and message.
        /// </summary>
        public ContainsStateResult(string location, string message)
            : this(location, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsStateResult"/> class 
        /// that represents a failed state comparison (<see cref="Success"/> is false) 
        /// with the associated location and format message and values.
        /// </summary>
        public ContainsStateResult(string location, string format, params object[] values)
        {
            var message = string.Format(format, values);
            FailReason = string.Format("{0}: {1}", location, message);
        }
    }
}