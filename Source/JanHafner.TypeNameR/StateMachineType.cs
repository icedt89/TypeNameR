namespace JanHafner.TypeNameR;

/// <summary>
/// Defines the type of the resolved state machine.
/// </summary>
[Flags]
internal enum StateMachineType : byte
{
    None = 0,

    /// <summary>
    /// It`s an iterator state machine (like <see cref="IEnumerable{T}"/>).
    /// </summary>
    Iterator = 1,

    /// <summary>
    /// It`s an async state machine (like <see cref="Task"/>, <see cref="Task{T}"/>, <see cref="ValueTask"/> or <see cref="ValueTask{T}"/>).
    /// </summary>
    Async = 2,

    /// <summary>
    /// It`s an async-iterator state machine (like <see cref="IAsyncEnumerable{T}"/>).
    /// </summary>
    AsyncIterator = Iterator | Async,
}