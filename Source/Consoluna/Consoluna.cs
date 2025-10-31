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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using ConsolunaLib.Events;
using ConsolunaLib.Handlers;
using ConsolunaLib.Internal;
using ConsolunaLib.Shapes;


using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	Consoluna																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Main console controller for a terminal session.
	/// </summary>
	public class Consoluna : IDisposable
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//	** SYSTEM **
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Standard output handle.
		/// </summary>
		private const int STD_OUTPUT_HANDLE = -11;

		/// <summary>
		/// Return the mode of the current console mode's input buffer.
		/// </summary>
		/// <param name="hConsoleHandle">
		/// Console input buffer handle.
		/// </param>
		/// <param name="lpMode">
		/// Current mode of the specified buffer.
		/// </param>
		/// <returns>
		/// True if the call succeeds. Otherwise, false.
		/// </returns>
		[DllImport("kernel32.dll")]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle,
			out uint lpMode);

		/// <summary>
		/// Return the handle of the specified standard device.
		/// </summary>
		/// <param name="nStdHandle">
		/// Handle ID of the standard device to retrieve.
		/// </param>
		/// <returns>
		/// The handle of the standard device, if found. Otherwise, IntPtr.Zero.
		/// </returns>
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		//	** LOCAL **
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Value indicating whether the character list events are active.
		/// </summary>
		private bool mCharacterEventsActive = true;
		/// <summary>
		/// Value indicating whether the cursor is visible.
		/// </summary>
		private bool mCursorVisible = true;
		/// <summary>
		/// Reference to the platform-specific input handler currently active.
		/// </summary>
		private IInputHandler mInputHandler = null;
		/// <summary>
		/// The last-known height of the display.
		/// </summary>
		private int mLastHeight = 0;
		/// <summary>
		/// The last-known width of the display.
		/// </summary>
		private int mLastWidth = 0;
		/// <summary>
		/// List of saved cursor positions for this instance.
		/// </summary>
		private List<PositionInfo> mSavedCursorPositions =
			new List<PositionInfo>();
		/// <summary>
		/// Cancellation token for stopping the self-hosting thread.
		/// </summary>
		private CancellationTokenSource mSelfHostingCancelToken = null;
		/// <summary>
		/// The self-hosting polling thread for this instance.
		/// </summary>
		private Thread mSelfHostingThread = null;
		/// <summary>
		/// Value indicating whether the shape list events are active.
		/// </summary>
		private bool mShapeEventsActive = true;

		//*-----------------------------------------------------------------------*
		//* GetInput																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a single event from the console, if available.
		/// </summary>
		/// <returns>
		/// Reference to the single event, if found. Otherwise, null.
		/// </returns>
		private InputEventArgs GetInput()
		{
			InputEventArgs e = null;

			if(mCharacterEventsActive)
			{
				e = mInputHandler.ReadInput();
				if(e == null)
				{
					mWidth = Console.WindowWidth;
					mHeight = Console.WindowHeight;
					if(mWidth != mLastWidth || mHeight != mLastHeight)
					{
						e = new InputResizeEventArgs()
						{
							Width = mWidth,
							Height = mHeight
						};
						mLastWidth = mWidth;
						mLastHeight = mHeight;
					}
				}
			}
			return e;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mBackColor_PropertyChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A property in the background color has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mBackColor_PropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnBackColorChanged();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mForeColor_PropertyChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A property in the foreground color has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mForeColor_PropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnForeColorChanged();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mScreenCharacters_CollectionChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of the screen characters collection have changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console character collection change event arguments.
		/// </param>
		private void mScreenCharacters_CollectionChanged(object sender,
			CollectionChangeEventArgs<CharacterItem> e)
		{
			OnScreenCharacterCollectionChanged(
				new CharacterCollectionEventArgs(e));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	mScreenCharacters_ItemPropertyChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of a specific item have been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mScreenCharacters_ItemPropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnScreenCharacterItemChanged(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mScreenShapes_CollectionChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of the screen shapes collection have changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console shape collection change event arguments.
		/// </param>
		private void mScreenShapes_CollectionChanged(object sender,
			CollectionChangeEventArgs<ShapeBase> e)
		{
			OnScreenShapeCollectionChanged(
				new ShapeCollectionEventArgs(e));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mScreenShapes_ItemPropertyChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of a single screen shape have changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mScreenShapes_ItemPropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnScreenShapeItemChanged(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PollingLoop																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Internal polling loop.
		/// </summary>
		/// <param name="cancellationToken">
		/// Reference to the thread cancellation token that can be used to stop
		/// the thread, if necessary.
		/// </param>
		private void PollingLoop(CancellationTokenSource cancellationToken)
		{
			InputEventArgs e = null;

			while(true)
			{
				e = WaitForInput();
				if(e != null)
				{
					OnInputReceived(e);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemapDisplay																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remap the display to new dimensions.
		/// </summary>
		/// <param name="oldWidth">
		/// The old width.
		/// </param>
		/// <param name="oldHeight">
		/// The old height.
		/// </param>
		/// <param name="newWidth">
		/// The new width.
		/// </param>
		/// <param name="newHeight">
		/// The new height.
		/// </param>
		private void RemapDisplay(int oldWidth, int oldHeight, int newWidth,
			int newHeight)
		{
			bool bCharacterEvents = mCharacterEventsActive;
			int colIndex = 0;
			int minHeight = 0;
			int minWidth = 0;
			List<CharacterItem> newCharacterDisplay = null;
			List<CharacterItem> newScreenDisplay = null;
			int rowIndex = 0;

			if(oldWidth != newWidth || oldHeight != newHeight)
			{
				mCharacterEventsActive = false;
				newCharacterDisplay =
				Enumerable.Range(0, newHeight * newWidth)
					.Select(i => new CharacterItem(mForeColor, mBackColor)
					{
						Dirty = false
					})
					.ToList();
				newScreenDisplay =
				Enumerable.Range(0, newHeight * newWidth)
					.Select(i => new CharacterItem(mForeColor, mBackColor)
					{
						Dirty = false
					})
					.ToList();
				minWidth = Math.Min(oldWidth, newWidth);
				minHeight = Math.Min(oldHeight, newHeight);
				for(rowIndex = 0; rowIndex < minHeight; rowIndex ++)
				{
					for(colIndex = 0; colIndex < minWidth; colIndex ++)
					{
						newCharacterDisplay[rowIndex * newWidth + colIndex] =
							mCharacters[rowIndex * oldWidth + colIndex];
						newScreenDisplay[rowIndex * newWidth + colIndex] =
							mPhysicalBuffer[rowIndex * oldWidth + colIndex];
					}
				}
				mCharacters.SafeClear();
				mCharacters.SafeAddRange(newCharacterDisplay);
				mPhysicalBuffer.SafeClear();
				mPhysicalBuffer.SafeAddRange(newScreenDisplay);
				mCharacterEventsActive = bCharacterEvents;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartThread																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Start the self-hosting thread.
		/// </summary>
		private void StartThread()
		{
			if(mSelfHostingThread == null && mSelfHostingCancelToken == null)
			{
				mSelfHostingCancelToken = new CancellationTokenSource();
				mSelfHostingThread = new Thread(
					() => PollingLoop(mSelfHostingCancelToken));
				mSelfHostingThread.Start();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StopThread																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Stop the self-hosting thread.
		/// </summary>
		private void StopThread()
		{
			if(mSelfHostingThread?.IsAlive == true &&
				mSelfHostingCancelToken != null)
			{
				mSelfHostingCancelToken.Cancel();
				mSelfHostingThread.Join();
				mSelfHostingCancelToken.Dispose();
				mSelfHostingCancelToken = null;
				mSelfHostingThread = null;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WaitForInput																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Wait for input from the console and return the retrieved event.
		/// </summary>
		/// <returns>
		/// Reference to the generated input event.
		/// </returns>
		private InputEventArgs WaitForInput()
		{
			InputEventArgs e = null;

			while(e == null)
			{
				e = GetInput();
				if(e == null)
				{
					Thread.Sleep(mPollInterval);
				}
			}
			return e;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnBackColorChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the BackColorChanged event when the default background color
		/// has changed.
		/// </summary>
		protected virtual void OnBackColorChanged()
		{
			BackColorChanged?.Invoke(this, new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnForeColorChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ForeColorChanged event when the default foreground color
		/// has changed.
		/// </summary>
		protected virtual void OnForeColorChanged()
		{
			ForeColorChanged?.Invoke(this, new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnKeyboardInputReceived																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the KeyboardInputReceived event when input has been received
		/// from the keyboard.
		/// </summary>
		/// <param name="e">
		/// Console input keyboard event arguments.
		/// </param>
		protected virtual void OnKeyboardInputReceived(
			KeyboardInputEventArgs e)
		{
			KeyboardInputReceived?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnInputReceived																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the InputReceive event when any input has been received from the
		/// terminal, including keyboard, mouse, or terminal window resize.
		/// </summary>
		/// <param name="e">
		/// Console input event arguments.
		/// </param>
		protected virtual void OnInputReceived(InputEventArgs e)
		{
			InputReceived?.Invoke(this, e);
			if(e != null)
			{
				if(e is KeyboardInputEventArgs keyEvent)
				{
					OnKeyboardInputReceived(keyEvent);
				}
				else if(e is MouseInputEventArgs mouseEvent)
				{
					OnMouseInputReceived(mouseEvent);
				}
				else if(e is InputResizeEventArgs resizeEvent)
				{
					OnTerminalResized(resizeEvent);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseInputReceived																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseInputReceived event when input has been received from
		/// the mouse.
		/// </summary>
		/// <param name="e">
		/// Console input mouse event arguments.
		/// </param>
		protected virtual void OnMouseInputReceived(MouseInputEventArgs e)
		{
			MouseInputReceived?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnScreenCharacterCollectionChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// One or more objects in the screen characters collection have changed.
		/// </summary>
		/// <param name="e">
		/// Console character collection event arguments.
		/// </param>
		protected virtual void OnScreenCharacterCollectionChanged(
			CharacterCollectionEventArgs e)
		{
			if(mCharacterEventsActive)
			{
				ScreenCharacterCollectionChanged?.Invoke(this, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnScreenCharacterItemChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of a character item have changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		protected virtual void OnScreenCharacterItemChanged(object sender,
			PropertyChangeEventArgs e)
		{
			if(mCharacterEventsActive)
			{
				ScreenCharacterItemChanged?.Invoke(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnScreenShapeCollectionChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ScreenShapeCollectionChanged event when the contents of the
		/// ScreenShapes collection have changed.
		/// </summary>
		/// <param name="e">
		/// Console shape collection event arguments.
		/// </param>
		protected virtual void OnScreenShapeCollectionChanged(
			ShapeCollectionEventArgs e)
		{
			if(mShapeEventsActive)
			{
				ScreenShapeCollectionChanged?.Invoke(this, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnScreenShapeItemChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ScreenShapeItemChanged event when the contents of a screen
		/// shape item have been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		protected virtual void OnScreenShapeItemChanged(object sender,
			PropertyChangeEventArgs e)
		{
			if(mShapeEventsActive)
			{
				ScreenShapeItemChanged?.Invoke(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnTerminalResized																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the TerminalResized event when the terminal has been resized.
		/// </summary>
		/// <param name="e">
		/// Console input resize event arguments.
		/// </param>
		protected virtual void OnTerminalResized(InputResizeEventArgs e)
		{
			TerminalResized?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the ConsolePlus item.
		/// </summary>
		public Consoluna()
		{
			//	Set the default styles.
			mStyles = new ScreenStyleCollection();
			mStyles.AddRange(new ScreenStyleItem[]
			{
				new ScreenStyleItem("ScreenColor",
					foreColor: new ColorInfo("#b0aedd"),
					backColor: new ColorInfo("#0b0938")),

				new ScreenStyleItem("ButtonColor",
					foreColor: new ColorInfo("#003300"),
					backColor: new ColorInfo("#01af00")),
				new ScreenStyleItem("ButtonColorDefault",
					foreColor: new ColorInfo("#5cffb1"),
					backColor: new ColorInfo("#01af00")),
				new ScreenStyleItem("ButtonShortcutColor",
					foreColor: new ColorInfo("#333300"),
					backColor: new ColorInfo("#01af00")),

				new ScreenStyleItem("BoxColor",
					foreColor: new ColorInfo("#010101"),
					backColor: new ColorInfo("#b4b3b1")),
				new ScreenStyleItem("BoxBorderColor",
					foreColor: new ColorInfo("#fffeff"),
					backColor: new ColorInfo("#b4b3b1")),
				new ScreenStyleItem("BoxListBoxColor",
					foreColor: new ColorInfo("#002427"),
					backColor: new ColorInfo("#03b1ba")),
				new ScreenStyleItem("BoxListBoxHighlightColor",
					foreColor: new ColorInfo("#eeffe1"),
					backColor: new ColorInfo("#01ae04")),
				new ScreenStyleItem("BoxTextBoxColor",
					foreColor: new ColorInfo("#f2fcff"),
					backColor: new ColorInfo("#0001ab")),

				new ScreenStyleItem("LabelColor",
					foreColor: new ColorInfo("#00b7fa")),

				new ScreenStyleItem("MenuColor",
					foreColor: new ColorInfo("#0b0708"),
					backColor: new ColorInfo("#b4b3b1")),
				new ScreenStyleItem("MenuHighlightColor",
					backColor: new ColorInfo("#01af00")),
				new ScreenStyleItem("MenuPanelBorderColor",
					foreColor: new ColorInfo("#010101"),
					backColor: new ColorInfo("#b4b3b1")),
				new ScreenStyleItem("MenuShortcutColor",
					foreColor: new ColorInfo("#912120")),

				new ScreenStyleItem("ShortcutColor",
					foreColor: new ColorInfo("#912120")),

				new ScreenStyleItem("ScrollBarColor",
					foreColor: new ColorInfo("#2a90c9"),
					backColor: new ColorInfo("#02138f")),

				new ScreenStyleItem("TextColor",
					foreColor: new ColorInfo("#f2fcff"),
					backColor: new ColorInfo("#0001ab")),

				new ScreenStyleItem("WindowColor",
					foreColor: new ColorInfo("#052792"),
					backColor: new ColorInfo("#04b1b0")),
				new ScreenStyleItem("WindowBorderColor",
					foreColor: new ColorInfo("#aaffff"),
					backColor: new ColorInfo("#04b1b0")),
				new ScreenStyleItem("WindowButtonColor",
					foreColor: new ColorInfo("#48f988"),
					backColor: new ColorInfo("#04b1b0")),
				new ScreenStyleItem("WindowListBoxColor",
					foreColor: new ColorInfo("#e7f1ff"),
					backColor: new ColorInfo("#0200ae")),
				new ScreenStyleItem("WindowListBoxHeaderColor",
					foreColor: new ColorInfo("#052792"),
					backColor: new ColorInfo("#04b1b0")),
				new ScreenStyleItem("WindowListBoxHighlightColor",
					foreColor: new ColorInfo("#ffc398"),
					backColor: new ColorInfo("#ad0100")),
				new ScreenStyleItem("WindowTitleColor",
					foreColor: new ColorInfo("#e7f1ff"),
					backColor: new ColorInfo("#04b1b0"))

			});


			//	TODO: !1 - Stopped here...
			//	Currently using Unicode box-drawing.
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			//// Set the console's output encoding to Code Page 437
			//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			//Console.OutputEncoding = Encoding.GetEncoding(437);

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				mInputHandler = new InputHandlerWindows();
			}
			else
			{
				mInputHandler = new InputHandlerNCurses();
			}
			if(mInputHandler != null)
			{
				mInputHandler.Initialize();
			}

			mForeColor = new ColorInfo()
			{
				Red = 0x7f,
				Green = 0x7f,
				Blue = 0x7f
			};
			mForeColor.PropertyChanged += mForeColor_PropertyChanged;

			mBackColor = new ColorInfo();
			mBackColor.PropertyChanged += mBackColor_PropertyChanged;

			mCharacters.CollectionChanged +=
				mScreenCharacters_CollectionChanged;
			mCharacters.ItemPropertyChanged +=
				mScreenCharacters_ItemPropertyChanged;

			mShapes = new SafeConsolunaShapeCollection();
			mShapes.CollectionChanged +=
				mScreenShapes_CollectionChanged;
			mShapes.ItemPropertyChanged +=
				mScreenShapes_ItemPropertyChanged;


			Console.CancelKeyPress += (_, __) =>
			{
				// Ensure cleanup if user presses Ctrl+C
				mInputHandler.Close();
				Console.WriteLine("Closed...");
				Thread.Sleep(500);
				Environment.Exit(0);
			};

			Update();

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Destructor																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Instance is being finalized without call to dispose.
		/// </summary>
		~Consoluna()
		{
			Dispose(false);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AdvanceCursor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Advance the cursor linearly the specified number of places.
		/// </summary>
		/// <param name="places">
		/// Number of places to advance the cursor.
		/// </param>
		public void AdvanceCursor(int spaces)
		{
			mCursorPosition.X += spaces;
			EnsureLegal(mWidth, mHeight, mCursorPosition);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiBackColorStart																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for starting the specified background
		/// color.
		/// </summary>
		/// <param name="color">
		/// Reference to the color information to print.
		/// </param>
		/// <returns>
		/// The ANSI escape code representing the specified background color.
		/// </returns>
		public static string AnsiBackColorStart(ColorInfo color)
		{
			StringBuilder builder = new StringBuilder();

			if(color != null)
			{
				builder.Append("\x1b[48;2;");
				builder.Append($"{color.Red};");
				builder.Append($"{color.Green};");
				builder.Append($"{color.Blue}");
				builder.Append('m');
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiCursorHide																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for hiding the cursor.
		/// </summary>
		/// <returns>
		/// ANSI escape code for hiding the cursor.
		/// </returns>
		public static string AnsiCursorHide()
		{
			return "\x1b[?25l";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiCursorShow																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for showing the cursor.
		/// </summary>
		/// <returns>
		/// ANSI escape code for showing the cursor.
		/// </returns>
		public static string AnsiCursorShow()
		{
			return "\x1b[?25h";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiEraseInDisplay																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code that will run the Erase in Display (ED)
		/// function on the terminal.
		/// </summary>
		/// <returns>
		/// ANSI Erase in Display (ED) escape code.
		/// </returns>
		public static string AnsiEraseInDisplay()
		{
			return "\x1b[2J";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiForeColorStart																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for starting the specified foreground
		/// color.
		/// </summary>
		/// <param name="color">
		/// Reference to the color information to print.
		/// </param>
		/// <returns>
		/// The ANSI escape code representing the specified background color.
		/// </returns>
		public static string AnsiForeColorStart(ColorInfo color)
		{
			StringBuilder builder = new StringBuilder();

			if(color != null)
			{
				builder.Append("\x1b[38;2;");
				builder.Append($"{color.Red};");
				builder.Append($"{color.Green};");
				builder.Append($"{color.Blue}");
				builder.Append('m');
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiResetStyles																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for resetting all colors and styles.
		/// </summary>
		/// <returns>
		/// The ANSI escape code for resetting all colors and styles.
		/// </returns>
		public static string AnsiResetStyles()
		{
			return "\x1b[0m";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AnsiStyleStart																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ANSI escape code for starting the specified character style.
		/// </summary>
		/// <param name="style">
		/// The character style to apply.
		/// </param>
		/// <returns>
		/// The ANSI escape code representing the provided style.
		/// </returns>
		public static string AnsiStyleStart(CharacterStyleType style)
		{
			StringBuilder builder = new StringBuilder();

			if(style != CharacterStyleType.None)
			{
				if((style & CharacterStyleType.Blink) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[5m");
				}
				if((style & CharacterStyleType.Bold) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[1m");
				}
				if((style & CharacterStyleType.Faint) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[2m");
				}
				if((style & CharacterStyleType.Hidden) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[8m");
				}
				if((style & CharacterStyleType.Inverse) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[7m");
				}
				if((style & CharacterStyleType.Italic) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[3m");
				}
				if((style & CharacterStyleType.Strike) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[9m");
				}
				if((style & CharacterStyleType.Underline) !=
					CharacterStyleType.None)
				{
					builder.Append("\x1b[4m");
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AssignColorFromStyle																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Assign a color item's foreground and background colors from the
		/// named style.
		/// </summary>
		/// <param name="item">
		/// Reference to the item whose colors will be stylized.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to retrieve.
		/// </param>
		/// <returns>
		/// Reference to the specified screen style, if found. Otherwise,
		/// null.
		/// </returns>
		public ScreenStyleItem AssignColorFromStyle(IForeBack item,
			string styleName)
		{
			ScreenStyleItem result = null;
			string styleNameLower = "";

			if(item != null && styleName?.Length > 0)
			{
				styleNameLower = styleName.ToLower();
				result = mStyles.FirstOrDefault(x =>
					x.Name.ToLower() == styleNameLower);
				if(result != null)
				{
					if(result.BackColor != null)
					{
						ColorInfo.TransferValues(result.BackColor, item.BackColor);
					}
					if(result.ForeColor != null)
					{
						ColorInfo.TransferValues(result.ForeColor, item.ForeColor);
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BackColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BackColor">BackColor</see>.
		/// </summary>
		private ColorInfo mBackColor = null;
		/// <summary>
		/// Get/Set a reference to the default background color for this screen.
		/// </summary>
		public ColorInfo BackColor
		{
			get { return mBackColor; }
			set
			{
				bool bChanged = (mBackColor != value);
				if(bChanged && mBackColor != null)
				{
					mBackColor.PropertyChanged -= mBackColor_PropertyChanged;
				}
				mBackColor = value;
				if(bChanged)
				{
					if(mBackColor != null)
					{
						mBackColor.PropertyChanged += mBackColor_PropertyChanged;
					}
					mStyles.SetProperty("ScreenColor", "BackColor", mBackColor);
					OnBackColorChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* BackColorChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the default background color has changed.
		/// </summary>
		public event EventHandler<EventArgs> BackColorChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Backspace																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply backspace at the current cursor position.
		/// </summary>
		/// <param name="count">
		/// The count of times to apply the pattern.
		/// </param>
		public void Backspace(int count)
		{
			CharacterItem character = null;
			List<CharacterItem> characters = mCharacters;
			int endIndex = 0;
			int index = 0;
			int startIndex = 0;

			EnsureLegal(mWidth, mHeight, mCursorPosition);
			startIndex = mCursorPosition.Y * mWidth + mCursorPosition.X - 1;
			endIndex = startIndex - count;
			for(index = startIndex; index > -1 && index > endIndex; index --)
			{
				character = characters[index];
				character.Symbol = char.MinValue;
				mCursorPosition.X--;
			}
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			SetTerminalPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CarriageReturn																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply a carriage return at the current cursor position.
		/// </summary>
		/// <param name="count">
		/// The count of times to apply the pattern.
		/// </param>
		public void CarriageReturn()
		{
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			mCursorPosition.X = 0;
			SetTerminalPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Characters																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Characters">Characters</see>.
		/// </summary>
		private SafeCharacterCollection mCharacters =
			new SafeCharacterCollection();
		/// <summary>
		/// Get a reference to the collection of characters in the buffer.
		/// </summary>
		public SafeCharacterCollection Characters
		{
			get { return mCharacters; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ClearCharacterWindow																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the entire character window to the specified default values.
		/// </summary>
		/// <param name="characterWindow">
		/// Reference to the cartesian-style character window to dirty.
		/// </param>
		/// <param name="foreColor">
		/// Optional foreground color.
		/// </param>
		/// <param name="backColor">
		/// Optional background color.
		/// </param>
		public void ClearCharacterWindow(CharacterItem[,] characterWindow,
			ColorInfo foreColor = null, ColorInfo backColor = null)
		{
			if(characterWindow?.Length > 0)
			{
				foreach(CharacterItem characterItem in characterWindow)
				{
					if(foreColor != null)
					{
						characterItem.ForeColor = foreColor;
					}
					else
					{
						characterItem.ForeColor = mForeColor;
					}
					if(backColor != null)
					{
						characterItem.BackColor = backColor;
					}
					else
					{
						characterItem.BackColor = mBackColor;
					}
					characterItem.Shadowed = false;
					characterItem.Symbol = '\0';
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Clear a portion of the characters in the character window to the
		/// specified default values.
		/// </summary>
		/// <param name="characterWindow">
		/// Reference to the cartesian-style character window to dirty.
		/// </param>
		/// <param name="column">
		/// Starting column index at which to begin clearing.
		/// </param>
		/// <param name="row">
		/// Starting row index at which to begin clearing.
		/// </param>
		/// <param name="width">
		/// Width of columns to clear.
		/// </param>
		/// <param name="height">
		/// Height of rows to clear.
		/// </param>
		/// <param name="foreColor">
		/// Optional foreground color.
		/// </param>
		/// <param name="backColor">
		/// Optional background color.
		/// </param>
		public void ClearCharacterWindow(CharacterItem[,] characterWindow,
			int column, int row, int width, int height,
			ColorInfo foreColor = null, ColorInfo backColor = null)
		{
			CharacterItem character = null;
			int colEnd = 0;
			int colIndex = 0;
			int colStart = 0;
			int rowEnd = 0;
			int rowIndex = 0;
			int rowStart = 0;

			if(characterWindow?.Length > 0)
			{
				colEnd = Math.Min(column + width, characterWindow.GetLength(0));
				rowEnd = Math.Min(row + height, characterWindow.GetLength(1));
				for(rowIndex = row; rowIndex < rowEnd; rowIndex ++)
				{
					if(rowIndex > -1)
					{
						for(colIndex = column; colIndex < colEnd; colIndex ++)
						{
							if(colIndex > -1)
							{
								character = characterWindow[colIndex, rowIndex];
								if(foreColor != null)
								{
									character.ForeColor = foreColor;
								}
								else
								{
									character.ForeColor = mForeColor;
								}
								if(backColor != null)
								{
									character.BackColor = backColor;
								}
								else
								{
									character.BackColor = mBackColor;
								}
								character.Symbol = '\0';
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ClearScreen																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the screen.
		/// </summary>
		public void ClearScreen()
		{
			mCharacters.Clear();
			Update();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CursorPosition																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="CursorPosition">CursorPosition</see>.
		/// </summary>
		private PositionInfo mCursorPosition = new PositionInfo();
		/// <summary>
		/// Get a reference to the current cursor position.
		/// </summary>
		public PositionInfo CursorPosition
		{
			get { return mCursorPosition; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Dispose																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Value indicating whether the instance has already been disposed.
		/// </summary>
		private bool mbDisposed = false;
		/// <summary>
		/// Dispose the item and its resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// Remove us from the finalization queue to prevent subsequent
			// finalizations.
			GC.SuppressFinalize(this);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Dispose the collection and its resources.
		/// </summary>
		/// <param name="disposing">
		/// Value indicating whether Dispose has been called from User Code (true),
		/// or from the internal finalizer (false).
		/// </param>
		public virtual void Dispose(bool disposing)
		{
			if(!mbDisposed)
			{
				// If the item hasn't already been disposed, then do so now.
				if(mInputHandler != null)
				{
					mInputHandler.Close();
					mInputHandler = null;
				}
				mbDisposed = true;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ForeColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ForeColor">ForeColor</see>.
		/// </summary>
		private ColorInfo mForeColor = null;
		/// <summary>
		/// Get/Set a reference to the default foreground color for this screen.
		/// </summary>
		public ColorInfo ForeColor
		{
			get { return mForeColor; }
			set
			{
				bool bChanged = (mForeColor != value);
				if(mForeColor != null)
				{
					mForeColor.PropertyChanged -= mForeColor_PropertyChanged;
				}
				mForeColor = value;
				if(bChanged)
				{
					if(mForeColor != null)
					{
						mForeColor.PropertyChanged += mForeColor_PropertyChanged;
					}
					mStyles.SetProperty("ScreenColor", "ForeColor", mForeColor);
					OnForeColorChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ForeColorChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the default foreground color has changed.
		/// </summary>
		public event EventHandler<EventArgs> ForeColorChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCharactersInArea																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of characters from the indicated cartesian area of the
		/// screen buffer.
		/// </summary>
		/// <param name="columnIndex">
		/// Starting column index.
		/// </param>
		/// <param name="rowIndex">
		/// Starting row index.
		/// </param>
		/// <param name="width">
		/// Width of area, in characters.
		/// </param>
		/// <param name="height">
		/// Height of area, in characters.
		/// </param>
		/// <returns>
		/// Reference to a 2-dimensional, 0-based array of characters found in
		/// the specified source area of the screen buffer. Any characters outside
		/// the boundaries of the logical screen buffer space are filled with
		/// dummy elements. If either width or height are less than 1, an empty
		/// array is returned.
		/// </returns>
		/// <remarks>
		/// Column and row indices should have legal values prior to calling this
		/// method. Any indicators that fall outside the logical bounds of the
		/// buffer's cartesian space will be filled with dummy elements.
		/// </remarks>
		public CharacterItem[,] GetCharactersInArea(int column,
			int row, int width, int height)
		{
			int bufferIndex = 0;
			int colIndex = 0;
			CharacterItem[,] result = null;
			int rowIndex = 0;
			int xIndex = 0;
			int xEnd = 0;
			int xStart = column;
			int yIndex = 0;
			int yEnd = 0;
			int yStart = row;

			if(width > 0 && height > 0)
			{
				xEnd = xStart + width;
				yEnd = yStart + height;
				result = new CharacterItem[width, height];
				try
				{
					for(yIndex = yStart, rowIndex = 0;
						yIndex < yEnd;
						yIndex++, rowIndex++)
					{
						for(xIndex = xStart, colIndex = 0;
							xIndex < xEnd;
							xIndex++, colIndex++)
						{
							if(xIndex >= 0 && xIndex < mWidth &&
								yIndex >= 0 && yIndex < mHeight)
							{
								bufferIndex = GetBufferIndex(mWidth, mHeight, xIndex, yIndex);
								result[colIndex, rowIndex] = mCharacters[bufferIndex];
							}
							else
							{
								result[colIndex, rowIndex] = new CharacterItem();
							}
						}
					}
				}
				catch
				{
					result = null;
				}
			}

			if(result == null)
			{
				result = new CharacterItem[0, 0];
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Height																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Height">Height</see>.
		/// </summary>
		private int mHeight = 0;
		/// <summary>
		/// Get/Set the height of the terminal.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
			set
			{
				mHeight = value;
				Console.WindowHeight = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HideCursor																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Hide the cursor at its current location.
		/// </summary>
		public void HideCursor()
		{
			Console.Write(AnsiCursorHide());
			mCursorVisible = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InputMode																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="InputMode">InputMode</see>.
		/// </summary>
		private InputMode mInputMode = InputMode.DirectPoll;
		/// <summary>
		/// Get/Set the active input mode for this instance.
		/// </summary>
		public InputMode InputMode
		{
			get { return mInputMode; }
			set
			{
				mInputMode = value;
				if(mInputMode == InputMode.EventDriven)
				{
					StartThread();
				}
				else
				{
					StopThread();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InputReceived																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when any input has been received from the terminal, including
		/// keyboard, mouse, or terminal window resize.
		/// </summary>
		public event EventHandler<InputEventArgs> InputReceived;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InWindow																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the provided coordinate is within the
		/// physical window.
		/// </summary>
		/// <param name="colIndex">
		/// The column index to check.
		/// </param>
		/// <param name="rowIndex">
		/// The row index to check.
		/// </param>
		/// <returns>
		/// True if the provided coordinate is within the physical terminal window.
		/// Otherwise, false.
		/// </returns>
		public bool InWindow(int colIndex, int rowIndex)
		{
			return (colIndex > -1 && colIndex < Console.WindowWidth &&
				rowIndex > -1 && rowIndex < Console.WindowHeight);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* KeyboardInputReceived																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when input has been received from the keyboard.
		/// </summary>
		public event EventHandler<KeyboardInputEventArgs>
			KeyboardInputReceived;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LineFeed																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply a line feed at the current cursor position.
		/// </summary>
		/// <param name="count">
		/// The count of times to apply the pattern.
		/// </param>
		public void LineFeed(int count)
		{
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			mCursorPosition.Y += count;
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			SetTerminalPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MouseInputReceived																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when input has been received from the mouse.
		/// </summary>
		public event EventHandler<MouseInputEventArgs> MouseInputReceived;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PhysicalBuffer																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PhysicalBuffer">PhysicalBuffer</see>.
		/// </summary>
		private SafeCharacterCollection mPhysicalBuffer =
			new SafeCharacterCollection();
		/// <summary>
		/// Get a reference to the collection of characters in the final screen
		/// buffer.
		/// </summary>
		internal SafeCharacterCollection PhysicalBuffer
		{
			get { return mPhysicalBuffer; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PollInterval																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PollInterval">PollInterval</see>.
		/// </summary>
		private int mPollInterval = 100;
		/// <summary>
		/// Get/Set the timer interval at which the input state is polled during
		/// blocking or event-driven operations.
		/// </summary>
		public int PollInterval
		{
			get { return mPollInterval; }
			set { mPollInterval = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Read																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of reading a single event from input.
		/// </summary>
		/// <returns>
		/// Reference to a InputEventArgs representing the read event,
		/// if found. Otherwise, null.
		/// </returns>
		public InputEventArgs Read()
		{
			InputEventArgs result = GetInput();
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RestoreCursorPosition																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Restore the most recently saved cursor position.
		/// </summary>
		public void RestoreCursorPosition()
		{
			int index = 0;

			index = mSavedCursorPositions.Count - 1;
			if(index > -1)
			{
				PositionInfo.TransferValues(
					mSavedCursorPositions[index], mCursorPosition);
				mSavedCursorPositions.RemoveAt(index);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveCursorPosition																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save the current cursor position.
		/// </summary>
		public void SaveCursorPosition()
		{
			mSavedCursorPositions.Add(new PositionInfo(mCursorPosition));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenCharacterCollectionChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of the ScreenCharacters collection have
		/// changed.
		/// </summary>
		public event EventHandler<CharacterCollectionEventArgs>
			ScreenCharacterCollectionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenCharacterItemChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of a Screen Symbol have changed.
		/// </summary>
		public event EventHandler<PropertyChangeEventArgs>
			ScreenCharacterItemChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenShapeCollectionChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of the screen shapes collection have changed.
		/// </summary>
		public event EventHandler<ShapeCollectionEventArgs>
			ScreenShapeCollectionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenShapeItemChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when a single value of a screen shape has been changed.
		/// </summary>
		public event EventHandler<PropertyChangeEventArgs>
			ScreenShapeItemChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetBackColor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the background color for all characters in the window.
		/// </summary>
		/// <param name="characterWindow">
		/// Reference to the cartesian-style character window to update.
		/// </param>
		/// <param name="foreColor">
		/// Reference to the backcolor to place. If null, the local background
		/// color is used.
		/// </param>
		public void SetBackColor(CharacterItem[,] characterWindow,
			ColorInfo backColor)
		{
			if(characterWindow?.Length > 0)
			{
				if(backColor != null)
				{
					foreach(CharacterItem characterItem in characterWindow)
					{
						characterItem.BackColor = backColor;
					}
				}
				else
				{
					foreach(CharacterItem characterItem in characterWindow)
					{
						characterItem.BackColor = mBackColor;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetCursorPosition																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the current logical cursor position.
		/// </summary>
		/// <param name="x">
		/// 0-based horizontal position.
		/// </param>
		/// <param name="y">
		/// 0-based vertical position.
		/// </param>
		public void SetCursorPosition(int x, int y)
		{
			mCursorPosition.X = x;
			mCursorPosition.Y = y;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetCursorShape																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the shape of the cursor.
		/// </summary>
		/// <param name="cursorShape">
		/// The shape of the cursor. If None, the cursor will be hidden.
		/// </param>
		public void SetCursorShape(CursorShapeEnum cursorShape)
		{
			switch(cursorShape)
			{
				case CursorShapeEnum.Bar:
					Console.Write("\x1b[5q");
					break;
				case CursorShapeEnum.BlinkingBar:
					Console.Write("\x1b[6q");
					break;
				case CursorShapeEnum.BlinkingBlock:
					Console.Write("\x1b[2q");
					break;
				case CursorShapeEnum.BlinkingUnderline:
					Console.Write("\x1b[4q");
					break;
				case CursorShapeEnum.Block:
					Console.Write("\x1b[1q");
					break;
				case CursorShapeEnum.None:
					HideCursor();
					break;
				case CursorShapeEnum.Underline:
					Console.Write("\x1b[3q");
					break;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetDirty																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set all of the characters of a dedicated character window to dirty.
		/// </summary>
		/// <param name="characterWindow">
		/// Reference to the cartesian-style character window to dirty.
		/// </param>
		public void SetDirty(CharacterItem[,] characterWindow)
		{
			if(characterWindow?.Length > 0)
			{
				foreach(CharacterItem characterItem in characterWindow)
				{
					characterItem.Dirty = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetForeColor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the foreground color for all characters in the window.
		/// </summary>
		/// <param name="characterWindow">
		/// Reference to the cartesian-style character window to update.
		/// </param>
		/// <param name="foreColor">
		/// Reference to the forecolor to place. If null, the local foreground
		/// color is used.
		/// </param>
		public void SetForeColor(CharacterItem[,] characterWindow,
			ColorInfo foreColor)
		{
			if(characterWindow?.Length > 0)
			{
				if(foreColor != null)
				{
					foreach(CharacterItem characterItem in characterWindow)
					{
						characterItem.ForeColor = foreColor;
					}
				}
				else
				{
					foreach(CharacterItem characterItem in characterWindow)
					{
						characterItem.ForeColor = mForeColor;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetTerminalPosition																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Safely set the current cursor position on the physical terminal.
		/// </summary>
		/// <param name="colIndex">
		/// The index of the column to activate.
		/// </param>
		/// <param name="rowIndex">
		/// The index of the row to activate.
		/// </param>
		/// <returns>
		/// True if the position was set. Otherwise, false.
		/// </returns>
		public bool SetTerminalPosition(int colIndex, int rowIndex)
		{
			bool result = false;

			if(InWindow(colIndex, rowIndex))
			{
				try
				{
					Console.SetCursorPosition(colIndex, rowIndex);
					result = true;
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetUnshaded																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set all of the characters in the supplied list to unshaded.
		/// </summary>
		/// <param name="characters">
		/// Reference to a collection of characters to set unshaded.
		/// </param>
		public static void SetUnshaded(List<CharacterItem> characters)
		{
			if(characters?.Count > 0)
			{
				foreach(CharacterItem characterItem in characters)
				{
					if(characterItem.Shadowed)
					{
						characterItem.Shadowed = false;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Shapes																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Shapes">Shapes</see>.
		/// </summary>
		private SafeConsolunaShapeCollection mShapes = null;
		/// <summary>
		/// Get a reference to the collection of screen shapes in this instance.
		/// </summary>
		public SafeConsolunaShapeCollection Shapes
		{
			get { return mShapes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShowCursor																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Show the cursor at its current location.
		/// </summary>
		public void ShowCursor()
		{
			Console.Write(AnsiCursorShow());
			mCursorVisible = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Styles																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Styles">Styles</see>.
		/// </summary>
		private ScreenStyleCollection mStyles = null;
		/// <summary>
		/// Get a reference to the collection of styles associated with this screen
		/// buffer.
		/// </summary>
		public ScreenStyleCollection Styles
		{
			get { return mStyles; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Tab																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply tab at the current cursor position.
		/// </summary>
		/// <param name="count">
		/// The count of times to apply the pattern.
		/// </param>
		public void Tab(int count)
		{
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			mCursorPosition.X += (count * 4);
			EnsureLegal(mWidth, mHeight, mCursorPosition);
			SetTerminalPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TerminalResized																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the terminal has been resized.
		/// </summary>
		public event EventHandler<InputResizeEventArgs> TerminalResized;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Update																																*
		//*-----------------------------------------------------------------------*
		private int mLastUpdateHeight = 0;
		private int mLastUpdateWidth = 0;
		/// <summary>
		/// Update the screen, drawing all of the characters out to the terminal.
		/// </summary>
		public void Update()
		{
			bool bCharacterEventsActive = mCharacterEventsActive;
			bool bCursorVisible = mCursorVisible;
			bool bFound = false;
			bool bFullRefresh = false;
			StringBuilder builder = new StringBuilder();
			CharacterItem character = null;
			int colCount = 0;
			int colIndex = 0;
			int count = 0;
			List<CharacterItem> dirties = null;
			int endIndex = 0;
			//int height = Console.WindowHeight;
			int index = 0;
			ColorInfo lastBackColor = null;
			ColorInfo lastForeColor = null;
			CharacterStyleType lastStyle =
				CharacterStyleType.None;
			int lastX = 0;
			int min = 0;
			int row = 0;
			//List<CharacterItem> rowContent = null;
			int rowCount = 0;
			int rowIndex = 0;
			List<int> rows = null;
			int startX = 0;
			CharacterItem target = null;
			//int width = Console.WindowWidth;

			lock(ResourceLock)
			{
				mWidth = Console.WindowWidth;
				mHeight = Console.WindowHeight;
				if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					//	There is a bug in System.Console where buffer maintenance
					//	is only available in Windows.
					if(Console.WindowWidth != Console.BufferWidth)
					{
						SetTerminalPosition(0, 0);
						Console.BufferWidth = Console.WindowWidth;
					}
					if(Console.WindowHeight != Console.BufferHeight)
					{
						SetTerminalPosition(0, 0);
						Console.BufferHeight = Console.WindowHeight;
					}
				}

				if(bCursorVisible)
				{
					HideCursor();
				}
				mCharacterEventsActive = false;
				count = mCharacters.Count;
				if(count > 0)
				{
					if(count != mWidth * mHeight)
					{
						//	Remap the display.
						RemapDisplay(mLastUpdateWidth, mLastUpdateHeight,
							mWidth, mHeight);

						bFullRefresh = true;
					}
				}
				else
				{
					//	Rebuild grid.
					mCharacters.SafeClear();
					mCharacters.SafeAddRange(
						Enumerable.Range(0, mHeight * mWidth)
							.Select(i => new CharacterItem(mForeColor, mBackColor)
							{
								Dirty = true
							}));
					mPhysicalBuffer.SafeClear();
					mPhysicalBuffer.SafeAddRange(
						Enumerable.Range(0, mHeight * mWidth)
							.Select(i => new CharacterItem(mForeColor, mBackColor)
							{
								Dirty = true
							}));
					bFullRefresh = true;
				}
				//	The screen pattern matches the display pattern.
				rowCount = mHeight;

				//	** Draw Shapes **
				SetUnshaded(mCharacters);
				foreach(ShapeBase shapeItem in mShapes)
				{
					shapeItem.Render(this);
					shapeItem.AfterRender(this);
				}

				//	** Virtual Update **
				//	Transfer dirty character buffer to physical.
				count = mCharacters.Count;
				for(index = 0; index < count; index ++)
				{
					character = mCharacters[index];
					if(character.Dirty)
					{
						CharacterItem.TransferValues(character, mPhysicalBuffer[index]);
						character.Dirty = false;
					}
				}

				//	** Physical Update **
				if(bFullRefresh)
				{
					Console.Write(AnsiBackColorStart(mBackColor));
					Console.Write(AnsiForeColorStart(mForeColor));
					Console.Write(AnsiEraseInDisplay());
					//Console.Write(AnsiCursorHide());
					SetTerminalPosition(0, 0);
				}
				//	Render the characters in every row.
				rows = new List<int>();
				index = 0;
				count = mPhysicalBuffer.Count;
				for(index = 0; index < count; index++)
				{
					character = mPhysicalBuffer[index];
					if(bFullRefresh || character.Dirty)
					{
						row = (index / mWidth);
						if(!rows.Contains(row))
						{
							rows.Add(row);
						}
					}
				}
				foreach(int rowItem in rows)
				{
					rowIndex = rowItem;
					//	In this strategy, we'll get an entire row to keep the columns
					//	aligned.
					bFound = false;
					for(index = rowItem * mWidth, colIndex = 0, lastX = -1,
						endIndex = index + mWidth;
						index < endIndex; index++, colIndex++)
					{
						character = mPhysicalBuffer[index];
						if(bFound)
						{
							//	Check to see if the current style still matches.
							if(AfterShadowEquals(character, "BackColor", lastBackColor) &&
								AfterShadowEquals(character, "ForeColor", lastForeColor) &&
								lastStyle == character.CharacterStyle &&
								lastX == colIndex - 1)
							{
								builder.Append(GetPrintable(character));
								lastX = colIndex;
							}
							else if(builder.Length > 0)
							{
								//	Current style doesn't match.
								//	Apply the old style.
								if(SetTerminalPosition(startX, rowIndex))
								{
									Console.Write(AnsiBackColorStart(
										lastBackColor ?? mBackColor));
									Console.Write(AnsiForeColorStart(
										lastForeColor ?? mForeColor));
									Console.Write(AnsiStyleStart(lastStyle));
									Console.Write(builder.ToString());
									Console.Write(AnsiResetStyles());
								}
								Clear(builder);
								bFound = false;
							}
						}
						if(!bFound)
						{
							//	Begin a new style.
							lastBackColor = ApplyShadowColor(
								character.BackColor, character.Shadowed);
							lastForeColor = ApplyShadowColor(
								character.ForeColor, character.Shadowed);
							lastStyle = character.CharacterStyle;
							lastX = colIndex;
							startX = colIndex;
							builder.Append(GetPrintable(character));
							bFound = true;
						}
						character.Dirty = false;
					}
					if(builder.Length > 0)
					{
						//	Apply the current style.
						if(SetTerminalPosition(startX, rowIndex))
						{
							Console.Write(AnsiBackColorStart(lastBackColor ?? mBackColor));
							Console.Write(AnsiForeColorStart(lastForeColor ?? mForeColor));
							Console.Write(AnsiStyleStart(lastStyle));
							Console.Write(builder.ToString());
							Console.Write(AnsiResetStyles());
						}
						Clear(builder);
					}
				}
				if(builder.Length > 0)
				{
					//	Apply any remaining style.
					if(SetTerminalPosition(startX, rowIndex))
					{
						Console.Write(AnsiBackColorStart(lastBackColor ?? mBackColor));
						Console.Write(AnsiForeColorStart(lastForeColor ?? mForeColor));
						Console.Write(AnsiStyleStart(lastStyle));
						Console.Write(builder.ToString());
						Console.Write(AnsiResetStyles());
					}
				}

				mCharacterEventsActive = bCharacterEventsActive;
				mLastUpdateWidth = mWidth;
				mLastUpdateHeight = mHeight;
				if(bCursorVisible)
				{
					ShowCursor();
					SetTerminalPosition(mCursorPosition.X, mCursorPosition.Y);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WaitForFilter																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Wait for a specified filter condition to be met on input and return the
		/// filtered value.
		/// </summary>
		/// <param name="filter">
		/// Reference to the filter to apply.
		/// </param>
		/// <returns>
		/// Reference to the matching event received.
		/// </returns>
		/// <remarks>
		/// In this version, non-matching input leading up to the filter is
		/// discarded.
		/// </remarks>
		public InputEventArgs WaitForFilter(InputEventArgs filter)
		{
			InputEventArgs e = null;
			InputEventArgs result = null;

			if(filter != null && mInputMode == InputMode.FilterWait)
			{
				while(result == null)
				{
					e = WaitForInput();
					if(e != null)
					{
						if(e.EventType == filter.EventType)
						{
							result = e;
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Width																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Width">Width</see>.
		/// </summary>
		private int mWidth = 0;
		/// <summary>
		/// Get/Set the width of the terminal.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
			set
			{
				mWidth = value;
				Console.WindowWidth = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Write																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Write a single character at the current character position.
		/// </summary>
		/// <param name="value">
		/// The character value to write.
		/// </param>
		public void Write(char value)
		{
			CharacterItem character = null;

			EnsureLegal(mWidth, mHeight, mCursorPosition);
			character =
				mCharacters[mCursorPosition.Y * mWidth + mCursorPosition.X];
			if(character != null)
			{
				character.Symbol = value;
				character.BackColor = mBackColor;
				character.ForeColor = mForeColor;
				AdvanceCursor(1);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Write a string of characters at the current character position.
		/// </summary>
		/// <param name="value">
		/// The string to write.
		/// </param>
		public void Write(string value)
		{
			CharacterItem character = null;
			char[] chars = null;
			int index = 0;

			EnsureLegal(mWidth, mHeight, mCursorPosition);
			if(value?.Length > 0 && mCharacters?.Count > 0)
			{
				character =
					mCharacters[mCursorPosition.Y * mWidth + mCursorPosition.X];
				if(character != null)
				{
					index = mCharacters.IndexOf(character);
					if(index > -1)
					{
						chars = value.ToCharArray();
						foreach(char charItem in chars)
						{
							character.Symbol = charItem;
							character.BackColor = mBackColor;
							character.ForeColor = mForeColor;
							index++;
							if(index >= mCharacters.Count)
							{
								index = 0;
							}
							try
							{
								character = mCharacters[index];
							}
							catch { }
							mCursorPosition.X++;
						}
						EnsureLegal(mWidth, mHeight, mCursorPosition);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
