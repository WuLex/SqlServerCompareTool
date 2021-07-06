using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace FishWebLib
{
	/// <summary>
	/// 实现字符串的“全字符匹配”，“区分大小写”的工具类
	/// </summary>
	public abstract class StringSearcher
	{
		/// <summary>
		/// 字符串是否匹配要搜索的关键词
		/// </summary>
		/// <param name="input">要测试匹配的字符串</param>
		/// <returns>是否匹配</returns>
		public virtual bool IsMatch(string input)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 根据要搜索的关键词，以及是否区分大小写，全字符匹配原则，创建一个StringSearcher的实现类
		/// </summary>
		/// <param name="searchWord">要搜索的关键词</param>
		/// <param name="wholeMatch">是否为全字符匹配原则</param>
		/// <param name="caseSensitive">是否区分大小写</param>
		/// <returns>StringSearcher的实现类</returns>
		public static StringSearcher GetStringSearcher(string searchWord, bool wholeMatch, bool caseSensitive)
		{
			if( wholeMatch )
				if( caseSensitive )
					return new StringSearcher_WholeMatch_Y_CaseSensitive_Y(searchWord);
				else
					return new StringSearcher_WholeMatch_Y_CaseSensitive_N(searchWord);
			else
				if( caseSensitive )
					return new StringSearcher_WholeMatch_N_CaseSensitive_Y(searchWord);
				else
					return new StringSearcher_WholeMatch_N_CaseSensitive_N(searchWord);
		}





		/// <summary>
		/// StringSearcher的实现类，支持“全字符匹配”，“区分大小写”的搜索方式
		/// </summary>
		public sealed class StringSearcher_WholeMatch_Y_CaseSensitive_Y : StringSearcher
		{
			private Regex m_regex;

			internal StringSearcher_WholeMatch_Y_CaseSensitive_Y(string searchWord)
			{
				m_regex = new Regex(string.Concat(@"\b", searchWord, @"\b"), RegexOptions.None);
			}

			/// <summary>
			/// 字符串是否匹配要搜索的关键词
			/// </summary>
			/// <param name="input">要测试匹配的字符串</param>
			/// <returns>是否匹配</returns>
			public override bool IsMatch(string input)
			{
				return m_regex.IsMatch(input);
			}
		}

		/// <summary>
		/// StringSearcher的实现类，支持“全字符匹配”的搜索方式
		/// </summary>
		public sealed class StringSearcher_WholeMatch_Y_CaseSensitive_N : StringSearcher
		{
			private Regex m_regex;

			internal StringSearcher_WholeMatch_Y_CaseSensitive_N(string searchWord)
			{
				m_regex = new Regex(string.Concat(@"\b", searchWord, @"\b"), RegexOptions.IgnoreCase);
			}

			/// <summary>
			/// 字符串是否匹配要搜索的关键词
			/// </summary>
			/// <param name="input">要测试匹配的字符串</param>
			/// <returns>是否匹配</returns>
			public override bool IsMatch(string input)
			{
				return m_regex.IsMatch(input);
			}
		}

		/// <summary>
		/// StringSearcher的实现类，不支持“全字符匹配”与“区分大小写”的搜索方式
		/// </summary>
		public sealed class StringSearcher_WholeMatch_N_CaseSensitive_N : StringSearcher
		{
			private string m_searchWord;
			internal StringSearcher_WholeMatch_N_CaseSensitive_N(string searchWord)
			{
				m_searchWord = searchWord;
			}

			/// <summary>
			/// 字符串是否匹配要搜索的关键词
			/// </summary>
			/// <param name="input">要测试匹配的字符串</param>
			/// <returns>是否匹配</returns>
			public override bool IsMatch(string input)
			{
				return input.IndexOf(m_searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
			}
		}

		/// <summary>
		/// StringSearcher的实现类，支持“区分大小写”的搜索方式
		/// </summary>
		public sealed class StringSearcher_WholeMatch_N_CaseSensitive_Y : StringSearcher
		{
			private string m_searchWord;
			internal StringSearcher_WholeMatch_N_CaseSensitive_Y(string searchWord)
			{
				m_searchWord = searchWord;
			}

			/// <summary>
			/// 字符串是否匹配要搜索的关键词
			/// </summary>
			/// <param name="input">要测试匹配的字符串</param>
			/// <returns>是否匹配</returns>
			public override bool IsMatch(string input)
			{
				return input.IndexOf(m_searchWord, StringComparison.Ordinal) >= 0;
			}
		}
	}
}
