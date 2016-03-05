using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;

using DictionaryHash = System.Collections.Generic.Dictionary<string, System.IO.FileInfo>;

namespace Imgr.Models
{
	public class FileNameHasher
	{
		private string _RootPath = "";
		private int _HashLength = 4;
		private DictionaryHash _HashTable = null;

		#region Constructors

		public FileNameHasher(string path, int hashLength)
		{
			this._RootPath = path;
			this._HashLength = hashLength;

			try
			{
				this._HashTable = FileInfoHash(FileInfo(this._RootPath));
			}
			catch(ArgumentException arg)
			{
				this._HashTable = new DictionaryHash();
			}
		}

		#endregion

		#region Properties

		public Dictionary<string, FileInfo> FileNameHash
		{
			get
			{
				return this._HashTable;
			}
		}

		#endregion

		#region Functions

		private List<FileInfo> FileInfo(string root)
		{
			var value = new List<FileInfo>();
			var directory = (new DirectoryInfo(root));

			// add files in the current directory
			value
				.AddRange(directory
							.EnumerateFiles()
							.Cast<FileInfo>());

			// iterate all directories and add files in that directory
			value
				.AddRange(directory
							.EnumerateDirectories()
							.AsParallel()
							.SelectMany(item => FileInfo(item.FullName)));

			return value;
		}

		private DictionaryHash FileInfoHash(List<FileInfo> fileInfos)
		{
			var algorithm = (HashAlgorithm) MD5.Create();

			return
				fileInfos
					.Select(item => new
					{
						Hash = ComputeHash(ref algorithm, item.FullName),
						FileInfo = item,
					})
					.ToDictionary(item => item.Hash.Substring(0, 4),
								  item => item.FileInfo);

		}

		private IEnumerable<char> ConvertToBase62(BigInteger number)
		{
			const string SYMBOLS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

			do
			{
				var index = (int) (number % SYMBOLS.Length);
				yield return SYMBOLS[index];
				number /= SYMBOLS.Length;
			}
			while(number > 0);
		}

		private byte[] GetBytes(string text)
		{
			byte[] bytes = new byte[text.Length * sizeof(char)];
			System.Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		private string ComputeHash(ref HashAlgorithm algorith, string text)
		{
			var bytes = GetBytes(text);
			var hashCode = algorith.ComputeHash(bytes);
			var hashNumber = String.Join("", hashCode.Select(item => String.Format("{0}", item)));
			var number = BigInteger.Abs(BigInteger.Parse(hashNumber));
			var digits = (new String(ConvertToBase62(number).ToArray()));

			return digits;
		}

		#endregion
	}
}