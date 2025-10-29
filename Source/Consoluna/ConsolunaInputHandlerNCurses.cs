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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mindmagma.Curses;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaInputHandlerNCurses																						*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Console input handler for processing NCurses activity in Consoluna.
	/// </summary>
	public class ConsolunaInputHandlerNCurses : IConsolunaInputHandler
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Reference to the STDIO screen.
		/// </summary>
		private IntPtr mScreen;
		/// <summary>
		/// Mapped mouse key character.
		/// </summary>
		private int mKeyMouse;

		//*-----------------------------------------------------------------------*
		//* GetKeyCode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the extended key code for the specified character code.
		/// </summary>
		/// <param name="code1">
		/// The primary ncurses code to inspect.
		/// </param>
		/// <param name="code2">
		/// The secondary ncurses code to inspect.
		/// </param>
		/// <returns>
		/// The consistent key code to return to the caller.
		/// </returns>
		private static int GetKeyCode(int code1, int code2)
		{
			int result = 0;

			if((code1 > 0 && code1 < 9) ||
				(code1 > 10 && code1 < 27))
			{
				//	So yea. We kind of have to skip support for the following:
				//	- 09: [Ctrl][I]. This is universal [Tab].
				//	- 10: [Ctrl][J]. This is [Enter] in Unix.
				result = code1 + 96;
			}
			else if(code1 == 27 &&
				code2 > 0 && code2 < 27)
			{
				result = code2 + 96;
			}
			else if(code1 > 31 && code1 < 127)
			{
				result = code1;
			}
			else if(code1 == 27 && code2 > 31 && code2 < 127)
			{
				result = code2;
			}
			else
			{
				switch(code1)
				{
					case 263:
						//	Backspace.
						result = 8;
						break;
					case 330:
						//	Delete.
						result = 127;
						break;
					case 9:
						//	Tab.
						result = 9;
						break;
					case 10:
					//	Unix [Enter].
					case 13:
					//	Actual physical [Enter] key on every modern keyboard.
					case 343:
						//	KEY_ENTER.
						result = '\r';
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetKeyCharacter																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Retrieve the recognizable character associated with the specified
		/// character code.
		/// </summary>
		/// <param name="code1">
		/// The primary ncurses code to inspect.
		/// </param>
		/// <param name="code2">
		/// The secondary ncurses code to inspect.
		/// </param>
		/// <returns>
		/// The recognizable character to return to the caller.
		/// </returns>
		private static char GetKeyCharacter(int code1, int code2)
		{
			char result = '\0';
			if((code1 > 0 && code1 < 9) ||
				(code1 > 10 && code1 < 27))
			{
				//	So yea. We kind of have to skip support for the following:
				//	- 09: [Ctrl][I]. This is universal [Tab].
				//	- 10: [Ctrl][J]. This is [Enter] in Unix.
				result = (char)(code1 + 96);
			}
			else if(code1 == 27 &&
				code2 > 0 && code2 < 27)
			{
				result = (char)(code2 + 96);
			}
			else if(code1 > 31 && code1 < 127)
			{
				result = (char)code1;
			}
			else if(code1 == 27 && code2 > 31 && code2 < 127)
			{
				result = (char)code2;
			}
			else
			{
				switch(code1)
				{
					case 263:
						//	Backspace.
						result = '\b';
						break;
					case 330:
						//	Delete.
						result = (char)127;
						break;
					case 9:
						result = '\t';
						break;
					case 10:
						//	Unix [Enter].
					case 13:
						//	Actual physical [Enter] key on every modern keyboard.
					case 343:
						//	KEY_ENTER.
						result = '\r';
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetKeyModifier																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the key modifier active for the current key code.
		/// </summary>
		/// <param name="code1">
		/// The primary ncurses code to inspect.
		/// </param>
		/// <param name="code2">
		/// The secondary ncurses code to inspect.
		/// </param>
		/// <returns>
		/// The input key modifier type associated with the current code, if
		/// found. Otherwise, None.
		/// </returns>
		private static ConsolunaInputKeyModifierType GetKeyModifier(int code1,
			int code2)
		{
			ConsolunaInputKeyModifierType result =
				ConsolunaInputKeyModifierType.None;

			if((code1 > 0 && code1 < 9) ||
				(code1 > 10 && code1 < 27))
			{
				//	So yea. We kind of have to skip support for the following:
				//	- 09: [Ctrl][I]. This is universal [Tab].
				//	- 10: [Ctrl][J]. This is [Enter] in Unix.
				result |= ConsolunaInputKeyModifierType.Ctrl;
			}
			else if(code1 == 27 && code2 > 0)
			{
				result |= ConsolunaInputKeyModifierType.Alt;
				if(code2 > 64 && code2 < 91)
				{
					result |= ConsolunaInputKeyModifierType.Shift;
				}
			}
			else if((code1 >= 33 && code1 <= 38) ||
				(code1 >= 40 && code1 <= 43) ||
				(code1 >= 65 && code1 <= 90))
			{
				result |= ConsolunaInputKeyModifierType.Shift;
			}
			else
			{
				switch(code1)
				{
					case 58:
					case 60:
					case 62:
					case 63:
					case 64:
					case 94:
					case 95:
					case 126:
						result |= ConsolunaInputKeyModifierType.Shift;
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMouseMethod																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Find and return a suitable GetMouse method.
		/// </summary>
		/// <returns>
		/// Reference to the most suitable GetMouse method found on this system.
		/// </returns>
		private static MethodInfo GetMouseMethod()
		{
			MethodInfo[] candidates = Array.Empty<MethodInfo>();
			Type ncType = typeof(Mindmagma.Curses.NCurses);
			MethodInfo result = null;

			try
			{
				candidates = ncType
					.GetMethods(BindingFlags.Public | BindingFlags.Static)
					.Where(m =>
					{
						bool nameOk = m.Name.Equals("GetMouse", StringComparison.Ordinal);
						ParameterInfo[] ps = m.GetParameters();
						bool oneByRef = (ps.Length == 1 && ps[0].ParameterType.IsByRef);
						bool retOk = (m.ReturnType == typeof(void) ||
							m.ReturnType == typeof(int));
						return nameOk && retOk && oneByRef;
					})
					.ToArray();

				if(candidates.Length > 0)
				{
					result = candidates[0];
				}
			}
			catch { }

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMouseCoords																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the current mouse coordinates.
		/// </summary>
		/// <param name="x">
		/// X position output.
		/// </param>
		/// <param name="y">
		/// Y position output.
		/// </param>
		/// <returns>
		/// Value indicating whether the operation was successful.
		/// </returns>
		private static bool GetMouseCoords(out int x, out int y)
		{
			object[] args = Array.Empty<object>();
			object boxedDefault = null;
			FieldInfo fieldX = null;
			FieldInfo fieldY = null;
			MethodInfo method = null;
			ParameterInfo parameter = null;
			PropertyInfo propertyX = null;
			PropertyInfo propertyY = null;
			bool result = false;
			object resultStruct = null;
			Type structType = null;

			x = 0;
			y = 0;

			try
			{
				method = GetMouseMethod();

				if(method != null)
				{
					parameter = method.GetParameters()[0];
					structType = parameter.ParameterType.GetElementType();

					if(structType != null)
					{
						boxedDefault = Activator.CreateInstance(structType);
						args = new object[] { boxedDefault };

						// On Invoke, wrapper fills args[0] by ref.
						if(method.ReturnType == typeof(void))
						{
							method.Invoke(null, args);
						}
						else
						{
							// Some wrappers return int; accept both shapes
							_ = method.Invoke(null, args);
						}

						resultStruct = args[0];

						if(resultStruct != null)
						{
							structType = resultStruct.GetType();

							// Prefer properties (X/Y), then fields (X/Y or x/y)
							propertyX =
								structType.GetProperty("X") ??
								structType.GetProperty("x");
							propertyY =
								structType.GetProperty("Y") ??
								structType.GetProperty("y");

							if(propertyX != null && propertyY != null)
							{
								x = Convert.ToInt32(propertyX.GetValue(resultStruct));
								y = Convert.ToInt32(propertyY.GetValue(resultStruct));
								result = true;
							}
							else
							{
								fieldX =
									structType.GetField("X") ??
									structType.GetField("x");
								fieldY =
									structType.GetField("Y") ??
									structType.GetField("y");

								if(fieldX != null && fieldY != null)
								{
									x = Convert.ToInt32(fieldX.GetValue(resultStruct));
									y = Convert.ToInt32(fieldY.GetValue(resultStruct));
									result = true;
								}
							}
						}
					}
				}
			}
			catch { }

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResolveSpecialKeys																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private static int ResolveSpecialKeys()
		{
			int fallbackMouse = 409; // KEY_MOUSE (typical)
			FieldInfo[] fields = Array.Empty<FieldInfo>();
			int foundMouse = 0;
			string[] mouseNames = new string[]
			{
				"KEY_MOUSE", "KeyMouse", "KEY__MOUSE", "Key_Mouse"
			};
			Type ncType = typeof(NCurses);
			PropertyInfo[] props = Array.Empty<PropertyInfo>();

			try
			{
				fields = ncType.GetFields(BindingFlags.Public | BindingFlags.Static);
				props =
					ncType.GetProperties(BindingFlags.Public | BindingFlags.Static);

				// Try fields first
				foreach(FieldInfo fieldItem in fields)
				{
					if(foundMouse == 0)
					{
						if(mouseNames.Any(n =>
							string.Equals(n, fieldItem.Name,
								StringComparison.OrdinalIgnoreCase)) &&
							fieldItem.FieldType == typeof(int))
						{
							foundMouse = (int)fieldItem.GetValue(null);
						}
					}
					if(foundMouse != 0)
					{
						break;
					}
				}

				// If not found yet, try properties
				if(foundMouse == 0)
				{
					foreach(PropertyInfo propertyInfoItem in props)
					{
						if(foundMouse == 0)
						{
							if(mouseNames.Any(n =>
								string.Equals(n, propertyInfoItem.Name,
									StringComparison.OrdinalIgnoreCase)) &&
								propertyInfoItem.PropertyType == typeof(int))
							{
								foundMouse = (int)propertyInfoItem.GetValue(null)!;
							}
						}
						if(foundMouse != 0)
						{
							break;
						}
					}
				}

				// Fall back if the wrapper didn't publish the constants.
				if(foundMouse == 0)
				{
					foundMouse = fallbackMouse;
				}
			}
			catch { }

			return foundMouse;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* Close																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Close the current terminal session.
		/// </summary>
		public void Close()
		{
			bool cursorRestored = false;
			IntPtr screen = mScreen;
			int setVisibleResult = 0;

			try
			{
				if(screen != IntPtr.Zero)
				{
					setVisibleResult = NCurses.SetCursor(1);
					cursorRestored = (setVisibleResult != -1);

					NCurses.EndWin();
					mScreen = IntPtr.Zero;
				}
			}
			catch { }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Initialize																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize this handler.
		/// </summary>
		public void Initialize()
		{
			uint allMouseMask = uint.MaxValue;
			bool cursorHidden = false;
			int initRows = 0;
			int initCols = 0;
			uint oldMask = 0;
			IntPtr screen = IntPtr.Zero;

			try
			{
				screen = NCurses.InitScreen(); // stdio (stdin/stdout)
				NCurses.CBreak();
				NCurses.NoEcho();
				NCurses.Keypad(screen, true);
				NCurses.NoDelay(screen, true);
				NCurses.FlushInputBuffer();

				int setVisibleResult = NCurses.SetCursor(0);
				cursorHidden = (setVisibleResult != -1);

				NCurses.MouseMask(allMouseMask, out oldMask);

				mKeyMouse = ResolveSpecialKeys();

				mScreen = screen;

				//	Issue: Console won't activate until a character is pulled.
				NCurses.GetChar(); // non-blocking due to NoDelay

				Console.WriteLine("NCurses console initialized...");
			}
			catch { }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	InputInfo																															*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="InputInfo">InputInfo</see>.
		///// </summary>
		//private List<ConsolunaInputEventArgs> mInputInfo =
		//	new List<ConsolunaInputEventArgs>();
		///// <summary>
		///// Get/Set a reference to the input info collection assigned to this
		///// instance.
		///// </summary>
		//public List<ConsolunaInputEventArgs> InputInfo
		//{
		//	get { return mInputInfo; }
		//	set { mInputInfo = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReadInput																															*
		//*-----------------------------------------------------------------------*
		private bool mWaitForNext = false;
		/// <summary>
		/// Read the current input from the console.
		/// </summary>
		/// <returns>
		/// Reference to the current console input event arguments, if an event
		/// was created. Otherwise, null.
		/// </returns>
		public ConsolunaInputEventArgs ReadInput()
		{
			int ch = -1;
			bool hasMouseCoords = false;
			int mx = 0;
			int my = 0;
			int cols = 0;
			bool sawResize = false;
			int rows = 0;
			ConsolunaInputEventArgs result = null;

			try
			{
				if(mScreen == IntPtr.Zero)
				{
					// not initialized
				}
				else
				{
					// ch = NCurses.GetChar(mScreen); // non-blocking due to NoDelay
					ch = NCurses.GetChar(); // non-blocking due to NoDelay

					if(ch == -1)
					{
						// no input
					}
					else
					{
						// Console.Write(ch.ToString());
						if(ch == mKeyMouse && mKeyMouse != 0)
						{
							hasMouseCoords = GetMouseCoords(out mx, out my);
							if(hasMouseCoords)
							{
								result = new ConsolunaInputMouseEventArgs()
								{
									MouseX = mx,
									MouseY = my
								};
							}
							else
							{
								result = new ConsolunaInputMouseEventArgs();
							}
						}
						else
						{
							if(mWaitForNext)
							{
								result = new ConsolunaInputKeyboardEventArgs()
								{
									KeyCode = GetKeyCode(27, ch),
									KeyCharacter = GetKeyCharacter(27, ch),
									KeyModifier = GetKeyModifier(27, ch)
								};
								mWaitForNext = false;
							}
							else if(ch == 27)
							{
								mWaitForNext = true;
							}
							else
							{
								//	Normal (is there such a thing in ncurses?) character.
								result = new ConsolunaInputKeyboardEventArgs()
								{
									KeyCode = GetKeyCode(ch, 0),
									KeyCharacter = GetKeyCharacter(ch, 0),
									KeyModifier = GetKeyModifier(ch, 0)
								};
							}
						}
					}
				}
			}
			catch { }

			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
