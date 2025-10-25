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
using System.Threading;

using ConsolunaLib.Internal;

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
		/// Reference to the platform-specific input handler currently active.
		/// </summary>
		private IConsoleInputHandler mInputHandler = null;
		/// <summary>
		/// The last-known height of the display.
		/// </summary>
		private int mLastHeight = 0;
		/// <summary>
		/// The last-known width of the display.
		/// </summary>
		private int mLastWidth = 0;
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
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the caller's string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the builder to clear.
		/// </param>
		private static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetInput																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a single event from the console, if available.
		/// </summary>
		/// <returns>
		/// Reference to the single event, if found. Otherwise, null.
		/// </returns>
		private ConsoleInputEventArgs GetInput()
		{
			bool bChanged = false;
			ConsoleInputEventArgs e = null;
			int value = 0;

			e = mInputHandler.ReadInput();
			if(e == null)
			{
				mWidth = Console.WindowWidth;
				mHeight = Console.WindowHeight;
				if(mWidth != mLastWidth || mHeight != mLastHeight)
				{
					e = new ConsoleInputResizeEventArgs()
					{
						Width = mWidth,
						Height = mHeight
					};
					mLastWidth = mWidth;
					mLastHeight = mHeight;
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
			ConsolePropertyChangeEventArgs e)
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
			ConsolePropertyChangeEventArgs e)
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
			CollectionChangeEventArgs<ConsoleCharacterItem> e)
		{
			OnScreenCharacterCollectionChanged(
				new ConsoleCharacterCollectionEventArgs(e));
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
			ConsolePropertyChangeEventArgs e)
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
			CollectionChangeEventArgs<ConsoleShapeItem> e)
		{
			OnScreenShapeCollectionChanged(
				new ConsoleShapeCollectionEventArgs(e));
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
			ConsolePropertyChangeEventArgs e)
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
			ConsoleInputEventArgs e = null;

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
			List<ConsoleCharacterItem> newDisplay = null;
			int rowIndex = 0;

			if(oldWidth != newWidth || oldHeight != newHeight)
			{
				newDisplay =
				Enumerable.Range(0, newHeight * newWidth)
					.Select(i => new ConsoleCharacterItem()
					{
						BackColor = mBackColor,
						ForeColor = mForeColor,
						Dirty = false
					})
					.ToList();
				minWidth = Math.Min(oldWidth, newWidth);
				minHeight = Math.Min(oldHeight, newHeight);
				for(rowIndex = 0; rowIndex < minHeight; rowIndex ++)
				{
					for(colIndex = 0; colIndex < minWidth; colIndex ++)
					{
						newDisplay[rowIndex * newWidth + colIndex] =
							mScreenCharacters[rowIndex * oldWidth + colIndex];
					}
				}
			}
			mCharacterEventsActive = false;
			mScreenCharacters.Clear();
			mScreenCharacters.AddRange(newDisplay);
			mCharacterEventsActive = bCharacterEvents;
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
		private ConsoleInputEventArgs WaitForInput()
		{
			ConsoleInputEventArgs e = null;

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
			ConsoleInputKeyboardEventArgs e)
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
		protected virtual void OnInputReceived(ConsoleInputEventArgs e)
		{
			InputReceived?.Invoke(this, e);
			if(e != null)
			{
				if(e is ConsoleInputKeyboardEventArgs keyEvent)
				{
					OnKeyboardInputReceived(keyEvent);
				}
				else if(e is ConsoleInputMouseEventArgs mouseEvent)
				{
					OnMouseInputReceived(mouseEvent);
				}
				else if(e is ConsoleInputResizeEventArgs resizeEvent)
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
		protected virtual void OnMouseInputReceived(ConsoleInputMouseEventArgs e)
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
			ConsoleCharacterCollectionEventArgs e)
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
			ConsolePropertyChangeEventArgs e)
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
			ConsoleShapeCollectionEventArgs e)
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
			ConsolePropertyChangeEventArgs e)
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
		protected virtual void OnTerminalResized(ConsoleInputResizeEventArgs e)
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
			//// TODO: Set the console's output encoding to Code Page 437
			//Console.OutputEncoding = Encoding.GetEncoding(437);

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				mInputHandler = new ConsoleInputHandlerWindows();
			}
			else
			{
				mInputHandler = new ConsoleInputHandlerNCurses();
			}
			if(mInputHandler != null)
			{
				mInputHandler.Initialize();
			}

			mForeColor = new ConsolunaColor()
			{
				Red = 0x7f,
				Green = 0x7f,
				Blue = 0x7f
			};
			mForeColor.PropertyChanged += mForeColor_PropertyChanged;

			mBackColor = new ConsolunaColor();
			mBackColor.PropertyChanged += mBackColor_PropertyChanged;

			mScreenCharacters = new ConsoleCharacterCollection();
			mScreenCharacters.CollectionChanged +=
				mScreenCharacters_CollectionChanged;
			mScreenCharacters.ItemPropertyChanged +=
				mScreenCharacters_ItemPropertyChanged;
			mScreenShapes = new ConsoleShapeCollection();
			mScreenShapes.CollectionChanged +=
				mScreenShapes_CollectionChanged;
			mScreenShapes.ItemPropertyChanged +=
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
			EnsureLegalCursor();
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
		public static string AnsiBackColorStart(ConsolunaColor color)
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
		public static string AnsiForeColorStart(ConsolunaColor color)
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
		public static string AnsiStyleStart(ConsoleCharacterStyle style)
		{
			StringBuilder builder = new StringBuilder();

			if(style != ConsoleCharacterStyle.None)
			{
				if((style & ConsoleCharacterStyle.Blink) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[5m");
				}
				if((style & ConsoleCharacterStyle.Bold) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[1m");
				}
				if((style & ConsoleCharacterStyle.Faint) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[2m");
				}
				if((style & ConsoleCharacterStyle.Hidden) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[8m");
				}
				if((style & ConsoleCharacterStyle.Inverse) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[7m");
				}
				if((style & ConsoleCharacterStyle.Italic) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[3m");
				}
				if((style & ConsoleCharacterStyle.Strike) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[9m");
				}
				if((style & ConsoleCharacterStyle.Underline) != ConsoleCharacterStyle.None)
				{
					builder.Append("\x1b[4m");
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BackColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BackColor">BackColor</see>.
		/// </summary>
		private ConsolunaColor mBackColor = null;
		/// <summary>
		/// Get/Set a reference to the default background color for this screen.
		/// </summary>
		public ConsolunaColor BackColor
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
			ConsoleCharacterItem character = null;
			int endIndex = 0;
			int index = 0;
			int startIndex = 0;

			EnsureLegalCursor();
			startIndex = mCursorPosition.Y * mWidth + mCursorPosition.X - 1;
			endIndex = startIndex - count;
			for(index = startIndex; index > -1 && index > endIndex; index --)
			{
				character = mScreenCharacters[index];
				character.Character = char.MinValue;
				mCursorPosition.X--;
			}
			EnsureLegalCursor();
			Console.SetCursorPosition(mCursorPosition.X, mCursorPosition.Y);
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
			EnsureLegalCursor();
			mCursorPosition.X = 0;
			Console.SetCursorPosition(mCursorPosition.X, mCursorPosition.Y);
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
			mScreenCharacters.Clear();
			Update();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CursorHide																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Hide the cursor at its current location.
		/// </summary>
		public void CursorHide()
		{
			Console.Write(AnsiCursorHide());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CursorPosition																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="CursorPosition">CursorPosition</see>.
		/// </summary>
		private ConsolunaPosition mCursorPosition = new ConsolunaPosition();
		/// <summary>
		/// Get a reference to the current cursor position.
		/// </summary>
		public ConsolunaPosition CursorPosition
		{
			get { return mCursorPosition; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CursorShow																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Show the cursor at its current location.
		/// </summary>
		public void CursorShow()
		{
			Console.Write(AnsiCursorShow());
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
		//* EnsureLegalCursor																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Ensure that the cursor is in a legal position.
		/// </summary>
		public void EnsureLegalCursor()
		{
			int total = mCursorPosition.Y * mWidth + mCursorPosition.X;

			mCursorPosition.X = total % mWidth;
			mCursorPosition.Y = (total / mWidth) % mHeight;

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ForeColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ForeColor">ForeColor</see>.
		/// </summary>
		private ConsolunaColor mForeColor = null;
		/// <summary>
		/// Get/Set a reference to the default foreground color for this screen.
		/// </summary>
		public ConsolunaColor ForeColor
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
		//*	InputMode																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="InputMode">InputMode</see>.
		/// </summary>
		private ConsoleInputMode mInputMode = ConsoleInputMode.DirectPoll;
		/// <summary>
		/// Get/Set the active input mode for this instance.
		/// </summary>
		public ConsoleInputMode InputMode
		{
			get { return mInputMode; }
			set
			{
				mInputMode = value;
				if(mInputMode == ConsoleInputMode.EventDriven)
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
		public event EventHandler<ConsoleInputEventArgs> InputReceived;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* KeyboardInputReceived																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when input has been received from the keyboard.
		/// </summary>
		public event EventHandler<ConsoleInputKeyboardEventArgs>
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
			EnsureLegalCursor();
			mCursorPosition.Y += count;
			EnsureLegalCursor();
			Console.SetCursorPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MouseInputReceived																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when input has been received from the mouse.
		/// </summary>
		public event EventHandler<ConsoleInputMouseEventArgs> MouseInputReceived;
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
		/// Reference to a ConsoleInputEventArgs representing the read event,
		/// if found. Otherwise, null.
		/// </returns>
		public ConsoleInputEventArgs Read()
		{
			ConsoleInputEventArgs result = GetInput();
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenCharacterCollectionChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of the ScreenCharacters collection have
		/// changed.
		/// </summary>
		public event EventHandler<ConsoleCharacterCollectionEventArgs>
			ScreenCharacterCollectionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenCharacterItemChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of a Screen Character have changed.
		/// </summary>
		public event EventHandler<ConsolePropertyChangeEventArgs>
			ScreenCharacterItemChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScreenCharacters																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ScreenCharacters">ScreenCharacters</see>.
		/// </summary>
		private ConsoleCharacterCollection mScreenCharacters = null;
		/// <summary>
		/// Get a reference to the character-based screen cache layer.
		/// </summary>
		public ConsoleCharacterCollection ScreenCharacters
		{
			get { return mScreenCharacters; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenShapeCollectionChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the contents of the screen shapes collection have changed.
		/// </summary>
		public event EventHandler<ConsoleShapeCollectionEventArgs>
			ScreenShapeCollectionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScreenShapeItemChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when a single value of a screen shape has been changed.
		/// </summary>
		public event EventHandler<ConsolePropertyChangeEventArgs>
			ScreenShapeItemChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScreenShapes																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ScreenShapes">ScreenShapes</see>.
		/// </summary>
		private ConsoleShapeCollection mScreenShapes = null;
		/// <summary>
		/// Get a reference to the collection of shapes in this instance.
		/// </summary>
		public ConsoleShapeCollection ScreenShapes
		{
			get { return mScreenShapes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetCursorPosition																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the current cursor position.
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
		public void SetCursorShape(ConsoleCursorShapeEnum cursorShape)
		{
			switch(cursorShape)
			{
				case ConsoleCursorShapeEnum.Bar:
					Console.Write("\x1b[5q");
					break;
				case ConsoleCursorShapeEnum.BlinkingBar:
					Console.Write("\x1b[6q");
					break;
				case ConsoleCursorShapeEnum.BlinkingBlock:
					Console.Write("\x1b[2q");
					break;
				case ConsoleCursorShapeEnum.BlinkingUnderline:
					Console.Write("\x1b[4q");
					break;
				case ConsoleCursorShapeEnum.Block:
					Console.Write("\x1b[1q");
					break;
				case ConsoleCursorShapeEnum.None:
					CursorHide();
					break;
				case ConsoleCursorShapeEnum.Underline:
					Console.Write("\x1b[3q");
					break;
			}
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
			EnsureLegalCursor();
			mCursorPosition.X += (count * 4);
			EnsureLegalCursor();
			Console.SetCursorPosition(mCursorPosition.X, mCursorPosition.Y);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TerminalResized																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the terminal has been resized.
		/// </summary>
		public event EventHandler<ConsoleInputResizeEventArgs> TerminalResized;
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
		public ConsoleInputEventArgs WaitForFilter(ConsoleInputEventArgs filter)
		{
			ConsoleInputEventArgs e = null;
			ConsoleInputEventArgs result = null;

			if(filter != null && mInputMode == ConsoleInputMode.FilterWait)
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
			bool bFound = false;
			bool bFullRefresh = false;
			StringBuilder builder = new StringBuilder();
			ConsoleCharacterItem character = null;
			int colCount = 0;
			int colIndex = 0;
			int count = 0;
			List<ConsoleCharacterItem> dirties = null;
			int endIndex = 0;
			//int height = Console.WindowHeight;
			int index = 0;
			ConsolunaColor lastBackColor = null;
			ConsolunaColor lastForeColor = null;
			ConsoleCharacterStyle lastStyle = ConsoleCharacterStyle.None;
			int lastX = 0;
			int min = 0;
			int row = 0;
			//List<ConsoleCharacterItem> rowContent = null;
			int rowCount = 0;
			int rowIndex = 0;
			List<int> rows = null;
			int startX = 0;
			//int width = Console.WindowWidth;

			mWidth = Console.WindowWidth;
			mHeight = Console.WindowHeight;

			mCharacterEventsActive = false;
			count = mScreenCharacters.Count;
			if(count > 0)
			{
				if(count != mWidth * mHeight)
				{
					//	Remap the display.
					RemapDisplay(mLastUpdateWidth, mLastUpdateHeight,
						mWidth, mHeight);

					//colCount = mScreenCharacters.Max(x => x.Position.X) + 1;
					//rowCount = mScreenCharacters.Max(y => y.Position.Y) + 1;
					//if(colCount != mWidth)
					//{
					//	min = Math.Min(rowCount, mHeight);
					//	if(colCount < mWidth)
					//	{
					//		//	Add columns to existing rows.
					//		for(colIndex = colCount; colIndex < mWidth; colIndex ++)
					//		{
					//			for(rowIndex = 0; rowIndex < min; rowIndex++)
					//			{
					//				//	Add a new character at this row and column.
					//				character = new ConsoleCharacterItem()
					//				{
					//					BackColor = this.mBackColor,
					//					ForeColor = this.mForeColor
					//				};
					//				character.Position.X = colIndex;
					//				character.Position.Y = rowIndex;
					//				character.Dirty = false;
					//				mScreenCharacters.Add(character);
					//			}
					//		}
					//	}
					//	else
					//	{
					//		//	Remove columns from existing rows.
					//		mScreenCharacters.RemoveAll(x => x.Position.X >= mWidth);
					//	}
					//}
					//if(rowCount != mHeight)
					//{
					//	if(rowCount < mHeight)
					//	{
					//		//	Add rows.
					//		for(colIndex = 0; colIndex < mWidth; colIndex ++)
					//		{
					//			for(rowIndex = rowCount; rowIndex < mHeight; rowIndex ++)
					//			{
					//				//	Add a new character at this row and column.
					//				character = new ConsoleCharacterItem()
					//				{
					//					BackColor = this.mBackColor,
					//					ForeColor = this.mForeColor
					//				};
					//				character.Position.X = colIndex;
					//				character.Position.Y = rowIndex;
					//				character.Dirty = false;
					//				mScreenCharacters.Add(character);
					//			}
					//		}
					//	}
					//	else
					//	{
					//		//	Remove rows.
					//		mScreenCharacters.RemoveAll(y => y.Position.Y >= mHeight);
					//	}
					//}
					bFullRefresh = true;
				}
			}
			else
			{
				//	Rebuild grid.
				mScreenCharacters.Clear();
				mScreenCharacters.AddRange(
					Enumerable.Range(0, mHeight * mWidth)
						.Select(i => new ConsoleCharacterItem()
						{
							BackColor = mBackColor,
							ForeColor = mForeColor,
							Dirty = false
						}));
				bFullRefresh = true;
			}
			//	The screen pattern matches the display pattern.
			rowCount = mHeight;
			//	TODO: Draw shapes over the character map.
			//	Render the characters in every row.
			if(bFullRefresh)
			{
				Console.Write(AnsiBackColorStart(mBackColor));
				Console.Write(AnsiForeColorStart(mForeColor));
				Console.Write(AnsiEraseInDisplay());
				//Console.Write(AnsiCursorHide());
				Console.SetCursorPosition(0, 0);
			}
			//rows = mScreenCharacters
			//	.Where(x => x.Dirty)
			//	.Select(y => y.Position.Y)
			//	.Distinct()
			//	.ToList();
			rows = new List<int>();
			index = 0;
			count = mScreenCharacters.Count;
			for(index = 0; index < count; index ++)
			{
				character = mScreenCharacters[index];
				if(character.Dirty)
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
					index < endIndex; index ++, colIndex ++)
				{
					character = mScreenCharacters[index];
					if(bFound)
					{
						//	Check to see if the current style still matches.
						if(ConsolunaColor.Equals(
								lastBackColor, character.BackColor) &&
							ConsolunaColor.Equals(
								lastForeColor, character.ForeColor) &&
							lastStyle == character.CharacterStyle &&
							lastX == colIndex - 1)
						{
							builder.Append(ConsoleCharacterItem.GetPrintable(character));
							lastX = colIndex;
						}
						else if(builder.Length > 0)
						{
							//	Apply the style.
							Console.SetCursorPosition(startX, rowIndex);
							Console.Write(AnsiBackColorStart(lastBackColor ?? mBackColor));
							Console.Write(AnsiForeColorStart(lastForeColor ?? mForeColor));
							Console.Write(AnsiStyleStart(lastStyle));
							Console.Write(builder.ToString());
							Console.Write(AnsiResetStyles());
							Clear(builder);
							bFound = false;
						}
					}
					if(!bFound)
					{
						//	Begin a new style.
						lastBackColor = character.BackColor;
						lastForeColor = character.ForeColor;
						lastStyle = character.CharacterStyle;
						lastX = colIndex;
						startX = colIndex;
						builder.Append(ConsoleCharacterItem.GetPrintable(character));
						bFound = true;
					}
					character.Dirty = false;
				}
				if(builder.Length > 0)
				{
					//	Apply the current style.
					Console.SetCursorPosition(startX, rowIndex);
					Console.Write(AnsiBackColorStart(lastBackColor ?? mBackColor));
					Console.Write(AnsiForeColorStart(lastForeColor ?? mForeColor));
					Console.Write(AnsiStyleStart(lastStyle));
					Console.Write(builder.ToString());
					Console.Write(AnsiResetStyles());
					Clear(builder);
				}
			}
			if(builder.Length > 0)
			{
				//	Apply any remaining style.
				Console.SetCursorPosition(startX, rowIndex);
				Console.Write(AnsiBackColorStart(lastBackColor ?? mBackColor));
				Console.Write(AnsiForeColorStart(lastForeColor ?? mForeColor));
				Console.Write(AnsiStyleStart(lastStyle));
				Console.Write(builder.ToString());
				Console.Write(AnsiResetStyles());
			}
			mCharacterEventsActive = bCharacterEventsActive;
			mLastUpdateWidth = mWidth;
			mLastUpdateHeight = mHeight;
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
			ConsoleCharacterItem character = null;

			EnsureLegalCursor();
			character =
				mScreenCharacters[mCursorPosition.Y * mWidth + mCursorPosition.X];
			if(character != null)
			{
				character.Character = value;
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
			ConsoleCharacterItem character = null;
			char[] chars = null;
			int index = 0;

			EnsureLegalCursor();
			if(value?.Length > 0)
			{
				character =
					mScreenCharacters[mCursorPosition.Y * mWidth + mCursorPosition.X];
				if(character != null)
				{
					index = mScreenCharacters.IndexOf(character);
					if(index > -1)
					{
						chars = value.ToCharArray();
						foreach(char charItem in chars)
						{
							character.Character = charItem;
							character.BackColor = mBackColor;
							character.ForeColor = mForeColor;
							index++;
							if(index >= mScreenCharacters.Count)
							{
								index = 0;
							}
							character = mScreenCharacters[index];
							mCursorPosition.X++;
						}
						EnsureLegalCursor();
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
