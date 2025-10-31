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
	//*	CharacterStyle																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Generic styling for one or more characters, typically of a given group.
	/// </summary>
	public class CharacterStyle : IForeBack
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
		//*	BackColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BackColor">BackColor</see>.
		/// </summary>
		private ColorInfo mBackColor = new ColorInfo();
		/// <summary>
		/// Get/Set a reference to the background color for this character.
		/// </summary>
		public ColorInfo BackColor
		{
			get { return mBackColor; }
			set
			{
				mBackColor = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StyleType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StyleType">StyleType</see>.
		/// </summary>
		private CharacterStyleType mStyleType =
			CharacterStyleType.Normal;
		/// <summary>
		/// Get/Set the current character style.
		/// </summary>
		public CharacterStyleType StyleType
		{
			get { return mStyleType; }
			set { mStyleType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ForeColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ForeColor">ForeColor</see>.
		/// </summary>
		private ColorInfo mForeColor = new ColorInfo();
		/// <summary>
		/// Get/Set a reference to the foreground color for this character.
		/// </summary>
		public ColorInfo ForeColor
		{
			get { return mForeColor; }
			set
			{
				mForeColor = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Properties																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Properties">Properties</see>.
		/// </summary>
		private PropertyCollection mProperties =
			new PropertyCollection();
		/// <summary>
		/// Get a reference to the collection of custom properties for this style.
		/// </summary>
		public PropertyCollection Properties
		{
			get { return mProperties; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
