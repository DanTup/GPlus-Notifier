using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AwesomiumSharp;
using System.Diagnostics;

namespace DanTup.GPlusNotifier {
    /// <summary>
    /// Converts between Awesomium CursorType and WinForms Cursor.
    /// </summary>
    static class CursorConvertion {
        private static readonly Dictionary<CursorType, Cursor> CursorDictionary = new Dictionary<CursorType, Cursor> {
            { CursorType.None, Cursors.Default },
            { CursorType.Pointer, Cursors.Arrow },
            { CursorType.Ibeam, Cursors.IBeam },
            { CursorType.Hand, Cursors.Hand },
        };

        public static Cursor GetCursor(CursorType cursorType) {
            Cursor result;
            if (CursorDictionary.TryGetValue(cursorType, out result)) {
                return result;
            } else {
                Debug.WriteLine("Unknown cursor type " + cursorType);
                return null;
            }
        }
    }
}
