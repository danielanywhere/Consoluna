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

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ScreenStyleCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ScreenStyleItem Items.
	/// </summary>
	public class ScreenStyleCollection : List<ScreenStyleItem>
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
		//* SetProperty																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified property on the specified style.
		/// </summary>
		/// <param name="styleName">
		/// Name of the style to access.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to set.
		/// </param>
		/// <param name="propertyValue">
		/// Object to be written to the target property.
		/// </param>
		/// <param name="createStyle">
		/// Value indicating whether to create the style object if not found.
		/// </param>
		public void SetProperty(string styleName, string propertyName,
			object propertyValue, bool createStyle = true)
		{
			ScreenStyleItem style = null;
			string styleNameLower = "";

			if(styleName?.Length > 0 && propertyName?.Length > 0)
			{
				styleNameLower = styleName.ToLower();
				style = this.FirstOrDefault(x => x.Name.ToLower() == styleNameLower);
				if(style == null && createStyle)
				{
					style = new ScreenStyleItem()
					{
						Name = styleName
					};
					this.Add(style);
				}
				if(style != null)
				{
					SetPropertyByName(style, propertyName, propertyValue);
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	ScreenStyleItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual screen style.
	/// </summary>
	public class ScreenStyleItem
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
		/// Create a new instance of the ScreenStyleItem item.
		/// </summary>
		public ScreenStyleItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ScreenStyleItem item.
		/// </summary>
		/// <param name="name">
		/// Name of the style.
		/// </param>
		/// <param name="foreColor">
		/// Optional fore color. Default = null.
		/// </param>
		/// <param name="backColor">
		/// Optional background color. Default = null.
		/// </param>
		/// <param name="characterStyle">
		/// Optional character style. Default = Normal.
		/// </param>
		public ScreenStyleItem(string name,
			ColorInfo foreColor = null, ColorInfo backColor = null,
			CharacterStyleType characterStyle =
				CharacterStyleType.Normal,
			(string name, string value)[] propertyValues = null)
		{
			if(name?.Length > 0)
			{
				mName = name;
			}
			if(foreColor != null || backColor != null ||
				propertyValues?.Length > 0 ||
				(characterStyle != CharacterStyleType.None &&
				characterStyle != CharacterStyleType.Normal))
			{
				mBackColor = backColor;
				mForeColor = foreColor;
				mCharacterStyle = characterStyle;
			}

			if(propertyValues?.Length > 0)
			{
				foreach((string name, string value) nameValueItem in
					propertyValues)
				{
					mProperties.Add(
						nameValueItem.name, nameValueItem.value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	CharacterStyle																												*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="CharacterStyle">CharacterStyle</see>.
		///// </summary>
		//private CharacterStyle mCharacterStyle = null;
		///// <summary>
		///// Get/Set the character styling for this style.
		///// </summary>
		//public CharacterStyle CharacterStyle
		//{
		//	get { return mCharacterStyle; }
		//	set { mCharacterStyle = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BackColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BackColor">BackColor</see>.
		/// </summary>
		private ColorInfo mBackColor = null;
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
		//*	CharacterStyle																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="CharacterStyle">CharacterStyle</see>.
		/// </summary>
		private CharacterStyleType mCharacterStyle =
			CharacterStyleType.Normal;
		/// <summary>
		/// Get/Set the current character style.
		/// </summary>
		public CharacterStyleType CharacterStyle
		{
			get { return mCharacterStyle; }
			set
			{
				mCharacterStyle = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ForeColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ForeColor">ForeColor</see>.
		/// </summary>
		private ColorInfo mForeColor = null;
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
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Name">Name</see>.
		/// </summary>
		private string mName = "";
		/// <summary>
		/// Get/Set the name of the style.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
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
