using System.Linq.Expressions;

namespace TesteTecnico.Application.Utils;

public static class ExpressionExtensions
{
    // Permite combinar duas expressões lógicas com AND (&&) de forma dinâmica
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        // Novo parâmetro comum para a expressão combinada
        var parameter = Expression.Parameter(typeof(T));

        // Substitui o parâmetro da primeira expressão pelo novo parâmetro
        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        // Substitui o parâmetro da segunda expressão pelo novo parâmetro
        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        // Retorna a expressão combinada: (expr1 && expr2)
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(left!, right!),
            parameter
        );
    }
}

// Visitor para substituir parâmetros de expressões
// Necessário para combinar expressões sem conflitos de parâmetros
class ReplaceExpressionVisitor : ExpressionVisitor
{
    private readonly Expression _oldValue;
    private readonly Expression _newValue;

    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    {
        _oldValue = oldValue;
        _newValue = newValue;
    }

    // Substitui o antigo valor pelo novo quando encontrado
    public override Expression? Visit(Expression? node)
    {
        if (node == _oldValue)
            return _newValue;

        return base.Visit(node);
    }
}