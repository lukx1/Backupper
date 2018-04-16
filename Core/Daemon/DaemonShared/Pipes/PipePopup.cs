using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public class PipePopup
    {
        /// <summary>
        /// Text
        /// </summary>
        public string T { get; set; }
        /// <summary>
        /// Caption
        /// </summary>
        public string C { get; set; }
        /// <summary>
        /// Buttons
        /// </summary>
        public Buttons B { get; set; }
        /// <summary>
        /// Icons
        /// </summary>
        public Icons I { get; set; }

        public static PipePopup FromJson(string s)
        {
            return JsonConvert.DeserializeObject<PipePopup>(s);
        }

        public static string Serialize(string s)
        {
            return JsonConvert.SerializeObject(s);
        }
        /// <summary>
        /// Jestli má DS odpovědět dialog resultem v DIALOG_RESULT
        /// </summary>
        public bool R { get; set; } = false;

        public enum Icons : byte
        {
            //
            // Summary:
            //     The message box contain no symbols.
            None = 0,
            //
            // Summary:
            //     The message box contains a symbol consisting of a white X in a circle with a
            //     red background.
            Hand = 16,
            //
            // Summary:
            //     The message box contains a symbol consisting of white X in a circle with a red
            //     background.
            Stop = 16,
            //
            // Summary:
            //     The message box contains a symbol consisting of white X in a circle with a red
            //     background.
            Error = 16,
            //
            // Summary:
            //     The message box contains a symbol consisting of a question mark in a circle.
            //     The question-mark message icon is no longer recommended because it does not clearly
            //     represent a specific type of message and because the phrasing of a message as
            //     a question could apply to any message type. In addition, users can confuse the
            //     message symbol question mark with Help information. Therefore, do not use this
            //     question mark message symbol in your message boxes. The system continues to support
            //     its inclusion only for backward compatibility.
            Question = 32,
            //
            // Summary:
            //     The message box contains a symbol consisting of an exclamation point in a triangle
            //     with a yellow background.
            Exclamation = 48,
            //
            // Summary:
            //     The message box contains a symbol consisting of an exclamation point in a triangle
            //     with a yellow background.
            Warning = 48,
            //
            // Summary:
            //     The message box contains a symbol consisting of a lowercase letter i in a circle.
            Asterisk = 64,
            //
            // Summary:
            //     The message box contains a symbol consisting of a lowercase letter i in a circle.
            Information = 64
        }

        public enum Buttons : byte
        {
            //
            // Summary:
            //     The message box contains an OK button.
            OK = 0,
            //
            // Summary:
            //     The message box contains OK and Cancel buttons.
            OKCancel = 1,
            //
            // Summary:
            //     The message box contains Abort, Retry, and Ignore buttons.
            AbortRetryIgnore = 2,
            //
            // Summary:
            //     The message box contains Yes, No, and Cancel buttons.
            YesNoCancel = 3,
            //
            // Summary:
            //     The message box contains Yes and No buttons.
            YesNo = 4,
            //
            // Summary:
            //     The message box contains Retry and Cancel buttons.
            RetryCancel = 5
        }

    }
}
