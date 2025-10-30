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
using ConsolunaLib.Events;
using ConsolunaLib.Internal;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	CharacterCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CharacterItem Items.
	/// </summary>
	public class CharacterCollection :
		ChangeObjectCollection<CharacterItem>
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

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CharacterItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Console display information about an individual character.
	/// </summary>
	public class CharacterItem : ChangeObjectItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* mBackColor_PropertyChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The background color has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mBackColor_PropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnPropertyChanged("BackColor");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mForeColor_PropertyChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The foreground color has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mForeColor_PropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			OnPropertyChanged("ForeColor");
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnPropertyChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property change event arguments.
		/// </param>
		protected override void OnPropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			if(e?.PropertyName != "Dirty")
			{
				mDirty = true;
			}
			base.OnPropertyChanged(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the CharacterItem item.
		/// </summary>
		public CharacterItem()
		{
			mBackColor = new ColorInfo();
			mBackColor.PropertyChanged += mBackColor_PropertyChanged;
			mForeColor = new ColorInfo()
			{
				Red = 0x7f,
				Green = 0x7f,
				Blue = 0x7f
			};
			mForeColor.PropertyChanged += mForeColor_PropertyChanged;
		}
		//*-----------------------------------------------------------------------*

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
				bool bChanged = (mBackColor != value);
				if(bChanged && mBackColor != null)
				{
					mBackColor.PropertyChanged -= mBackColor_PropertyChanged;
				}
				mBackColor = value;
				if(bChanged)
				{
					if(mBackColor != null)
					{
						mBackColor.PropertyChanged += mBackColor_PropertyChanged;
					}
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Character																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Character">Character</see>.
		/// </summary>
		private char mCharacter = char.MinValue;
		/// <summary>
		/// Get/Set the printable character for this position.
		/// </summary>
		public char Character
		{
			get { return mCharacter; }
			set
			{
				bool bChanged = (mCharacter != value);
				mCharacter = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
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
				bool bChanged = (mCharacterStyle != value);
				mCharacterStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Dirty																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Dirty">Dirty</see>.
		/// </summary>
		private bool mDirty = true;
		/// <summary>
		/// Get/Set a value indicating whether this item has changed since the last
		/// update.
		/// </summary>
		public bool Dirty
		{
			get { return mDirty; }
			set
			{
				bool bChanged = (mDirty != value);
				mDirty = value;
				if(bChanged && mDirty == true)
				{
					OnPropertyChanged();
				}
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
				bool bChanged = (mForeColor != value);
				if(mForeColor != null)
				{
					mForeColor.PropertyChanged -= mForeColor_PropertyChanged;
				}
				mForeColor = value;
				if(bChanged)
				{
					if(mForeColor != null)
					{
						mForeColor.PropertyChanged += mForeColor_PropertyChanged;
					}
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Shadowed																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Shadowed">Shadowed</see>.
		/// </summary>
		private bool mShadowed = false;
		/// <summary>
		/// Get/Set a value indicating whether this item is shadowed.
		/// </summary>
		public bool Shadowed
		{
			get { return mShadowed; }
			set
			{
				bool bChanged = (mShadowed != value);
				mShadowed = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	SafeCharacterCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CharacterItem Items.
	/// </summary>
	public class SafeCharacterCollection :
		SafeChangeObjectCollection<CharacterItem>
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

	}
	//*-------------------------------------------------------------------------*

}
