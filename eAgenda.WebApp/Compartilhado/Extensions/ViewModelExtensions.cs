using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Compartilhado.Extensions;

public static class ViewModelExtensions
{
    public static FormFieldViewModel ConvertToFormField<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression, string type, string? label = null, IReadOnlyDictionary<string, object?>? attributes = null)
    {
        var name = GetMemberName(expression.Body) ?? throw new ArgumentException($"Could not determine the field name from expression '{expression}'.", nameof(expression));
        var value = expression.Compile()(model);

        return new FormFieldViewModel(name, label ?? name, value, type, attributes);
    }

    public static FormSelectViewModel ConvertToFormSelect<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> options, string? label = null, string? placeholder = null)
    {
        var name = GetMemberName(expression.Body) ?? throw new ArgumentException($"Could not determine the field name from expression '{expression}'.", nameof(expression));
        var value = expression.Compile()(model);

        return new FormSelectViewModel(name, label ?? name, value, options, placeholder);
    }

    public static FormSelectViewModel ConvertToFormSelect<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression, string? label = null, string? placeholder = null)
    {
        var name = GetMemberName(expression.Body) ?? throw new ArgumentException($"Could not determine the field name from expression '{expression}'.", nameof(expression));
        var value = expression.Compile()(model);
        var valueType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

        if (!valueType.IsEnum)
            throw new ArgumentException($"The expression '{expression}' must point to an enum property.", nameof(expression));

        var options = Enum.GetValues(valueType)
            .Cast<Enum>()
            .Select(enumValue => new SelectListItem
            {
                Value = enumValue.ToString(),
                Text = GetEnumDisplayName(enumValue)
            });

        return new FormSelectViewModel(name, label ?? name, value, options, placeholder);
    }

    public static DisplayFieldViewModel DisplayField<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression, string? label = null)
    {
        var name = GetMemberName(expression.Body);
        if (name is null && label is null)
            throw new ArgumentException($"Could not determine the field name from expression '{expression}'. Provide the label explicitly.", nameof(expression));
        var value = expression.Compile()(model);

        return new((label ?? name)!, value is Enum enumValue? GetEnumDisplayName(enumValue) : value);
    }

    public static DisplayFieldViewModel DisplayField<TModel, TValue>(this TModel _, Expression<Func<TModel, TValue>> expression, object? value, string? label = null)
    {
        var name = GetMemberName(expression.Body);
        if (name is null && label is null)
            throw new ArgumentException($"Could not determine the field name from expression '{expression}'. Provide the label explicitly.", nameof(expression));

        return new((label ?? name)!, value);
    }

    private static string? GetMemberName(Expression expression)
    {
        return expression switch
        {
            MemberExpression member => member.Member.Name,
            MethodCallExpression { Object: not null } method => GetMemberName(method.Object),
            MethodCallExpression { Arguments.Count: > 0 } method => GetMemberName(method.Arguments[0]),
            UnaryExpression unary => GetMemberName(unary.Operand),
            _ => null
        };
    }

    private static string GetEnumDisplayName(Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var displayAttribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .OfType<DisplayAttribute>()
            .FirstOrDefault();

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
