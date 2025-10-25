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
using System.Threading.Tasks;

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
			int height = 0;
			int hotKeyIndex = 0;
			int width = 0;

			base.Render(screenBuffer);
			if(screenBuffer != null)
			{
				width = screenBuffer.Width;
				height = screenBuffer.Height;
				hotKeyIndex = mText.IndexOf('_');
			}
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


	}
	//*-------------------------------------------------------------------------*

}
