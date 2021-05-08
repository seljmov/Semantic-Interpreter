namespace Semantic_Interpreter.Core
{
    public class SemanticTree
    {
        private int _count;
        public SemanticOperator Root { get; set; }

        public void InsertOperator(SemanticOperator prevOperator, SemanticOperator newOperator, bool asChild = false)
        {
            if (prevOperator == null && newOperator is Module)
            {
                InsertRoot(newOperator);
            }
            else if (newOperator is Beginning beginning)
            {
                ((Module) FindOperator(prevOperator)).SetBeginning(beginning);
            }
            else if (asChild)
            {
                InsertOperatorAsChild(newOperator, FindOperator(prevOperator));
            }
            else
            {
                InsertOperatorAsNext(newOperator, FindOperator(prevOperator));
            }

            _count++;
        }

        public void TraversalTree()
        {
            var index = 0;
            var curr = Root;
            var parent = Root;
            while (index != _count)
            {
                curr?.Execute();
                
                if (curr?.Child != null)
                {
                    parent = curr;
                    curr = curr.Child;
                }
                else
                {
                    if (curr?.Next != null)
                    {
                        curr = curr.Next;
                    }
                    else
                    {
                        curr = parent.Next;

                        while (curr == null && !(parent is Beginning))
                        {
                            parent = parent.Parent;
                            curr = parent.Next;
                        }
                    }
                }

                index++;
            }
        }
        
        private SemanticOperator FindOperator(SemanticOperator @operator)
        {
            var index = 0;
            var curr = Root;
            var parent = Root;
            while (index != _count)
            {
                if (curr == @operator)
                {
                    return curr;
                }
                else
                {
                    if (curr.Child != null)
                    {
                        parent = curr;
                        curr = curr.Child;
                    }
                    else
                    {
                        if (curr.Next != null)
                        {
                            curr = curr.Next;
                        }
                        else
                        {
                            curr = parent.Next;

                            while (curr == null)
                            {
                                parent = parent.Parent;
                                curr = parent.Next;
                            }
                        }
                    }
                }

                index++;
            }
            
            return null;
        }

        private void InsertRoot(SemanticOperator @operator) => Root = @operator;

        private void InsertOperatorAsPrevious(SemanticOperator newOperator, SemanticOperator previous)
        {
            if (previous.Previous != null)
            {
                previous.Previous.Next = newOperator;
                newOperator.Previous = previous.Previous;
                previous.Previous = newOperator;
                newOperator.Next = previous;
            }
            else
            {
                newOperator.Next = previous.Parent.Child;
                previous.Parent.Child = newOperator;
                previous.Previous = newOperator;
                newOperator.Parent = previous.Parent;
                previous.Parent = null;
            }

            newOperator.Parent = previous.Parent;
        }

        private static void InsertOperatorAsNext(SemanticOperator newOperator, SemanticOperator next)
        {
            if (next.Next != null)
            {
                next.Next.Previous = newOperator;
                newOperator.Next = next.Next;
            }

            next.Next = newOperator;
            newOperator.Previous = next;
            newOperator.Parent = next.Parent;
        }

        private static void InsertOperatorAsChild(SemanticOperator newOperator, SemanticOperator child)
        {
            if (child.Child != null)
            {
                newOperator.Next = child.Child;
                child.Child.Previous = newOperator;
                child.Child.Parent = null;
            }

            child.Child = newOperator;
            newOperator.Parent = child;
            newOperator.Parent = child;
        }
    }
}