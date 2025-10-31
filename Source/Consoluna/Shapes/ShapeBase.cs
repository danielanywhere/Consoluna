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
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsolunaLib.Events;
using ConsolunaLib.Internal;
using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib.Shapes
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaShapeCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ShapeBase Items.
	/// </summary>
	public class ConsolunaShapeCollection :
		ChangeObjectCollection<ShapeBase>
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
	//*	ShapeBase																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a shape directive that will be rendered as a set of
	/// characters.
	/// </summary>
	public abstract class ShapeBase : ChangeObjectItem, IForeBack
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// The position at the last render.
		/// </summary>
		private PositionInfo mLastPosition = null;
		/// <summary>
		/// The size at the last render.
		/// </summary>
		private SizeInfo mLastSize = null;

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
			PropertyChangeEventArgs e)
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
			PropertyChangeEventArgs e)
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
			PropertyChangeEventArgs e)
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
			PropertyChangeEventArgs e)
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
		protected CharacterItem[,] mCharacterWindow =
			new CharacterItem[0, 0];

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
			if(mEventsActive)
			{
				if(e?.PropertyName != "Dirty")
				{
					mDirty = true;
				}
				base.OnPropertyChanged(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetCharacter																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the content and style of the character at the specified column and
		/// row.
		/// </summary>
		/// <param name="colIndex">
		/// 0-based index of the column containing the character to set.
		/// </param>
		/// <param name="rowIndex">
		/// 0-based index of the row containing the character to set.
		/// </param>
		/// <param name="symbol">
		/// The symbol to apply on the character.
		/// </param>
		/// <param name="style">
		/// The color style to apply on the character.
		/// </param>
		protected virtual void SetCharacter(int colIndex, int rowIndex,
			char symbol, IForeBack style)
		{
			CharacterItem character = null;

			if(colIndex > -1 && rowIndex > -1 &&
				colIndex < mCharacterWindow.GetLength(0) &&
				rowIndex < mCharacterWindow.GetLength(1))
			{
				character = mCharacterWindow[colIndex, rowIndex];
				character.Symbol = symbol;
				if(style != null)
				{
					character.BackColor = style.BackColor ?? BackColor;
					character.ForeColor = style.ForeColor ?? ForeColor;
				}
			}

		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the ShapeBase item.
		/// </summary>
		public ShapeBase()
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
			mPosition = new PositionInfo();
			mPosition.PropertyChanged += mPosition_PropertyChanged;
			mSize = new SizeInfo();
			mSize.PropertyChanged += mSize_PropertyChanged;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the TextShape item.
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
		public ShapeBase(string name, int x = 0, int y = 0,
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
		//* AfterRender																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Called after the shape has been rendered to the buffer.
		/// </summary>
		/// <param name="screenBuffer">
		/// Reference to the screen buffer.
		/// </param>
		public virtual void AfterRender(Consoluna screenBuffer)
		{
			if(mDirty)
			{
				mDirty = false;
			}
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
				bool bChanged = mBackColor != value;
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
				bool bChanged = mDirty != value;
				mDirty = value;
				if(bChanged && mDirty == true)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EventsActive																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EventsActive">EventsActive</see>.
		/// </summary>
		private bool mEventsActive = true;
		/// <summary>
		/// Get/Set a value indicating whether event processing is active on this
		/// object.
		/// </summary>
		public bool EventsActive
		{
			get { return mEventsActive; }
			set { mEventsActive = value; }
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
				bool bChanged = mForeColor != value;
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
		private PositionInfo mPosition = null;
		/// <summary>
		/// Get/Set a reference to this shape's current grid position.
		/// </summary>
		public PositionInfo Position
		{
			get { return mPosition; }
			set
			{
				bool bChanged = mPosition != value;
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
			int colCount = 0;
			int rowCount = 0;

			if(screenBuffer != null && mPosition != null &&
				SizeInfo.HasVolume(mSize))
			{
				//	Update the local styling properties from explicit style names.
				screenBuffer.AssignColorFromStyle(this, mStyleName);
				screenBuffer.AssignColorFromStyle(mShortcutStyle, mShortcutStyleName);
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
						foreach(CharacterItem characterItem in mCharacterWindow)
						{
							characterItem.Shadowed = true;
						}
						mCharacterWindow = screenBuffer.GetCharactersInArea(
							mPosition.X + 1,
							mPosition.Y + mSize.Height,
							mSize.Width - 1,
							1);
						foreach(CharacterItem characterItem in mCharacterWindow)
						{
							characterItem.Shadowed = true;
						}
						mCharacterWindow = screenBuffer.GetCharactersInArea(
							mPosition.X, mPosition.Y, mSize.Width, mSize.Height);
					}
				}
				else if(mDirty && mLastPosition != null &&
					SizeInfo.HasVolume(mLastSize))
				{
					//	Erase this level after going invisible.
					screenBuffer.SetDirty(mCharacterWindow);
				}
			}
			else
			{
				mCharacterWindow = new CharacterItem[0, 0];
			}
			mLastPosition = mPosition;
			mLastSize = mSize;
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
		private ShapeType mShapeType = ShapeType.None;
		/// <summary>
		///  Get/Set the type of shape to be displayed.
		/// </summary>
		public ShapeType ShapeType
		{
			get { return mShapeType; }
			set
			{
				bool bChanged = mShapeType != value;
				mShapeType = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	ShortcutBackColor																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="ShortcutBackColor">ShortcutBackColor</see>.
		///// </summary>
		//private ColorInfo mShortcutBackColor = null;
		///// <summary>
		///// Get/Set a reference to the background color for shortcut chaaracters
		///// on this shape.
		///// </summary>
		//public ColorInfo ShortcutBackColor
		//{
		//	get { return mShortcutBackColor; }
		//	set
		//	{
		//		bool bChanged = mShortcutBackColor != value;
		//		if(bChanged && mShortcutBackColor != null)
		//		{
		//			mShortcutBackColor.PropertyChanged -=
		//				mShortcutBackColor_PropertyChanged;
		//		}
		//		mShortcutBackColor = value;
		//		if(bChanged)
		//		{
		//			if(mShortcutBackColor != null)
		//			{
		//				mShortcutBackColor.PropertyChanged +=
		//					mShortcutBackColor_PropertyChanged;
		//			}
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	ShortcutForeColor																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="ShortcutForeColor">ShortcutForeColor</see>.
		///// </summary>
		//private ColorInfo mShortcutForeColor = null;
		///// <summary>
		///// Get/Set a reference to the foreground color for shortcut keys on this
		///// shape.
		///// </summary>
		//public ColorInfo ShortcutForeColor
		//{
		//	get { return mShortcutForeColor; }
		//	set
		//	{
		//		bool bChanged = mShortcutForeColor != value;
		//		if(mShortcutForeColor != null)
		//		{
		//			mShortcutForeColor.PropertyChanged -=
		//				mShortcutForeColor_PropertyChanged;
		//		}
		//		mShortcutForeColor = value;
		//		if(bChanged)
		//		{
		//			if(mShortcutForeColor != null)
		//			{
		//				mShortcutForeColor.PropertyChanged +=
		//					mShortcutForeColor_PropertyChanged;
		//			}
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShortcutStyle																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ShortcutStyle">ShortcutStyle</see>.
		/// </summary>
		private CharacterStyle mShortcutStyle = null;
		/// <summary>
		/// Get/Set a reference to the shortcut key style.
		/// </summary>
		public CharacterStyle ShortcutStyle
		{
			get { return mShortcutStyle; }
			set
			{
				bool bChanged = (mShortcutStyle != value);
				mShortcutStyle = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ShortcutStyleName																											*
		//*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="ShortcutStyleName">ShortcutStyleName</see>.
		///// </summary>
		//protected ScreenStyleItem mShortcutStyleItem = null;
		/// <summary>
		/// Private member for
		/// <see cref="ShortcutStyleName">ShortcutStyleName</see>.
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
				bool bChanged = mShortcutStyleName != value;
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
		private SizeInfo mSize = null;
		/// <summary>
		/// Get/Set a reference to the size of this shape.
		/// </summary>
		public SizeInfo Size
		{
			get { return mSize; }
			set
			{
				bool bChanged = mSize != value;
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
		///// <summary>
		///// Private member for <see cref="StyleName">StyleName</see>.
		///// </summary>
		//protected ScreenStyleItem mStyleItem = null;
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
				bool bChanged = mStyleName != value;
				mStyleName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StyleType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="CharacterStyleType">CharacterStyleType</see>.
		/// </summary>
		private CharacterStyleType mStyleType = CharacterStyleType.Normal;
		/// <summary>
		/// Get/Set the current character style.
		/// </summary>
		public CharacterStyleType StyleType
		{
			get { return mStyleType; }
			set
			{
				bool bChanged = mStyleType != value;
				mStyleType = value;
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
				bool bChanged = mVisible != value;
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
	/// Collection of ShapeBase Items.
	/// </summary>
	public class SafeConsolunaShapeCollection :
		SafeChangeObjectCollection<ShapeBase>
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
