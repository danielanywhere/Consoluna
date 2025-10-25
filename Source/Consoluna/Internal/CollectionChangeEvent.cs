/*
 * Copyright (c). 2000 - 2025 Daniel Patterson, MCSD (danielanywhere).
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
using System.Text;

namespace ConsolunaLib.Internal
{
	//*-------------------------------------------------------------------------*
	//*	CollectionChangeEventArgs																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// General collection change event arguments.
	/// </summary>
	/// <typeparam name="T">
	/// Type of item handled by this collection.
	/// </typeparam>
	public class CollectionChangeEventArgs<T>
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
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		public CollectionChangeEventArgs()
		{
			mAffectedItems = new List<T>();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		/// <param name="affectedItems">
		/// Reference to a collection of affected items for which this event was
		/// raised.
		/// </param>
		public CollectionChangeEventArgs(List<T> affectedItems)
		{
			if(affectedItems != null)
			{
				mAffectedItems = affectedItems;
			}
			else
			{
				mAffectedItems = new List<T>();
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		/// <param name="affectedItem">
		/// Reference to an affected item for which this event was raised.
		/// </param>
		public CollectionChangeEventArgs(T affectedItem) : this()
		{
			if(affectedItem != null)
			{
				mAffectedItems.Add(affectedItem);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		/// <param name="actionName">
		/// Name of the action taken.
		/// </param>
		public CollectionChangeEventArgs(string actionName) : this()
		{
			if(actionName?.Length > 0)
			{
				mActionName = actionName;
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		/// <param name="actionName">
		/// Name of the action taken.
		/// </param>
		/// <param name="affectedItems">
		/// Reference to a collection of affected items for which this event was
		/// raised.
		/// </param>
		public CollectionChangeEventArgs(string actionName, List<T> affectedItems)
		{
			if(actionName?.Length > 0)
			{
				mActionName = actionName;
			}
			if(affectedItems != null)
			{
				mAffectedItems = affectedItems;
			}
			else
			{
				mAffectedItems = new List<T>();
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CollectionChangeEventArgs item.
		/// </summary>
		/// <param name="actionName">
		/// Name of the action taken.
		/// </param>
		/// <param name="affectedItem">
		/// Reference to an affected item for which this event was raised.
		/// </param>
		public CollectionChangeEventArgs(string actionName, T affectedItem) :
			this()
		{
			if(actionName?.Length > 0)
			{
				mActionName = actionName;
			}
			if(affectedItem != null)
			{
				mAffectedItems.Add(affectedItem);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ActionName																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ActionName">ActionName</see>.
		/// </summary>
		private string mActionName = "";
		/// <summary>
		/// Get/Set the name of the action.
		/// </summary>
		public string ActionName
		{
			get { return mActionName; }
			set { mActionName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AffectedItems																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AffectedItems">AffectedItems</see>.
		/// </summary>
		private List<T> mAffectedItems = null;
		/// <summary>
		/// Get/Set a reference to the collection of items affected by this
		/// event.
		/// </summary>
		public List<T> AffectedItems
		{
			get { return mAffectedItems; }
			set { mAffectedItems = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Handled																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Handled">Handled</see>.
		/// </summary>
		private bool mHandled = false;
		/// <summary>
		/// Get/Set a value indicating whether this change has been handled.
		/// </summary>
		public bool Handled
		{
			get { return mHandled; }
			set { mHandled = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PropertyName																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PropertyName">PropertyName</see>.
		/// </summary>
		private string mPropertyName = "";
		/// <summary>
		/// Get/Set the name of the property affected on the item.
		/// </summary>
		public string PropertyName
		{
			get { return mPropertyName; }
			set { mPropertyName = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
