using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public interface IHaveBlock
    {
        public List<SemanticOperator> Block { get; set; }
        
        /// <summary>
        ///     Удаление использованных переменных блока из общего хранилища 
        /// </summary>
        static void ClearVariableStorage(List<SemanticOperator> block)
        {
            var operators = block
                .Where(x => x is Variable)
                .Cast<Variable>()
                .ToList();

            // operators.ForEach(v => VariableStorage.Remove(v.Id));
            operators.ForEach(v => v.GetRoot().Module.VariableStorage.Remove(v.Id));
        }
    }
}