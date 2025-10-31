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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using ConsolunaLib;
using ConsolunaLib.Events;
using ConsolunaLib.Shapes;

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
		private bool mInitialized = false;

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
		private void console_InputReceived(object sender,
			InputEventArgs e)
		{
			int asc = 0;

			if(mInitialized && e != null)
			{
				if(e is KeyboardInputEventArgs keyEvent)
				{
					asc = (int)keyEvent.KeyCharacter;
					if(asc > 31 && asc < 127 &&
						((int)keyEvent.KeyModifier & 0x06) == 0)
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
									CursorShapeEnum.BlinkingBlock);
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
				else if(e is MouseInputEventArgs mouseEvent)
				{
					Console.WriteLine(
						$" Mouse: {mouseEvent.MouseX}, {mouseEvent.MouseY} ");
				}
				else if(e is InputResizeEventArgs resizeEvent)
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
				key = "/mode:";
				if(lowerArg.StartsWith(key))
				{
					prg.mMode = ToInt(arg.Substring(key.Length));
				}
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
		//*	Mode																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Mode">Mode</see>.
		/// </summary>
		private int mMode = 0;
		/// <summary>
		/// Get/Set the test mode to run.
		/// </summary>
		public int Mode
		{
			get { return mMode; }
			set { mMode = value; }
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

			switch(mMode)
			{
				case 0:
					//	Color shapes.
					mConsole = new Consoluna()
					{
						InputMode = InputMode.EventDriven,
						PollInterval = 100
					};
					mConsole.InputReceived += console_InputReceived;
					mConsole.BackColor = new ColorInfo("#000050");
					mConsole.ForeColor = new ColorInfo("#d0d000");

					mConsole.Shapes.Add(new TextShape("txtThis",
						"The words you are currently reading are precisely the ones " +
						"that were destined to appear in this very sentence.",
						10, 10, 10, 5, wordWrap: true)
					{
						StyleName = "TextColor"
					});
					mConsole.Shapes.Add(new LabelShape("lblThis",
						"This sentence exists solely to inform you that it contains the " +
						"very words required to say exactly what it's saying.",
						21, 10, 10, 5, wordWrap: true)
					{
						StyleName = "LabelColor"
					});
					mConsole.Shapes.Add(new BoxShape("boxArea",
						"A box",
						32, 10, 10, 5)
					{
						StyleName = "DialogColor",
						BorderStyle = BoxBorderStyle.Single,
						Shadow = true
					});
					mConsole.Shapes.Add(new ScrollBarShape("scrlVert",
						42, 10, 5, CartesianOrientation.Vertical));
					mConsole.Shapes.Add(new ScrollBarShape("scrlHorz",
						43, 10, 5, CartesianOrientation.Horizontal));

					if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						Console.BufferHeight = Console.WindowHeight;
					}

					mConsole.ClearScreen();
					mConsole.SetCursorPosition(10, 9);
					mConsole.SetCursorShape(CursorShapeEnum.None);
					mConsole.Write("Start writing here: ");

					mConsole.Update();
					break;

				case 1:
					//	Show word wrap.
					if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						Console.BufferHeight = 1000;
					}

					baseText = "A dog jogs past big quirky clowns juggling crazy boxes, " +
						"swiftly demonstrating extraordinary coordination, " +
						"unpredictability, and, unquestionably, " +
						"delightfully veracious " +
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

					break;
			}
			mInitialized = true;

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
