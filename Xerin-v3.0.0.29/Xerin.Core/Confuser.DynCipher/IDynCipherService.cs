using System.Collections.Generic;
using Confuser.DynCipher.AST;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Generator;

namespace Confuser.DynCipher;

public interface IDynCipherService
{
	void GenerateCipherPair(RandomGenerator random, out StatementBlock encrypt, out StatementBlock decrypt);

	void GenerateExpressionPair(RandomGenerator random, Expression var, Expression result, int depth, out Expression expression, out Expression inverse);

	void GenerateExpressionMutation(RandomGenerator random, MethodDef method, Local stateVariable, IList<Instruction> instructions, int initialValue, int depth);

	void GenerateStatementMutation(RandomGenerator random, MethodDef method, Local stateVariable, IList<Instruction> instructions, int initialValue, int depth);
}
