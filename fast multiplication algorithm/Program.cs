using System;

namespace fast_multiplication_algorithm
{
	class Program
	{
		static void Main(string[] args)
		{
#if DEBUG
			const bool check = true;
			if (check)
			{
				//check if the algorithm works by multiplying 2 small numbers(int)
				ulong a = 0x1234567;
				ulong b = 0x65432;
				//Calculation A * B with the Andrea Bruno algorithm
				var ab = Multiplication(BitConverter.GetBytes(a), BitConverter.GetBytes(b));
				var hex = BitConverter.ToString(ab);
				//Standard calculation A* B
				var ab2 = BitConverter.GetBytes(a * b);
				var hex2 = BitConverter.ToString(ab2);
				//Compare the two the two algorithms and find the differences
				for (var i = 0; i < ab2.Length; i++)
				{
					//If it enters here then the algorithm is wrong
					if (ab2[i] != ab[i])
					{
						Console.WriteLine("The Algotithm does not work!");
						return;
					}
				}
				//The algorithm works
			}
#endif

			const int byteSize = 1000; //Number of 256th digits, for multiplication
			var multiplicand = GetRandomBigNumber(byteSize);
			var multiplicandText = BitConverter.ToString(multiplicand);
			Console.WriteLine("multiplicand=" + multiplicandText);
			var multiplier = GetRandomBigNumber(byteSize);
			var multiplierText = BitConverter.ToString(multiplier);
			Console.WriteLine("multiplier=" + multiplierText);
			DateTime startTime = DateTime.UtcNow;
			var result = Multiplication(multiplicand, multiplier);
			var sec = (DateTime.UtcNow - startTime).TotalSeconds;
			Console.WriteLine("result=" + BitConverter.ToString(result));
			Console.WriteLine("Calculation: " + sec + " Sec.");
#if DEBUG
			Console.WriteLine("Please, launch it in release mode for better performance!");
#endif
			Console.WriteLine("© Andrea Bruno");
		}

		public static byte[] Multiplication(byte[] multiplicand, byte[] multiplier)
		{
			/*                         --- Private license with royalty ---
			        The intellectual property of this algorithm is owned by Andrea Bruno. Use is not allowed.
							The code is released according to the following terms and conditions: For use, royalties
							must be paid.The date of publication on github, shows the authenticity of the algorithm.
							Any fork must comply with this license.For those wishing to use this algorithm, contact
							the author: andrea_bruno @hotmail.com
			*/
			//============ initialization
			const int word = 8;
			if (multiplicand.Length < multiplier.Length)
			{
				var tmp = multiplicand;
				multiplicand = multiplier;
				multiplier = tmp;
			}
			var result = new byte[multiplicand.Length + multiplier.Length];
			var o = new int[multiplier.Length * 8];
			var ad = 0;
			var r = 0;
			//============ end initialization

			for (var p = 0; p < result.Length * word; p++)
			{
				var by = p >> 3;
				var bt = p & 7;
				var s = 0;
				if (p < multiplier.Length * word)
					if ((multiplier[by] & 1 << bt) != 0)
					{
						o[ad] = p;
						ad++;
					}
				if (p < multiplicand.Length * 8)
					for (var i = 0; i < ad; i++)
						s += (multiplicand[(p - o[i]) >> 3] & (1 << ((p - o[i]) & 7))) != 0 ? 1 : 0;
				else if (r == 0)
					return result;
				r += s;
				result[by] |= (byte)((r & 1) << bt);
				r >>= 1;
			}
			return result;
		}

		static byte[] GetRandomBigNumber(int len)
		{
			var r = new Random();
			var big = new byte[len];
			r.NextBytes(big);
			return big;
		}

	}
}
