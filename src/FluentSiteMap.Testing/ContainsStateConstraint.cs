using NUnit.Framework.Constraints;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// An NUnit <see cref="Constraint"/> that will execute the 
    /// <see cref="ContainsStateExtensions.ContainsState(object,object)"/> method.
    /// </summary>
    public class ContainsStateConstraint
        : Constraint
    {
        private readonly object _expectedState;
        private ContainsStateResult _result;

        /// <summary>
        /// Intializes a new instance of the <see cref="ContainsStateConstraint"/> class 
        /// with the specified expected object.
        /// </summary>
        public ContainsStateConstraint(object expectedState)
        {
            _expectedState = expectedState;
        }

        /// <summary>
        /// Overrides the <see cref="Constraint.Matches(object)"/> method 
        /// in order to perform the state comparison.
        /// </summary>
        public override bool Matches(object actualState)
        {
            _result = actualState.ContainsState(_expectedState);

            actual = _result.Actual;

            return _result.Success;
        }

        /// <summary>
        /// Overrides the <see cref="Constraint.WriteMessageTo"/> method 
        /// in order to write the failure message.
        /// </summary>
        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteMessageLine(_result.FailReason);

            if (_result.Actual != null && _result.Expected != null)
                base.WriteMessageTo(writer);
        }

        /// <summary>
        /// Overrides the <see cref="Constraint.WriteDescriptionTo"/> method 
        /// in order to write the failure description.
        /// </summary>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteExpectedValue(_result.Expected);
        }
    }
}