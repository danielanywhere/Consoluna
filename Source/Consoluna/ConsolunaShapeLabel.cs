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
using System.Text;
using System.Text.RegularExpressions;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaShapeLabel																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a general label shape with optional hot key definition.
	/// </summary>
	public class ConsolunaShapeLabel : ConsolunaShapeItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
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
		/// Create a new instance of the ConsolunaShapeLabel item.
		/// </summary>
		public ConsolunaShapeLabel()
		{
			ShapeType = ConsolunaShapeType.Label;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaShapeLabel item.
		/// </summary>
		/// <param name="name">
		/// Unique name of the shape.
		/// </param>
		/// <param name="text">
		/// The text to place in the control.
		/// </param>
		/// <param name="x">
		/// The horizontal starting position of the shape.
		/// </param>
		/// <param name="y">
		/// The vertical starting position of the shape.
		/// </param>
		/// <param name="width">
		/// The width of the shape, in characters.
		/// </param>
		/// <param name="height">
		/// The height of the shape, in characters.
		/// </param>
		public ConsolunaShapeLabel(string name, string text = "",
			int x = 0, int y = 0,
			int width = 1, int height = 1, bool wordWrap = false) :
			base(name, x, y, width, height)
		{
			if(text?.Length > 0)
			{
				mText = text;
			}
			mWordWrap = wordWrap;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Render																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Render the character output of this shape to the screen buffer.
		/// </summary>
		/// <param name="screenBuffer">
		/// Reference to the screen buffer to which the contents will be written.
		/// </param>
		public override void Render(Consoluna screenBuffer)
		{
			StringBuilder builder = new StringBuilder();
			ConsolunaCharacterItem character = null;
			char[] chars = null;
			int colCount = 0;
			int colIndex = 0;
			int count = 0;
			int index = 0;
			Match match = null;
			int rowCount = 0;
			int rowIndex = 0;
			int shortcutKeyIndex = -1;
			string text = mText;

			base.Render(screenBuffer);
			if(Visible && mCharacterWindow.GetLength(0) > 0)
			{
				//	If the character window is full, the base values are good.
				colCount = Size.Width;
				rowCount = Size.Height;
				text = text.Replace("\r", "");
				if(mWordWrap)
				{
					text = WordWrap(text, colCount, false);
				}
				if(text.Length > 0)
				{
					match = Regex.Match(text, ResourceMain.rxShortcutKey);
					if(match.Success)
					{
						shortcutKeyIndex = GetIndex(match, "shortcutKey");
						shortcutKeyIndex--;
						if(shortcutKeyIndex > 0)
						{
							builder.Append(text.Substring(0, shortcutKeyIndex));
						}
						builder.Append(text.Substring(shortcutKeyIndex + 1));
						text = builder.ToString();
						Clear(builder);
					}

					if(text.Length > 0)
					{

						//	General layout.
						chars = text.ToCharArray();
						count = chars.Length;
						index = 0;
						colIndex = 0;
						rowIndex = 0;
						if(rowCount > 1 && text.IndexOf('\n') > -1)
						{
							//	Potentially multiple lines only if using line-feeds.
							foreach(char charItem in chars)
							{
								if(charItem == '\n')
								{
									colIndex = 0;
									rowIndex++;
									if(rowIndex >= rowCount)
									{
										break;
									}
								}
								else if(colIndex < colCount)
								{
									character = mCharacterWindow[colIndex, rowIndex];
									if(index == shortcutKeyIndex && mShortcutStyleItem != null)
									{
										if(mShortcutStyleItem.BackColor != null)
										{
											character.BackColor = mShortcutStyleItem.BackColor;
										}
										if(mShortcutStyleItem.ForeColor != null)
										{
											character.ForeColor = mShortcutStyleItem.ForeColor;
										}
									}
									else if(ForeColor != null)
									{
										character.ForeColor = ForeColor;
									}
									character.Character = charItem;
									colIndex++;
								}
								else
								{
									colIndex++;
								}
								index++;
							}
						}
						else
						{
							//	Single line layout.
							foreach(char charItem in chars)
							{
								character = mCharacterWindow[colIndex, rowIndex];
								if(index == shortcutKeyIndex && mShortcutStyleItem != null)
								{
									if(mShortcutStyleItem.BackColor != null)
									{
										character.BackColor = mShortcutStyleItem.BackColor;
									}
									if(mShortcutStyleItem.ForeColor != null)
									{
										character.ForeColor = mShortcutStyleItem.ForeColor;
									}
								}
								else if(ForeColor != null)
								{
									character.ForeColor = ForeColor;
								}
								character.Character = charItem;
								index++;
								colIndex++;
								if(colIndex >= colCount)
								{
									break;
								}
							}
						}
					}
				}
				else
				{
					for(rowIndex = 0; rowIndex < rowCount; rowIndex ++)
					{
						for(colIndex = 0; colIndex < colCount; colIndex++)
						{
							character.Character = '\0';
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShortcutEnabled																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ShortcutEnabled">ShortcutEnabled</see>.
		/// </summary>
		private bool mShortcutEnabled = false;
		/// <summary>
		/// Get/Set a value indicating whether the shortcut key is enabled.
		/// </summary>
		public bool ShortcutEnabled
		{
			get { return mShortcutEnabled; }
			set { mShortcutEnabled = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Text">Text</see>.
		/// </summary>
		private string mText = "";
		/// <summary>
		/// Get/Set the content of this control.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set
			{
				bool bChanged = (mText != value);
				if(value == null)
				{
					mText = "";
				}
				else
				{
					mText = value;
				}
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WordWrap																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WordWrap">WordWrap</see>.
		/// </summary>
		private bool mWordWrap = false;
		/// <summary>
		/// Get/Set a value indicating whether long lines will be wrapped on
		/// shapes with heights greater than 1.
		/// </summary>
		public bool WordWrap
		{
			get { return mWordWrap; }
			set { mWordWrap = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
