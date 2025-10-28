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
	//*	ConsolunaShapeBox																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a general box shape.
	/// </summary>
	public class ConsolunaShapeBox : ConsolunaShapeItem
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
		/// Create a new instance of the ConsolunaShapeBox item.
		/// </summary>
		public ConsolunaShapeBox()
		{
			ShapeType = ConsolunaShapeType.Text;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaShapeBox item.
		/// </summary>
		/// <param name="name">
		/// Unique name of the shape.
		/// </param>
		/// <param name="title">
		/// Text for a title legend.
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
		public ConsolunaShapeBox(string name, string title = "",
			int x = 0, int y = 0,
			int width = 1, int height = 1) :
			base(name, x, y, width, height)
		{
			if(title?.Length > 0)
			{
				mTitle = title;
			}
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
			string title = "";

			base.Render(screenBuffer);
			if(Visible && mCharacterWindow.GetLength(0) > 0)
			{
				//	If the character window is full, the base values are good.
				colCount = Size.Width;
				rowCount = Size.Height;
				if(mTitle?.Length > 0)
				{
					title = GetPrintable(mTitle);
					if(title.Length > colCount - 2)
					{
						title = title.Substring(0, colCount - 2);
					}
				}

				//	Draw the actual box.
				screenBuffer.ClearCharacterWindow(mCharacterWindow,
					ForeColor, BackColor);
				mCharacterWindow[0, 0].Character = (char)201;

				//if(text.Length > 0)
				//{
				//	match = Regex.Match(text, ResourceMain.rxShortcutKey);
				//	if(match.Success)
				//	{
				//		shortcutKeyIndex = GetIndex(match, "shortcutKey");
				//		shortcutKeyIndex--;
				//		if(shortcutKeyIndex > 0)
				//		{
				//			builder.Append(text.Substring(0, shortcutKeyIndex));
				//		}
				//		builder.Append(text.Substring(shortcutKeyIndex + 1));
				//		text = builder.ToString();
				//		Clear(builder);
				//	}

				//	screenBuffer.SetForeColor(mCharacterWindow, ForeColor);
				//	screenBuffer.SetBackColor(mCharacterWindow, BackColor);

				//	if(text.Length > 0)
				//	{

				//		//	General layout.
				//		chars = text.ToCharArray();
				//		count = chars.Length;
				//		index = 0;
				//		colIndex = 0;
				//		rowIndex = 0;
				//		if(rowCount > 1 && text.IndexOf('\n') > -1)
				//		{
				//			//	Potentially multiple lines only if using line-feeds.
				//			foreach(char charItem in chars)
				//			{
				//				if(charItem == '\n')
				//				{
				//					colIndex = 0;
				//					rowIndex++;
				//					if(rowIndex >= rowCount)
				//					{
				//						break;
				//					}
				//				}
				//				else if(colIndex < colCount)
				//				{
				//					character = mCharacterWindow[colIndex, rowIndex];
				//					if(index == shortcutKeyIndex && mShortcutStyleItem != null)
				//					{
				//						if(mShortcutStyleItem.BackColor != null)
				//						{
				//							character.BackColor = mShortcutStyleItem.BackColor;
				//						}
				//						if(mShortcutStyleItem.ForeColor != null)
				//						{
				//							character.ForeColor = mShortcutStyleItem.ForeColor;
				//						}
				//					}
				//					character.Character = charItem;
				//					colIndex++;
				//				}
				//				else
				//				{
				//					colIndex++;
				//				}
				//				index++;
				//			}
				//		}
				//		else
				//		{
				//			//	Single line layout.
				//			foreach(char charItem in chars)
				//			{
				//				character = mCharacterWindow[colIndex, rowIndex];
				//				if(index == shortcutKeyIndex && mShortcutStyleItem != null)
				//				{
				//					if(mShortcutStyleItem.BackColor != null)
				//					{
				//						character.BackColor = mShortcutStyleItem.BackColor;
				//					}
				//					if(mShortcutStyleItem.ForeColor != null)
				//					{
				//						character.ForeColor = mShortcutStyleItem.ForeColor;
				//					}
				//				}
				//				character.Character = charItem;
				//				index++;
				//				colIndex++;
				//				if(colIndex >= colCount)
				//				{
				//					break;
				//				}
				//			}
				//		}
				//	}
				//}
				//else
				//{
				//	for(rowIndex = 0; rowIndex < rowCount; rowIndex++)
				//	{
				//		for(colIndex = 0; colIndex < colCount; colIndex++)
				//		{
				//			character.Character = '\0';
				//		}
				//	}
				//}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Title																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Title">Title</see>.
		/// </summary>
		private string mTitle = "";
		/// <summary>
		/// Get/Set the display title of this control.
		/// </summary>
		public string Title
		{
			get { return mTitle; }
			set
			{
				bool bChanged = (mTitle != value);
				if(value == null)
				{
					mTitle = "";
				}
				else
				{
					mTitle = value;
				}
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
