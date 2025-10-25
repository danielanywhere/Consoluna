/*
 * Copyright (c). 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	WindowsConsoleInputHandler																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// User input handler specifically for Windows Console.
	/// </summary>
	public class ConsoleInputHandlerWindows : IConsoleInputHandler
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//	** CONSTANTS **
		//*-----------------------------------------------------------------------*
		private const int KeyBufferLength = 32;

		private const int STD_OUTPUT_HANDLE = -11;
		private const int STD_INPUT_HANDLE = -10;

		private const uint ENABLE_PROCESSED_INPUT = 0x0001;
		private const uint ENABLE_LINE_INPUT = 0x0002;
		private const uint ENABLE_ECHO_INPUT = 0x0004;
		private const uint ENABLE_WINDOW_INPUT = 0x0008;
		private const uint ENABLE_MOUSE_INPUT = 0x0010;
		private const uint ENABLE_INSERT_MODE = 0x0020;
		private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
		private const uint ENABLE_EXTENDED_FLAGS = 0x0080;

		private const ushort KEY_EVENT = 0x0001;
		private const ushort MOUSE_EVENT = 0x0002;
		private const ushort WINDOW_BUFFER_SIZE_EVENT = 0x0004;

		private const uint WAIT_OBJECT_0 = 0;
		private const uint WAIT_TIMEOUT = 0x00000102;

		//	** STRUCTURES **
		//*-----------------------------------------------------------------------*
		//	Be sure to note explicit layout and offsets.
		[StructLayout(LayoutKind.Sequential)]
		private struct COORD
		{
			public short X;
			public short Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct FOCUS_EVENT_RECORD
		{
			public int SetFocus; // BOOL
		}

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		private struct INPUT_RECORD
		{
			[FieldOffset(0)] public ushort EventType;
			[FieldOffset(4)] public KEY_EVENT_RECORD KeyEvent;
			[FieldOffset(4)] public MOUSE_EVENT_RECORD MouseEvent;
			[FieldOffset(4)] public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct KEY_EVENT_RECORD
		{
			// Because of packing, BOOL is DWORD. Use int to avoid ambiguity.
			public int KeyDown;
			public ushort RepeatCount;
			public ushort VirtualKeyCode;
			public ushort VirtualScanCode;
			//	This value maps to uChar union unicode member.
			public char UnicodeChar;
			public uint ControlKeyState;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct MENU_EVENT_RECORD
		{
			public uint CommandId;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct MOUSE_EVENT_RECORD
		{
			public COORD MousePosition;
			public uint ButtonState;
			public uint ControlKeyState;
			public uint EventFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct WINDOW_BUFFER_SIZE_RECORD
		{
			public COORD Size;
		}



		// ** WINDOWS API FUNCTIONS **
		//*-----------------------------------------------------------------------*
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle,
			out uint lpMode);

		[DllImport("kernel32.dll")]
		private static extern bool ReadConsoleInput(
				IntPtr hConsoleInput,
				[Out] INPUT_RECORD[] lpBuffer,
				uint nLength,
				out uint lpNumberOfEventsRead);

		[DllImport("kernel32.dll")]
		private static extern bool SetConsoleMode(IntPtr hConsoleHandle,
			uint dwMode);

		[DllImport("kernel32.dll")]
		private static extern bool PeekConsoleInput(IntPtr hConsoleInput,
			[Out] INPUT_RECORD[] lpBuffer, uint nLength,
			out uint lpNumberOfEventsRead);

		[DllImport("kernel32.dll")]
		private static extern uint WaitForSingleObject(IntPtr hHandle,
			uint dwMilliseconds);

		//	** LOCAL PRIVATE **
		//*-----------------------------------------------------------------------*
		private bool mInitialized = false;
		private uint mWindowConsoleMode = 0;
		private INPUT_RECORD[] mWindowRecord = new INPUT_RECORD[KeyBufferLength];
		private IntPtr mWindowInputHandle = default(IntPtr);

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the WindowsConsoleInputHandler item.
		/// </summary>
		public ConsoleInputHandlerWindows()
		{
			uint windowConsoleMode = 0;
			IntPtr windowOutputHandle = default(IntPtr);

			windowOutputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
			if(GetConsoleMode(windowOutputHandle, out windowConsoleMode))
			{
				mWindowConsoleMode = windowConsoleMode;
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the WindowsConsoleInputHandler item.
		/// </summary>
		/// <param name="WindowConsoleMode">
		/// The starting window console mode.
		/// </param>
		public ConsoleInputHandlerWindows(uint windowConsoleMode)
		{
			mWindowConsoleMode = windowConsoleMode;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Close																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Close any open connections and reset system settings.
		/// </summary>
		public void Close() { }
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Initialize																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize the input handler.
		/// </summary>
		public void Initialize()
		{
			if(!mInitialized)
			{
				mWindowConsoleMode &=
					~ENABLE_QUICK_EDIT_MODE &
					~ENABLE_LINE_INPUT &
					~ENABLE_ECHO_INPUT;
				mWindowConsoleMode |= ENABLE_MOUSE_INPUT |
					ENABLE_EXTENDED_FLAGS |
					ENABLE_WINDOW_INPUT |
					ENABLE_PROCESSED_INPUT |
					ENABLE_INSERT_MODE;
			}

			mWindowInputHandle = GetStdHandle(STD_INPUT_HANDLE);
			SetConsoleMode(mWindowInputHandle, mWindowConsoleMode);

			mInitialized = true;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	InputInfo																															*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="InputInfo">InputInfo</see>.
		///// </summary>
		//private List<ConsoleInputEventArgs> mInputInfo =
		//	new List<ConsoleInputEventArgs>();
		///// <summary>
		///// Get/Set a reference to the input info collection assigned to this
		///// instance.
		///// </summary>
		//public List<ConsoleInputEventArgs> InputInfo
		//{
		//	get { return mInputInfo; }
		//	set { mInputInfo = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReadInput																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read any available input.
		/// </summary>
		/// <returns>
		/// Reference to a single event read from the input, if present. Otherwise,
		/// null.
		/// </returns>
		public ConsoleInputEventArgs ReadInput()
		{
			int colCount = 0;
			KEY_EVENT_RECORD keyEvent = default(KEY_EVENT_RECORD);
			MOUSE_EVENT_RECORD mouseEvent = default(MOUSE_EVENT_RECORD);
			INPUT_RECORD record = default(INPUT_RECORD);
			ConsoleInputEventArgs result = null;
			int rowCount = 0;
			uint windowEventCount = 0;
			uint windowReadCount = 0;

			if(mInitialized)
			{
				PeekConsoleInput(mWindowInputHandle, mWindowRecord, KeyBufferLength,
					out windowEventCount);
				if(windowEventCount > 0)
				{
					ReadConsoleInput(mWindowInputHandle, mWindowRecord, 1,
						out windowReadCount);
					record = mWindowRecord[0];
					switch(record.EventType)
					{
						case KEY_EVENT:
							//Console.WriteLine("Keyboard input received.");
							keyEvent = record.KeyEvent;
							//Console.WriteLine(
							//	"Keyboard: " +
							//	$"Key {(Convert.ToBoolean(keyEvent.KeyDown) ? "Dn" : "Up")}, " +
							//	$"Ctrl {keyEvent.ControlKeyState} -> " +
							//	$"{(char)keyEvent.UnicodeChar}"
							//	);
							if(keyEvent.KeyDown == 1)
							{
								result = new ConsoleInputKeyboardEventArgs()
								{
									KeyCharacter = keyEvent.UnicodeChar,
									KeyCode = (int)keyEvent.UnicodeChar
								};
							}
							break;
						case MOUSE_EVENT:
							//Console.WriteLine("Mouse input received.");
							mouseEvent = record.MouseEvent;
							//Console.WriteLine(
							//	"Mouse: (" +
							//	$"{mouseEvent.MousePosition.X}, " +
							//	$"{mouseEvent.MousePosition.Y}) - " +
							//	$"ButtonState: {mouseEvent.ButtonState}");
							if(mouseEvent.ButtonState == 1)
							{
								result = new ConsoleInputMouseEventArgs()
								{
									MouseX = mouseEvent.MousePosition.X,
									MouseY = mouseEvent.MousePosition.Y
								};
							}
							break;
						//	NOTE: Window size handled at host in this version.
						//case WINDOW_BUFFER_SIZE_EVENT:
						//	colCount = record.WindowBufferSizeEvent.Size.X;
						//	rowCount = record.WindowBufferSizeEvent.Size.Y;
						//	Console.WriteLine(
						//		$"Window buffer size changed: {colCount} x {rowCount}");
						//	break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
