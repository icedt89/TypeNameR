using JanHafner.TypeNameR.StackTrace;
using System.Text;

namespace JanHafner.TypeNameR.Helper;

internal static class AppendHelper
{
    public static void AppendArrayRank(this StringBuilder stringBuilder, int arrayRank)
        => stringBuilder
            .Append(Constants.LeftSquareBracket)
            .Append(Constants.Comma, arrayRank - 1)
            .Append(Constants.RightSquareBracket);

    public static void AppendPointerMarker(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Asterisk);

    public static void AppendNullableMarker(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.QuestionMark);

    public static void AppendPlus(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Plus);

    public static void AppendNamespace(this StringBuilder stringBuilder, ReadOnlySpan<char> @namespace)
        => stringBuilder.Append(@namespace).Append(Constants.FullStop);

    public static void AppendCommaWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.CommaWithEndingSpace);

    public static void AppendGreaterThanSign(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.GreaterThanSign);

    public static void AppendLessThanSign(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.LessThanSign);

    public static void AppendLeftParenthesis(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.LeftParenthesis);

    public static void AppendRightParenthesis(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.RightParenthesis);

    public static void AppendThisWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.ThisWithEndingSpace);

    public static void AppendAsyncWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.AsyncWithEndingSpace);

    public static void AppendSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Space);

    public static void AppendFullStop(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.FullStop);

    public static void AppendConstructor(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Constructor);

    public static void AppendStaticConstructor(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.StaticConstructor);

    public static void AppendPrivateWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.PrivateWithEndingSpace);

    public static void AppendPublicWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.PublicWithEndingSpace);

    public static void AppendStaticWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.StaticWithEndingSpace);

    public static void AppendParameterName(this StringBuilder stringBuilder, ReadOnlySpan<char> parameterName)
        => stringBuilder.Append(Constants.Space).Append(parameterName);

    public static void AppendOutWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.OutWithEndingSpace);

    public static void AppendInWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.InWithEndingSpace);

    public static void AppendRefWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.RefWithEndingSpace);

    public static void AppendParamsWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.ParamsWithEndingSpace);

    public static void AppendEqualsDefaultWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsDefaultWithLeadingSpace);

    public static void AppendEqualsNullWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsNullWithLeadingSpace);

    #region AppendEqualsValue

    private static StringBuilder AppendEqualsValuePrefix(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Space + Constants.EqualsSignWithEndingSpace);

    public static void AppendEqualsQuotedValue(this StringBuilder stringBuilder, ReadOnlySpan<char> value)
        => stringBuilder
            .AppendEqualsValuePrefix()
            .Append(Constants.QuotationMark)
            .Append(value)
            .Append(Constants.QuotationMark);

    public static void AppendEqualsEnumValue(this StringBuilder stringBuilder, ReadOnlySpan<char> enumTypeName, ReadOnlySpan<char> memberName)
        => stringBuilder.AppendEqualsValuePrefix().Append(enumTypeName).Append(Constants.FullStop).Append(memberName);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, ReadOnlySpan<char> value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, bool value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, char value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, sbyte value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, byte value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, short value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, int value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, long value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, float value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, double value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, decimal value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, ushort value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, uint value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, ulong value)
        => stringBuilder.AppendEqualsValuePrefix().Append(value);

    #endregion

    public static void AppendUnknownStackFrameName(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.UnknownStackFrameName);

    public static void AppendMoveNextCallSuffix(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.MoveNextCallSuffix);

    public static void AppendCallDepth(this StringBuilder stringBuilder, int callDepth)
        => stringBuilder.Append(Constants.RecursionMarkWithLeadingAndEndingSpace).Append(callDepth);

    public static void AppendExceptionMessage(this StringBuilder stringBuilder, ReadOnlySpan<char> exceptionMessage)
        => stringBuilder.Append(Constants.ColonWithEndingSpace).Append(exceptionMessage);

    public static void AppendStackFramePreamble(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Indent + Constants.AtWithEndingSpace);

    public static void AppendStackFrameMetadata(this StringBuilder stringBuilder, in StackFrameMetadata stackFrameMetadata)
    {
        stringBuilder.Append(Constants.InSourceWithLeadingAndEndingSpace).Append(stackFrameMetadata.FileName);

        if (stackFrameMetadata.LineNumber == 0)
        {
            return;
        }

        stringBuilder.Append(Constants.LineWithEndingSpace).Append(stackFrameMetadata.LineNumber);

        if (stackFrameMetadata.ColumnNumber > 0)
        {
            stringBuilder.Append(Constants.Colon).Append(stackFrameMetadata.ColumnNumber);
        }
    }

    public static void AppendDynamic(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Dynamic);
}