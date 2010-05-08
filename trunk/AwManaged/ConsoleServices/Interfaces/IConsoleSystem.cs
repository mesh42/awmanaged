using SharedMemory;using System;

namespace AwManaged.ConsoleServices.Interfaces
{
    /// <summary>
    /// Console System Interface.
    /// </summary>
    public interface IConsoleSystem
    {
        ConsoleColor BackgroundColor {get;set;}
        string Title { get; set; }
        void Clear();
        /// <summary>
        /// Gets a value indicating whether this instance has its prompt enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is prompt enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsPromptEnabled { get; }
        /// <summary>
        /// Reads a line of text.
        /// </summary>
        void ReadLine();
        /// <summary>
        /// Writes a line of text accoording to a color schema.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        void WriteLine(ConsoleMessageType type, string text);
        /// <summary>
        ///Writes a line of text with nominal color.
        /// </summary>
        /// <param name="text">The text.</param>
        void WriteLine(string text);
        /// <summary>
        /// Writes text accoording to a color schema.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        void Write(ConsoleMessageType type, string text);
        /// <summary>
        /// Writes text with nomonal color.
        /// </summary>
        /// <param name="text">The text.</param>
        void Write(string text);
    }
}
