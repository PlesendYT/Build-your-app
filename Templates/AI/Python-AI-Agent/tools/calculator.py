"""Calculator tool for the AI agent using safe expression evaluation."""

from __future__ import annotations

import ast
import operator

# Mapping of AST node types to Python operator functions
_OPERATORS: dict[type, callable] = {
    ast.Add: operator.add,
    ast.Sub: operator.sub,
    ast.Mult: operator.mul,
    ast.Div: operator.truediv,
    ast.Pow: operator.pow,
    ast.USub: operator.neg,
}


def _safe_eval(node: ast.AST) -> float:
    """Recursively evaluate an AST node using only allowed operations."""
    if isinstance(node, ast.Expression):
        return _safe_eval(node.body)
    if isinstance(node, ast.Constant):
        return float(node.value)
    if isinstance(node, ast.BinOp):
        op_func = _OPERATORS.get(type(node.op))
        if op_func is None:
            raise ValueError(f"Unsupported operator: {type(node.op).__name__}")
        return op_func(_safe_eval(node.left), _safe_eval(node.right))
    if isinstance(node, ast.UnaryOp) and isinstance(node.op, ast.USub):
        return -_safe_eval(node.operand)
    raise ValueError(f"Unsupported expression: {type(node).__name__}")


def calculate(expression: str) -> str:
    """Evaluate a mathematical expression safely without using eval()."""
    try:
        tree = ast.parse(expression.strip(), mode="eval")
        result = _safe_eval(tree)
        return f"{result:.2f}" if result != int(result) else str(int(result))
    except Exception as e:
        return f"Error: {e}"
