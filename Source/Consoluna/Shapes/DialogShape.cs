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

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib.Shapes
{
	//*-------------------------------------------------------------------------*
	//*	DialogShape																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a dialog box shape.
	/// </summary>
	public class DialogShape : ShapeBase
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* CommonInit																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Common initialization.
		/// </summary>
		private void CommonInit()
		{
			BoxBorderStyle = BoxBorderStyle.Double;
			StyleName = "WindowColor";
			mBorderColorStyleName = "WindowBorderColor";
			mWindowButtonColorStyleName = "WindowButtonColor";
			mListBoxColorStyleName = "WindowListBoxColor";
			mListBoxHeaderColorStyleName = "WindowListBoxHeaderColor";
			mListBoxHighlightColorStyleName = "WindowListBoxHighlightColor";
			mTitleColorStyleName = "WindowTitleColor";
			ShapeType = ShapeType.Dialog;
		}
		//*-----------------------------------------------------------------------*

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
		/// Create a new instance of the BoxShape item.
		/// </summary>
		public DialogShape()
		{
			CommonInit();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the BoxShape item.
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
		public DialogShape(string name, string title = "",
			int x = 0, int y = 0,
			int width = 1, int height = 1) :
			base(name, x, y, width, height)
		{
			CommonInit();
			if(title?.Length > 0)
			{
				mTitle = title;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BorderColorStyle																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BorderColorStyle">BorderColorStyle</see>.
		/// </summary>
		private CharacterStyle mBorderColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the border color style for this object.
		/// </summary>
		public CharacterStyle BorderColorStyle
		{
			get { return mBorderColorStyle; }
			set
			{
				bool bChanged = (mBorderColorStyle != value);
				mBorderColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BorderColorStyleName																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="BorderColorStyleName">BorderColorStyleName</see>.
		/// </summary>
		private string mBorderColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the border color style.
		/// </summary>
		public string BorderColorStyleName
		{
			get { return mBorderColorStyleName; }
			set
			{
				bool bChanged = (mBorderColorStyleName != value);
				mBorderColorStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BoxBorderStyle																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BoxBorderStyle">BoxBorderStyle</see>.
		/// </summary>
		private BoxBorderStyle mBoxBorderStyle =
			BoxBorderStyle.Double;
		/// <summary>
		/// Get/Set the border style for the box.
		/// </summary>
		public BoxBorderStyle BoxBorderStyle
		{
			get { return mBoxBorderStyle; }
			set { mBoxBorderStyle = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxColorStyle																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxColorStyle">ListBoxColorStyle</see>.
		/// </summary>
		private CharacterStyle mListBoxColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the list box color style for this object.
		/// </summary>
		public CharacterStyle ListBoxColorStyle
		{
			get { return mListBoxColorStyle; }
			set
			{
				bool bChanged = (mListBoxColorStyle != value);
				mListBoxColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxColorStyleName																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxColorStyleName">ListBoxColorStyleName</see>.
		/// </summary>
		private string mListBoxColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the list box color style for this dialog.
		/// </summary>
		public string ListBoxColorStyleName
		{
			get { return mListBoxColorStyleName; }
			set
			{
				bool bChanged = (mListBoxColorStyleName != value);
				mListBoxColorStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxHeaderColorStyle																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxHeaderColorStyle">ListBoxHeaderColorStyle</see>.
		/// </summary>
		private CharacterStyle mListBoxHeaderColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the list box header color style for this object.
		/// </summary>
		public CharacterStyle ListBoxHeaderColorStyle
		{
			get { return mListBoxHeaderColorStyle; }
			set
			{
				bool bChanged = (mListBoxHeaderColorStyle != value);
				mListBoxHeaderColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxHeaderColorStyleName																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxHeaderColorStyleName">
		/// ListBoxHeaderColorStyleName
		/// </see>.
		/// </summary>
		private string mListBoxHeaderColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the color style for list box headers on this
		/// dialog.
		/// </summary>
		public string ListBoxHeaderColorStyleName
		{
			get { return mListBoxHeaderColorStyleName; }
			set
			{
				bool bChanged = (mListBoxHeaderColorStyleName != value);
				mListBoxHeaderColorStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxHighlightColorStyle																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxHighlightColorStyle">
		/// ListBoxHighlightColorStyle
		/// </see>.
		/// </summary>
		private CharacterStyle mListBoxHighlightColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the list box highlight color style for this
		/// object.
		/// </summary>
		public CharacterStyle ListBoxHighlightColorStyle
		{
			get { return mListBoxHighlightColorStyle; }
			set
			{
				bool bChanged = (mListBoxHighlightColorStyle != value);
				mListBoxHighlightColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ListBoxHighlightColorStyleName																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ListBoxHighlightColorStyleName">
		/// ListBoxHighlightColorStyleName
		/// </see>.
		/// </summary>
		private string mListBoxHighlightColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the listbox highlight color for this dialog.
		/// </summary>
		public string ListBoxHighlightColorStyleName
		{
			get { return mListBoxHighlightColorStyleName; }
			set
			{
				bool bChanged = (mListBoxHighlightColorStyleName != value);
				mListBoxHighlightColorStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
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
			char charBLCorner = DrawingSymbols.GetSymbolValue(
				$"BoxBottomLeftCorner{mBoxBorderStyle}");
			char charBRCorner = DrawingSymbols.GetSymbolValue(
				$"BoxBottomRightCorner{mBoxBorderStyle}");
			char charCloseButton = DrawingSymbols.GetSymbolValue(
				"DialogCloseButton");
			char charHLine = DrawingSymbols.GetSymbolValue(
				$"BoxHorizontalLine{mBoxBorderStyle}");
			char charMaxButton = DrawingSymbols.GetSymbolValue("ArrowUp");
			char charTLCorner = DrawingSymbols.GetSymbolValue(
				$"BoxTopLeftCorner{mBoxBorderStyle}");
			char charTRCorner = DrawingSymbols.GetSymbolValue(
				$"BoxTopRightCorner{mBoxBorderStyle}");
			char charVLine = DrawingSymbols.GetSymbolValue(
				$"BoxVerticalLine{mBoxBorderStyle}");
			char[] chars = null;
			int colCount = 0;
			int colEnd = 0;
			int colIndex = 0;
			int colStart = 0;
			int rowCount = 0;
			int rowEnd = 0;
			int rowIndex = 0;
			int textLength = 0;
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
					textLength = Math.Min(colCount - 14, title.Length);
					if(title.Length > textLength)
					{
						title = Right(title, textLength);
					}
				}

				//	Assign colors from loaded styles. If a style was not found,
				//	use the shape's colors.
				if(screenBuffer.AssignColorFromStyle(
					mBorderColorStyle, mBorderColorStyleName) == null)
				{
					ColorInfo.TransferValues(this, mBorderColorStyle);
				}
				if(screenBuffer.AssignColorFromStyle(
					mWindowButtonColorStyle, mWindowButtonColorStyleName) == null)
				{
					ColorInfo.TransferValues(this, mWindowButtonColorStyle);
				}
				if(screenBuffer.AssignColorFromStyle(
					mListBoxColorStyle, mListBoxColorStyleName) == null)
				{
					ColorInfo.TransferValues(this, mListBoxColorStyle);
				}
				if(screenBuffer.AssignColorFromStyle(
					mListBoxHeaderColorStyle, mListBoxHeaderColorStyleName) == null)
				{
					ColorInfo.TransferValues(this, mListBoxHeaderColorStyle);
				}
				if(screenBuffer.AssignColorFromStyle(
					mListBoxHighlightColorStyle, mListBoxHighlightColorStyleName) ==
					null)
				{
					ColorInfo.TransferValues(this, mListBoxHighlightColorStyle);
				}
				if(screenBuffer.AssignColorFromStyle(
					mTitleColorStyle, mTitleColorStyleName) == null)
				{
					ColorInfo.TransferValues(this, mTitleColorStyle);
				}

				//	Draw the border box.
				screenBuffer.ClearCharacterWindow(mCharacterWindow,
					ForeColor, BackColor);
				colEnd = colCount - 1;
				rowEnd = rowCount - 1;
				SetCharacter(0, 0, charTLCorner, mBorderColorStyle);
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					SetCharacter(colIndex, 0, charHLine, mBorderColorStyle);
				}
				SetCharacter(colIndex, 0, charTRCorner, mBorderColorStyle);
				for(rowIndex = 1; rowIndex < rowEnd; rowIndex++)
				{
					SetCharacter(0, rowIndex, charVLine, mBorderColorStyle);
					SetCharacter(colEnd, rowIndex, charVLine, mBorderColorStyle);
				}
				SetCharacter(0, rowIndex, charBLCorner, mBorderColorStyle);
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					SetCharacter(colIndex, rowIndex, charHLine, mBorderColorStyle);
				}
				SetCharacter(colIndex, rowIndex, charBRCorner, mBorderColorStyle);

				//	Draw the window buttons.
				if(colCount > 7)
				{
					//	Window close.
					SetCharacter(2, 0, '[', mBorderColorStyle);
					SetCharacter(3, 0, charCloseButton, mWindowButtonColorStyle);
					SetCharacter(4, 0, ']', mBorderColorStyle);
					if(colCount > 11)
					{
						//	Window maximize.
						SetCharacter(colCount - 5, 0, '[', mBorderColorStyle);
						SetCharacter(colCount - 4, 0,
							charMaxButton, mWindowButtonColorStyle);
						SetCharacter(colCount - 3, 0, ']', mBorderColorStyle);
					}
				}

				//	Draw the title.
				if(title.Length > 0 && colCount > 14)
				{
					colStart = Math.Max(7, (colCount / 2) - (title.Length / 2));
					colEnd = colCount - 6 - 1;
					chars = title.ToCharArray();
					for(colIndex = 6; colIndex < colStart; colIndex ++)
					{
						SetCharacter(colIndex, 0, ' ', mTitleColorStyle);
					}
					foreach(char charItem in chars)
					{
						SetCharacter(colIndex, 0,
							GetPrintable(charItem), mTitleColorStyle);
						colIndex++;
						if(colIndex >= colEnd)
						{
							break;
						}
					}
					for(; colIndex <= colEnd; colIndex ++)
					{
						SetCharacter(colIndex, 0, ' ', mTitleColorStyle);
					}
				}
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
				bool bChanged = mTitle != value;
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

		//*-----------------------------------------------------------------------*
		//*	TitleColorStyle																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TitleColorStyle">TitleColorStyle</see>.
		/// </summary>
		private CharacterStyle mTitleColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the title color style for this object.
		/// </summary>
		public CharacterStyle TitleColorStyle
		{
			get { return mTitleColorStyle; }
			set
			{
				bool bChanged = (mTitleColorStyle != value);
				mTitleColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TitleColorStyleName																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TitleColorStyleName">
		/// TitleColorStyleName
		/// </see>.
		/// </summary>
		private string mTitleColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the title style for this object.
		/// </summary>
		public string TitleColorStyleName
		{
			get { return mTitleColorStyleName; }
			set
			{
				bool bChanged = (mTitleColorStyleName != value);
				mTitleColorStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WindowButtonColorStyle																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ButtonColorStyle">ButtonColorStyle</see>.
		/// </summary>
		private CharacterStyle mWindowButtonColorStyle = new CharacterStyle();
		/// <summary>
		/// Get/Set a reference to the button color style for this object.
		/// </summary>
		public CharacterStyle WindowButtonColorStyle
		{
			get { return mWindowButtonColorStyle; }
			set
			{
				bool bChanged = (mWindowButtonColorStyle != value);
				mWindowButtonColorStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WindowButtonColorStyleName																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="WindowButtonColorStyleName">
		/// WindowButtonColorStyleName
		/// </see>.
		/// </summary>
		private string mWindowButtonColorStyleName = "";
		/// <summary>
		/// Get/Set the name of the button color style.
		/// </summary>
		public string WindowButtonColorStyleName
		{
			get { return mWindowButtonColorStyleName; }
			set
			{
				bool bChanged = (mWindowButtonColorStyleName != value);
				mWindowButtonColorStyleName = value;
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
