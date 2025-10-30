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

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib.Internal
{
	//*-------------------------------------------------------------------------*
	//* SafeChangeObjectCollection																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Change object collection that participates in the singleton resource
	/// lock for competitive resources.
	/// </summary>
	/// <typeparam name="T">
	/// Any type for which change handling will be configured.
	/// </typeparam>
	public class SafeChangeObjectCollection<T> : ChangeObjectCollection<T>
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
		//* Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an item to the collection.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be added.
		/// </param>
		public new void Add(T item)
		{
			lock(ResourceLock)
			{
				base.Add(item);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a range of items to the collection.
		/// </summary>
		/// <param name="collection">
		/// Reference to the collection of new items to add to this collection.
		/// </param>
		public new void AddRange(IEnumerable<T> collection)
		{
			lock(ResourceLock)
			{
				base.AddRange(collection);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all of the elements from the collection.
		/// </summary>
		public new void Clear()
		{
			lock(ResourceLock)
			{
				base.Clear();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Insert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert an item into the collection at the specified ordinal index.
		/// </summary>
		/// <param name="index">
		/// Index at which to insert the item.
		/// </param>
		/// <param name="item">
		/// Reference to the item to be inserted.
		/// </param>
		public new void Insert(int index, T item)
		{
			lock(ResourceLock)
			{
				base.Insert(index, item);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertRange																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a range of items at the specified ordinal index.
		/// </summary>
		/// <param name="index">
		/// Index at which the range will be inserted.
		/// </param>
		/// <param name="collection">
		/// Reference to a collection of items to insert into this collection.
		/// </param>
		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			lock(ResourceLock)
			{
				base.InsertRange(index, collection);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the first matching instance of the specified item from the
		/// collection.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be removed.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified item was removed from the
		/// collection.
		/// </returns>
		public new bool Remove(T item)
		{
			lock(ResourceLock)
			{
				return base.Remove(item);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAll																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all items matching the condition from the collection.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be found.
		/// </param>
		/// <returns>
		/// Count of items removed.
		/// </returns>
		public new int RemoveAll(Predicate<T> match)
		{
			lock(ResourceLock)
			{
				return base.RemoveAll(match);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAt																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the item at the specified ordinal index of the collection.
		/// </summary>
		/// <param name="index">
		/// Ordinal index of the item to be removed.
		/// </param>
		public new void RemoveAt(int index)
		{
			lock(ResourceLock)
			{
				base.RemoveAt(index);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveRange																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove a range of items from the collection.
		/// </summary>
		/// <param name="index">
		/// Index at which to remove the items.
		/// </param>
		/// <param name="count">
		/// Count of items to remove.
		/// </param>
		public new void RemoveRange(int index, int count)
		{
			lock(ResourceLock)
			{
				base.RemoveRange(index, count);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Reverse																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Reverse the order of elements in the list.
		/// </summary>
		public new void Reverse()
		{
			lock(ResourceLock)
			{
				base.Reverse();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Sort																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Sort elements in the list using the default comparer.
		/// </summary>
		public new void Sort()
		{
			lock(ResourceLock)
			{
				base.Sort();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TrimExcess																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the capacity to the actual number of elements.
		/// </summary>
		public new void TrimExcess()
		{
			lock(ResourceLock)
			{
				base.TrimExcess();
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
