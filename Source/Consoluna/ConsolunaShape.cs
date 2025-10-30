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
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ConsolunaLib.Internal;

using static ConsolunaLib.ConsolunaUtil;

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
		/// <summary>
		/// The position at the last render.
		/// </summary>
		private ConsolunaPosition mLastPosition = null;
		/// <summary>
		/// The size at the last render.
		/// </summary>
		private ConsolunaSize mLastSize = null;

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
		//* mShortcutBackColor_PropertyChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The shortcut background color property has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mShortcutBackColor_PropertyChanged(object sender,
			ConsolunaPropertyChangeEventArgs e)
		{
			OnPropertyChanged("ShortcutBackColor");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mShortcutForeColor_PropertyChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The shortcut foreground color property has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Console property change event arguments.
		/// </param>
		private void mShortcutForeColor_PropertyChanged(object sender,
			ConsolunaPropertyChangeEventArgs e)
		{
			OnPropertyChanged("ShortcutForeColor");
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
		/// <summary>
		/// <para>
		/// Pre-sized window of target character references allow the inherited
		/// class to apply changes directly to a relative area the size of this
		/// item's box.
		/// </para>
		/// <para>
		/// If a shadow is active, the width and height of the array are each one
		/// greater than the base dimension.
		/// </para>
		/// <para>
		/// This area is called during the base.Render call from any inherited
		/// class.
		/// </para>
		/// </summary>
		protected ConsolunaCharacterItem[,] mCharacterWindow =
			new ConsolunaCharacterItem[0, 0];

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
		public ConsolunaShapeItem(string name, int x = 0, int y = 0,
			int width = 1, int height = 1) : this()
		{
			if(name?.Length > 0)
			{
				mName = name;
			}
			mPosition.X = x;
			mPosition.Y = y;
			mSize.Width = width;
			mSize.Height = height;
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
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Name">Name</see>.
		/// </summary>
		private string mName = "";
		/// <summary>
		/// Get/Set the name of this shape.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
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
		public virtual void Render(Consoluna screenBuffer)
		{
			ConsolunaCharacterItem character = null;
			int colCount = 0;
			int colIndex = 0;
			int rowCount = 0;
			int rowIndex = 0;
			string styleNameLower = "";

			if(screenBuffer != null && mPosition != null &&
				ConsolunaSize.HasVolume(mSize))
			{
				//	Update the local styling properties from explicit style names.
				if(mStyleName?.Length > 0)
				{
					styleNameLower = mStyleName.ToLower();
					mStyleItem = screenBuffer.Styles.FirstOrDefault(x =>
						x.Name.ToLower() == styleNameLower);
					if(mStyleItem != null)
					{
						if(mStyleItem.BackColor != null)
						{
							mBackColor = mStyleItem.BackColor;
						}
						if(mStyleItem.ForeColor != null)
						{
							mForeColor = mStyleItem.ForeColor;
						}
					}
				}
				if(mShortcutStyleName?.Length > 0)
				{
					styleNameLower = mShortcutStyleName.ToLower();
					mShortcutStyleItem = screenBuffer.Styles.FirstOrDefault(x =>
						x.Name.ToLower() == styleNameLower);
					if(mShortcutStyleItem != null)
					{
						if(mShortcutStyleItem.BackColor != null)
						{
							mShortcutBackColor = mShortcutStyleItem.BackColor;
						}
						if(mShortcutStyleItem.ForeColor != null)
						{
							mShortcutForeColor = mShortcutStyleItem.ForeColor;
						}
					}
				}
				//	Any inherited class can use this window.
				if(!mShadow)
				{
					mCharacterWindow = screenBuffer.GetCharactersInArea(
						mPosition.X, mPosition.Y, mSize.Width, mSize.Height);
				}

				colCount = mCharacterWindow.GetLength(0);
				rowCount = mCharacterWindow.GetLength(1);

				if(mVisible)
				{
					//	Draw shadow.
					if(mShadow)
					{
						mCharacterWindow = screenBuffer.GetCharactersInArea(
							mPosition.X + mSize.Width,
							mPosition.Y + 1,
							1,
							mSize.Height);
						foreach(ConsolunaCharacterItem characterItem in mCharacterWindow)
						{
							characterItem.Shadowed = true;
						}
						mCharacterWindow = screenBuffer.GetCharactersInArea(
							mPosition.X + 1,
							mPosition.Y + mSize.Height,
							mSize.Width - 1,
							1);
						foreach(ConsolunaCharacterItem characterItem in mCharacterWindow)
						{
							characterItem.Shadowed = true;
						}
						mCharacterWindow = screenBuffer.GetCharactersInArea(
							mPosition.X, mPosition.Y, mSize.Width, mSize.Height);
					}
				}
				else if(mDirty && mLastPosition != null &&
					ConsolunaSize.HasVolume(mLastSize))
				{
					//	Erase this level after going invisible.
					screenBuffer.SetDirty(mCharacterWindow);
				}
			}
			else
			{
				mCharacterWindow = new ConsolunaCharacterItem[0, 0];
			}
			mLastPosition = mPosition;
			mLastSize = mSize;
			mDirty = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Shadow																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Shadow">Shadow</see>.
		/// </summary>
		private bool mShadow = false;
		/// <summary>
		/// Get/Set a value indicating whether this shape throws a shadow.
		/// </summary>
		public bool Shadow
		{
			get { return mShadow; }
			set { mShadow = value; }
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
		//*	ShortcutBackColor																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ShortcutBackColor">ShortcutBackColor</see>.
		/// </summary>
		private ConsolunaColor mShortcutBackColor = null;
		/// <summary>
		/// Get/Set a reference to the background color for shortcut chaaracters
		/// on this shape.
		/// </summary>
		public ConsolunaColor ShortcutBackColor
		{
			get { return mShortcutBackColor; }
			set
			{
				bool bChanged = (mShortcutBackColor != value);
				if(bChanged && mShortcutBackColor != null)
				{
					mShortcutBackColor.PropertyChanged -=
						mShortcutBackColor_PropertyChanged;
				}
				mShortcutBackColor = value;
				if(bChanged)
				{
					if(mShortcutBackColor != null)
					{
						mShortcutBackColor.PropertyChanged +=
							mShortcutBackColor_PropertyChanged;
					}
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShortcutForeColor																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ShortcutForeColor">ShortcutForeColor</see>.
		/// </summary>
		private ConsolunaColor mShortcutForeColor = null;
		/// <summary>
		/// Get/Set a reference to the foreground color for shortcut keys on this
		/// shape.
		/// </summary>
		public ConsolunaColor ShortcutForeColor
		{
			get { return mShortcutForeColor; }
			set
			{
				bool bChanged = (mShortcutForeColor != value);
				if(mShortcutForeColor != null)
				{
					mShortcutForeColor.PropertyChanged -=
						mShortcutForeColor_PropertyChanged;
				}
				mShortcutForeColor = value;
				if(bChanged)
				{
					if(mShortcutForeColor != null)
					{
						mShortcutForeColor.PropertyChanged +=
							mShortcutForeColor_PropertyChanged;
					}
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShortcutStyleName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ShortcutStyleName">ShortcutStyleName</see>.
		/// </summary>
		protected ConsolunaScreenStyleItem mShortcutStyleItem = null;
		/// <summary>
		/// Private member for <see cref="ShortcutStyleName">ShortcutStyleName</see>.
		/// </summary>
		private string mShortcutStyleName = "";
		/// <summary>
		/// Get/Set the name of the screen style to assign to shortcuts on this
		/// object.
		/// </summary>
		public string ShortcutStyleName
		{
			get { return mShortcutStyleName; }
			set
			{
				bool bChanged = (mShortcutStyleName != value);
				mShortcutStyleName = value;
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

		//*-----------------------------------------------------------------------*
		//*	StyleName																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StyleName">StyleName</see>.
		/// </summary>
		protected ConsolunaScreenStyleItem mStyleItem = null;
		/// <summary>
		/// Private member for <see cref="StyleName">StyleName</see>.
		/// </summary>
		private string mStyleName = "";
		/// <summary>
		/// Get/Set the name of the screen style to assign to this object.
		/// </summary>
		public string StyleName
		{
			get { return mStyleName; }
			set
			{
				bool bChanged = (mStyleName != value);
				mStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Visible																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Visible">Visible</see>.
		/// </summary>
		private bool mVisible = true;
		/// <summary>
		/// Get/Set a value indicating whether this shape is visible.
		/// </summary>
		public bool Visible
		{
			get { return mVisible; }
			set
			{
				bool bChanged = (mVisible != value);
				mVisible = value;
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
	//*	SafeConsolunaShapeCollection																						*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ConsolunaShapeItem Items.
	/// </summary>
	public class SafeConsolunaShapeCollection :
		SafeChangeObjectCollection<ConsolunaShapeItem>
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
