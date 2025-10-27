using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaTokenCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ConsolunaTokenItem Items.
	/// </summary>
	public class ConsolunaTokenCollection : List<ConsolunaTokenItem>
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
		//* ParseWords																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the caller's text into a collection of words.
		/// </summary>
		/// <param name="text">
		/// The text to parse.
		/// </param>
		/// <returns>
		/// Reference to a collection of word tokens.
		/// </returns>
		public static ConsolunaTokenCollection ParseWords(string text)
		{
			bool bFound = false;
			MatchCollection matches = null;
			int matchIndex = 0;
			string matchValue = "";
			ConsolunaTokenCollection result = new ConsolunaTokenCollection();
			List<(string memberName, ConsolunaTokenType tokenType)> tests =
				new List<(string memberName, ConsolunaTokenType tokenType)>()
			{
					("text", ConsolunaTokenType.Text),
					("whitespace", ConsolunaTokenType.Whitespace),
					("punctuation", ConsolunaTokenType.Punctuation)
			};

			if(text?.Length > 0)
			{
				matches = Regex.Matches(text, ResourceMain.rxWordToken);
				foreach(Match matchItem in matches)
				{
					foreach((string memberName, ConsolunaTokenType tokenType) testItem
						in tests)
					{
						matchValue = GetValue(matchItem, testItem.memberName);
						if(matchValue.Length > 0)
						{
							matchIndex = GetIndex(matchItem, testItem.memberName);
							result.Add(new ConsolunaTokenItem()
							{
								Index = matchIndex,
								TokenType = testItem.tokenType,
								Value = matchValue
							});
							break;
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this collection.
		/// </summary>
		/// <returns>
		/// The string representation of the contents of this collection.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach(ConsolunaTokenItem tokenItem in this)
			{
				builder.Append(tokenItem.Value);
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	ConsolunaTokenItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a single token in a sequence.
	/// </summary>
	public class ConsolunaTokenItem
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
		//*	Index																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Index">Index</see>.
		/// </summary>
		private int mIndex = 0;
		/// <summary>
		/// Get/Set the index of the word.
		/// </summary>
		public int Index
		{
			get { return mIndex; }
			set { mIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TokenType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TokenType">TokenType</see>.
		/// </summary>
		private ConsolunaTokenType mTokenType = ConsolunaTokenType.None;
		/// <summary>
		/// Get/Set the token type for this item.
		/// </summary>
		public ConsolunaTokenType TokenType
		{
			get { return mTokenType; }
			set { mTokenType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Value">Value</see>.
		/// </summary>
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of the token.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
