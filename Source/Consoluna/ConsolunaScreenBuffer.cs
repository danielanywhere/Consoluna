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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaScreenBuffer																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Screen buffer and related configuration.
	/// </summary>
	public class ConsolunaScreenBuffer
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
		/// Create a new instance of the ConsolunaScreenBuffer item.
		/// </summary>
		public ConsolunaScreenBuffer()
		{
			//	Set the default styles.
			mStyles = new ConsolunaScreenStyleCollection();
			mStyles.AddRange(new ConsolunaScreenStyleItem[]
			{
				new ConsolunaScreenStyleItem("ScreenColor",
					foreColor: new ConsolunaColor("#b0aedd"),
					backColor: new ConsolunaColor("#0b0938")),
				new ConsolunaScreenStyleItem("MenuColor",
					foreColor: new ConsolunaColor("#0b0708"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("MenuHighlightColor",
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("MenuHotkeyColor",
					foreColor: new ConsolunaColor("#912120")),
				new ConsolunaScreenStyleItem("MenuPanelBorderColor",
					foreColor: new ConsolunaColor("#010101"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("DialogColor",
					foreColor: new ConsolunaColor("#010101"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("DialogBorderColor",
					foreColor: new ConsolunaColor("#fffeff"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("DialogTextBoxColor",
					foreColor: new ConsolunaColor("#f2fcff"),
					backColor: new ConsolunaColor("#0001ab")),
				new ConsolunaScreenStyleItem("DialogListBoxColor",
					foreColor: new ConsolunaColor("#002427"),
					backColor: new ConsolunaColor("#03b1ba")),
				new ConsolunaScreenStyleItem("DialogListBoxHighlightColor",
					foreColor: new ConsolunaColor("#eeffe1"),
					backColor: new ConsolunaColor("#01ae04")),
				new ConsolunaScreenStyleItem("ButtonColorDefault",
					foreColor: new ConsolunaColor("#5cffb1"),
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("ButtonColor",
					foreColor: new ConsolunaColor("#003300"),
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("WindowColor",
					foreColor: new ConsolunaColor("#002593"),
					backColor: new ConsolunaColor("#04b0b0")),
				new ConsolunaScreenStyleItem("WindowListBoxColor",
					foreColor: new ConsolunaColor("#bfffff"),
					backColor: new ConsolunaColor("#0300ac")),
				new ConsolunaScreenStyleItem("WindowListBoxHighlightColor",
					foreColor: new ConsolunaColor("#ffc3a1"),
					backColor: new ConsolunaColor("#ad0100")),
				new ConsolunaScreenStyleItem("WindowBorder",
					foreColor: new ConsolunaColor("#b2ffff"),
					backColor: new ConsolunaColor("#04b0b0")),
				new ConsolunaScreenStyleItem("WindowControl",
					foreColor: new ConsolunaColor("#46f95f"),
					backColor: new ConsolunaColor("#04b0b0"))
			});

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Characters																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Characters">Characters</see>.
		/// </summary>
		private ConsolunaCharacterCollection mCharacters =
			new ConsolunaCharacterCollection();
		/// <summary>
		/// Get a reference to the collection of characters in the buffer.
		/// </summary>
		public ConsolunaCharacterCollection Characters
		{
			get { return mCharacters; }
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
		/// Get/Set the logical height of the buffer.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
			set { mHeight = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Styles																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Styles">Styles</see>.
		/// </summary>
		private ConsolunaScreenStyleCollection mStyles = null;
		/// <summary>
		/// Get a reference to the collection of styles associated with this screen
		/// buffer.
		/// </summary>
		public ConsolunaScreenStyleCollection Styles
		{
			get { return mStyles; }
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
		/// Get/Set the logical width of the buffer.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
			set { mWidth = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
