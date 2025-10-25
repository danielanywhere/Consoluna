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

using ConsolunaLib.Internal;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaShapeCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ConsolunaShapeItem Items.
	/// </summary>
	public class ConsolunaShapeCollection :
		ChangeObjectCollection<ConsolunaShapeItem>
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
	//*	ConsolunaShapeItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a shape directive that will be rendered as a set of
	/// characters.
	/// </summary>
	public abstract class ConsolunaShapeItem : ChangeObjectItem
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
			ConsolunaPropertyChangeEventArgs e)
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
			ConsolunaPropertyChangeEventArgs e)
		{
			OnPropertyChanged("ForeColor");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mPosition_PropertyChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The position has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mPosition_PropertyChanged(object sender,
			ConsolunaPropertyChangeEventArgs e)
		{
			OnPropertyChanged("Position");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mSize_PropertyChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The size has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mSize_PropertyChanged(object sender,
			ConsolunaPropertyChangeEventArgs e)
		{
			OnPropertyChanged("Size");
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
			ConsolunaPropertyChangeEventArgs e)
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
		/// Create a new instance of the ConsolunaShapeItem item.
		/// </summary>
		public ConsolunaShapeItem()
		{
			mBackColor = new ConsolunaColor();
			mBackColor.PropertyChanged += mBackColor_PropertyChanged;
			mForeColor = new ConsolunaColor()
			{
				Red = 0x7f,
				Green = 0x7f,
				Blue = 0x7f
			};
			mForeColor.PropertyChanged += mForeColor_PropertyChanged;
			mPosition = new ConsolunaPosition();
			mPosition.PropertyChanged += mPosition_PropertyChanged;
			mSize = new ConsolunaSize();
			mSize.PropertyChanged += mSize_PropertyChanged;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BackColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BackColor">BackColor</see>.
		/// </summary>
		private ConsolunaColor mBackColor = null;
		/// <summary>
		/// Get/Set a reference to the background color for this character.
		/// </summary>
		public ConsolunaColor BackColor
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
		//*	CharacterStyleType																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="CharacterStyleType">CharacterStyleType</see>.
		/// </summary>
		private ConsolunaCharacterStyleTypeEnum mCharacterStyleType =
			ConsolunaCharacterStyleTypeEnum.Normal;
		/// <summary>
		/// Get/Set the current character style.
		/// </summary>
		public ConsolunaCharacterStyleTypeEnum CharacterStyleType
		{
			get { return mCharacterStyleType; }
			set
			{
				bool bChanged = (mCharacterStyleType != value);
				mCharacterStyleType = value;
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
		private ConsolunaColor mForeColor = null;
		/// <summary>
		/// Get/Set a reference to the foreground color for this character.
		/// </summary>
		public ConsolunaColor ForeColor
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
		//*	Position																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Position">Position</see>.
		/// </summary>
		private ConsolunaPosition mPosition = null;
		/// <summary>
		/// Get/Set a reference to this shape's current grid position.
		/// </summary>
		public ConsolunaPosition Position
		{
			get { return mPosition; }
			set
			{
				bool bChanged = (mPosition != value);
				if(bChanged && mPosition != null)
				{
					mPosition.PropertyChanged -= mPosition_PropertyChanged;
				}
				mPosition = value;
				if(bChanged)
				{
					if(mPosition != null)
					{
						mPosition.PropertyChanged += mPosition_PropertyChanged;
					}
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
		public virtual void Render(ConsolunaScreenBuffer screenBuffer)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShapeType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ShapeType">ShapeType</see>.
		/// </summary>
		private ConsolunaShapeType mShapeType = ConsolunaShapeType.None;
		/// <summary>
		///  Get/Set the type of shape to be displayed.
		/// </summary>
		public ConsolunaShapeType ShapeType
		{
			get { return mShapeType; }
			set
			{
				bool bChanged = (mShapeType != value);
				mShapeType = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Size																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Size">Size</see>.
		/// </summary>
		private ConsolunaSize mSize = null;
		/// <summary>
		/// Get/Set a reference to the size of this shape.
		/// </summary>
		public ConsolunaSize Size
		{
			get { return mSize; }
			set
			{
				bool bChanged = (mSize != value);
				if(mSize != null)
				{
					mSize.PropertyChanged -= mSize_PropertyChanged;
				}
				mSize = value;
				if(bChanged)
				{
					if(mSize != null)
					{
						mSize.PropertyChanged += mSize_PropertyChanged;
					}
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*



	}
	//*-------------------------------------------------------------------------*


}
