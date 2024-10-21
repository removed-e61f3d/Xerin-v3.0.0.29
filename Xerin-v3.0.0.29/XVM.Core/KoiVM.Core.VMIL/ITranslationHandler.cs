using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL;

public interface ITranslationHandler
{
	IROpCode IRCode { get; }

	void Translate(IRInstruction instr, ILTranslator tr);
}
