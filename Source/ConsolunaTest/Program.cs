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
using System.Linq;
using System.Runtime.InteropServices;
using ConsolunaLib;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaTest
{
	//*-------------------------------------------------------------------------*
	//*	Program																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Main instance of the ConsolePlus Test application.
	/// </summary>
	public class Program
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* console_InputReceived																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Input has been received from the console.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console input event arguments.
		/// </param>
		private void console_InputReceived(object sender, ConsolunaInputEventArgs e)
		{
			int asc = 0;
			ConsolunaCharacterItem character = null;
			int width = 0;
			int height = 0;

			if(e != null)
			{
				if(e is ConsolunaInputKeyboardEventArgs keyEvent)
				{
					asc = (int)keyEvent.KeyCharacter;
					if(asc > 31 && asc < 127)
					{
						mConsole.Write(keyEvent.KeyCharacter);
					}
					else
					{
						switch(asc)
						{
							case 8:
								//	Backspace.
								mConsole.Backspace(1);
								mConsole.ShowCursor();
								break;
							case 9:
								//	Tab.
								mConsole.Tab(1);
								mConsole.SetCursorShape(
									ConsolunaCursorShapeEnum.BlinkingBlock);
								break;
							case 10:
								//	Line feed.
								mConsole.LineFeed(1);
								break;
							case 13:
								//	Carriage return.
								mConsole.CarriageReturn();
								mConsole.LineFeed(1);
								break;
							case 127:
								//	Delete.
								break;
						}
					}
					mConsole.Update();
				}
				else if(e is ConsolunaInputMouseEventArgs mouseEvent)
				{
					Console.WriteLine(
						$" Mouse: {mouseEvent.MouseX}, {mouseEvent.MouseY} ");
				}
				else if(e is ConsolunaInputResizeEventArgs resizeEvent)
				{
					mConsole.SaveCursorPosition();
					mConsole.SetCursorPosition(0, 0);
					mConsole.Write(
						$" Size: {resizeEvent.Width}, {resizeEvent.Height} ");
					if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						mConsole.Write("Win");
					}
					else
					{
						mConsole.Write("Non-Win");
					}
					mConsole.Update();
					mConsole.RestoreCursorPosition();
					mConsole.HideCursor();
				}
				e.Handled = true;
			}

		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Main																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Configure and run the application.
		/// </summary>
		public static void Main(string[] args)
		{
			bool bShowHelp = false; //	Flag - Explicit Show Help.
			string key = "";        //	Current Parameter Key.
			string lowerArg = "";   //	Current Lowercase Argument.
			string message = "";    //	Message to display in Console.
			Program prg = new Program();  //	Initialized instance.

			Console.WriteLine("ConsolePlusTest.exe");
			foreach(string arg in args)
			{
				lowerArg = arg.ToLower();
				key = "/?";
				if(lowerArg == key)
				{
					bShowHelp = true;
					continue;
				}
				//key = "/exampleparameter:";
				//if(lowerArg.StartsWith(key))
				//{
				//	prg.exampleparameter = arg.Substring(key.Length);
				//	continue;
				//}
				key = "/wait";
				if(lowerArg.StartsWith(key))
				{
					prg.mWaitAfterEnd = true;
					continue;
				}
			}
			if(bShowHelp)
			{
				//	Display Syntax.
				Console.WriteLine(message.ToString() + "\r\n" + ResourceMain.Syntax);
			}
			else
			{
				//	Run the configured application.
				prg.Run();
			}
			if(prg.mWaitAfterEnd)
			{
				Console.WriteLine("Press [Enter] to exit...");
				Console.ReadLine();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Run																																		*
		//*-----------------------------------------------------------------------*
		private Consoluna mConsole = null;
		/// <summary>
		/// Run the configured application.
		/// </summary>
		public void Run()
		{
			string baseText = "";
			string testText = "";

			Console.Clear();
			Console.BufferHeight = 1000;

			baseText = "A fox jogs past big quirky wizards juggling hazy boxes, " +
				"swiftly demonstrating extraordinary coordination, " +
				"unpredictability, and, unquestionably, " +
				"delightful " +
				"supercalifragilisticexpialidociousness.";

			Console.WriteLine(
				$"Word-wrapping the following with split:\r\n{baseText}\r\n");
			testText = WordWrap(baseText, 10, true);
			Console.WriteLine("* OUTPUT *");
			Console.WriteLine(testText);
			Console.WriteLine("");

			Console.WriteLine(
				$"Word-wrapping the following without split:\r\n{baseText}\r\n");
			testText = WordWrap(baseText, 10, false);
			Console.WriteLine("* OUTPUT *");
			Console.WriteLine(testText);
			Console.WriteLine("");


			Console.WriteLine("Press [Enter] to continue.");
			Console.ReadLine();


			mConsole = new Consoluna()
			{
				InputMode = ConsolunaInputMode.EventDriven,
				PollInterval = 100
			};
			mConsole.InputReceived += console_InputReceived;
			mConsole.BackColor = new ConsolunaColor("#000050");
			mConsole.ForeColor = new ConsolunaColor("#d0d000");
			mConsole.Shapes.Add(new ConsolunaShapeText("txtThis",
				"The words you are currently reading are precisely the ones that " +
				"were destined to appear in this very sentence.",
				10, 10, 10, 5, wordWrap: true)
			{
				StyleName = "TextColor"
			});
			mConsole.ClearScreen();
			mConsole.SetCursorPosition(10, 9);
			mConsole.SetCursorShape(ConsolunaCursorShapeEnum.None);
			mConsole.Write("Start writing here: ");

			mConsole.Update();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaitAfterEnd																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WaitAfterEnd">WaitAfterEnd</see>.
		/// </summary>
		private bool mWaitAfterEnd = false;
		/// <summary>
		/// Get/Set a value indicating whether to wait for user keypress after
		/// processing has completed.
		/// </summary>
		public bool WaitAfterEnd
		{
			get { return mWaitAfterEnd; }
			set { mWaitAfterEnd = value; }
		}

	}
	//*-------------------------------------------------------------------------*

}
