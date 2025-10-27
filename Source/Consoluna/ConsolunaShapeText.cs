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
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaShapeText																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a general text shape with optional hot key definition.
	/// </summary>
	public class ConsolunaShapeText : ConsolunaShapeItem
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
		/// Create a new instance of the ConsolunaShapeText item.
		/// </summary>
		public ConsolunaShapeText()
		{
			ShapeType = ConsolunaShapeType.Text;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaShapeText item.
		/// </summary>
		/// <param name="name">
		/// Unique name of the shape.
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
		public ConsolunaShapeText(string name, int x = 0, int y = 0,
			int width = 1, int height = 1) : this()
		{
			if(name?.Length > 0)
			{
				Name = name;
			}
			Position.X = x;
			Position.Y = y;
			Size.Width = width;
			Size.Height = height;
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
		public override void Render(ConsolunaScreenBuffer screenBuffer)
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
			ConsolunaScreenStyleItem shortcutStyle = null;
			string text = mText;
			ConsolunaScreenStyleItem textStyle = null;
			ConsolunaTokenItem token = null;
			ConsolunaTokenCollection tokens = null;

			//	TODO: Test of text.
			base.Render(screenBuffer);
			if(Visible && mCharacterWindow.GetLength(0) > 0)
			{
				//	If the character window is full, the base values are good.
				colCount = Size.Width;
				rowCount = Size.Height;
				textStyle = screenBuffer.Styles.FirstOrDefault(x =>
					x.Name == "TextColor");
				text = text.Replace("\r", "");
				if(text.Length > 0)
				{
					shortcutStyle = screenBuffer.Styles.FirstOrDefault(x =>
						x.Name == "ShortcutStyle");
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
						tokens = ConsolunaTokenCollection.ParseWords(text);
						if(text.Length > colCount && rowCount > 1 && mWordWrap)
						{
							//	Word-wrapping layout.
							colIndex = 0;
							rowIndex = 0;
							count = tokens.Count;
							for(index = 0; index < count; index ++)
							{
								token = tokens[index];
								if(token.TokenType == ConsolunaTokenType.Whitespace &&
									token.Value == "\n")
								{
									colIndex = 0;
									rowIndex++;
								}
								else if(colIndex + token.Value.Length >= colCount &&
									colIndex > 0)
								{
									//	Place the word on the next line.
									if(token.Value != "\n")
									{
										token = new ConsolunaTokenItem()
										{
											TokenType = ConsolunaTokenType.Whitespace,
											Value = "\n"
										};
										if(shortcutKeyIndex >= index)
										{
											shortcutKeyIndex++;
										}
										tokens.Insert(index, token);
										count++;
									}
									colIndex = 0;
									rowIndex++;
								}
								else
								{
									colIndex += token.Value.Length;
								}
							}
							text = tokens.ToString();
						}

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
								else
								{
									character = mCharacterWindow[colIndex, rowIndex];
									if(index == shortcutKeyIndex && shortcutStyle != null)
									{
										if(shortcutStyle.BackColor != null)
										{
											character.BackColor = shortcutStyle.BackColor;
										}
										if(shortcutStyle.ForeColor != null)
										{
											character.ForeColor = shortcutStyle.ForeColor;
										}
									}
									else if(textStyle != null)
									{
										if(textStyle.BackColor != null)
										{
											character.BackColor = textStyle.BackColor;
										}
										if(textStyle.ForeColor != null)
										{
											character.ForeColor = textStyle.ForeColor;
										}
									}
									character.Character = charItem;
								}
								index++;
								colIndex++;
								if(colIndex >= colCount)
								{
									break;
								}
							}
						}
						else
						{
							//	Single line layout.
							foreach(char charItem in chars)
							{
								character = mCharacterWindow[colIndex, rowIndex];
								if(index == shortcutKeyIndex && shortcutStyle != null)
								{
									if(shortcutStyle.BackColor != null)
									{
										character.BackColor = shortcutStyle.BackColor;
									}
									if(shortcutStyle.ForeColor != null)
									{
										character.ForeColor = shortcutStyle.ForeColor;
									}
								}
								else if(textStyle != null)
								{
									if(textStyle.BackColor != null)
									{
										character.BackColor = textStyle.BackColor;
									}
									if(textStyle.ForeColor != null)
									{
										character.ForeColor = textStyle.ForeColor;
									}
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
							character = mCharacterWindow[colIndex, rowIndex];
							if(textStyle != null)
							{
								if(textStyle.BackColor != null)
								{
									character.BackColor = textStyle.BackColor;
								}
								if(textStyle.ForeColor != null)
								{
									character.ForeColor = textStyle.ForeColor;
								}
							}
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
