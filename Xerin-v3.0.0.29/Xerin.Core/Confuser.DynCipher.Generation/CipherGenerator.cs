using System.Collections.Generic;
using System.Linq;
using Confuser.DynCipher.AST;
using Confuser.DynCipher.Elements;
using Confuser.DynCipher.Transforms;
using XCore.Generator;

namespace Confuser.DynCipher.Generation;

public class CipherGenerator
{
	private const int NUMOP_RATIO = 10;

	private const int BINOP_RATIO = 9;

	private const int RATIO_SUM = 35;

	private static void Shuffle<T>(RandomGenerator random, IList<T> arr)
	{
		for (int i = 1; i < arr.Count; i++)
		{
			int index = random.NextInt32(i + 1);
			T value = arr[i];
			arr[i] = arr[index];
			arr[index] = value;
		}
	}

	private static void PostProcessStatements(StatementBlock block, RandomGenerator random)
	{
		MulToShiftTransform.Run(block);
		NormalizeBinOpTransform.Run(block);
		ExpansionTransform.Run(block);
		ShuffleTransform.Run(block, random);
		ConvertVariables.Run(block);
	}

	public static void GeneratePair(RandomGenerator random, out StatementBlock encrypt, out StatementBlock decrypt)
	{
		double num = 1.0 + (random.NextDouble() * 2.0 - 1.0) * 0.2;
		int num2 = (int)((random.NextDouble() + 1.0) * 35.0 * num);
		List<CryptoElement> list = new List<CryptoElement>();
		for (int i = 0; i < num2 * 4 / 35; i++)
		{
			list.Add(new Matrix());
		}
		for (int j = 0; j < num2 * 10 / 35; j++)
		{
			list.Add(new NumOp());
		}
		for (int k = 0; k < num2 * 6 / 35; k++)
		{
			list.Add(new Swap());
		}
		for (int l = 0; l < num2 * 9 / 35; l++)
		{
			list.Add(new BinOp());
		}
		for (int m = 0; m < num2 * 6 / 35; m++)
		{
			list.Add(new RotateBit());
		}
		for (int n = 0; n < 16; n++)
		{
			list.Add(new AddKey(n));
		}
		Shuffle(random, list);
		int[] array = Enumerable.Range(0, 16).ToArray();
		int num3 = 16;
		bool flag = false;
		foreach (CryptoElement item in list)
		{
			item.Initialize(random);
			for (int num4 = 0; num4 < item.DataCount; num4++)
			{
				if (num3 == 16)
				{
					flag = true;
					num3 = 0;
				}
				item.DataIndexes[num4] = array[num3++];
			}
			if (flag)
			{
				Shuffle(random, array);
				num3 = 0;
				flag = false;
			}
		}
		CipherGenContext cipherGenContext = new CipherGenContext(random, 16);
		foreach (CryptoElement item2 in list)
		{
			item2.Emit(cipherGenContext);
		}
		encrypt = cipherGenContext.Block;
		PostProcessStatements(encrypt, random);
		CipherGenContext cipherGenContext2 = new CipherGenContext(random, 16);
		foreach (CryptoElement item3 in Enumerable.Reverse(list))
		{
			item3.EmitInverse(cipherGenContext2);
		}
		decrypt = cipherGenContext2.Block;
		PostProcessStatements(decrypt, random);
	}
}
