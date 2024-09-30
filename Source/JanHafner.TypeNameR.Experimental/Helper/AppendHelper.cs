using System.Text;
using StackFrameMetadata = JanHafner.TypeNameR.Experimental.StackTrace.StackFrameMetadata;

namespace JanHafner.TypeNameR.Experimental.Helper;

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

    public static void AppendNamespace(this StringBuilder stringBuilder, string @namespace)
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

    public static void AppendParameterName(this StringBuilder stringBuilder, string parameterName)
        => stringBuilder.Append(Constants.Space).Append(parameterName);

    public static void AppendOutWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.OutWithEndingSpace);

    public static void AppendInWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.InWithEndingSpace);

    public static void AppendRefWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.RefWithEndingSpace);

    public static void AppendParamsWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.ParamsWithEndingSpace);

    public static void AppendQuotedParameterValue(this StringBuilder stringBuilder, string value)
        => stringBuilder
            .Append(Constants.EqualsSignWithEndingSpace + Constants.QuotationMark)
            .Append(value)
            .Append(Constants.QuotationMark);

    public static void AppendEqualsDefaultWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsSign + Constants.DefaultWithLeadingSpace);

    public static void AppendEqualsNullWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsSign + Constants.NullWithLeadingSpace);

    public static void AppendEqualsValue(this StringBuilder stringBuilder, object value)
        => stringBuilder.Append(Constants.EqualsSignWithEndingSpace).Append(value);

    public static void AppendUnknownStackFrameName(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.UnknownStackFrameName);

    public static void AppendMoveNextCallSuffix(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.MoveNextCallSuffix);

    public static void AppendCallDepth(this StringBuilder stringBuilder, uint callDepth)
        => stringBuilder.Append(Constants.RecursionMarkWithLeadingAndEndingSpace).Append(callDepth);

    public static void AppendExceptionMessage(this StringBuilder stringBuilder, string exceptionMessage)
        => stringBuilder.Append(Constants.ColonWithEndingSpace).Append(exceptionMessage);

    public static void AppendStackFramePreamble(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Indent + Constants.AtWithEndingSpace);

    public static void AppendStackFrameMetadata(this StringBuilder stringBuilder, in StackFrameMetadata stackFrameMetadata)
    {
        stringBuilder.AppendSourceFileName(stackFrameMetadata.FileName!);

        if (stackFrameMetadata.LineNumber == 0)
        {
            return;
        }

        stringBuilder.AppendLineNumber(stackFrameMetadata.LineNumber);

        if (stackFrameMetadata.ColumnNumber > 0)
        {
            stringBuilder.AppendColumNumber(stackFrameMetadata.ColumnNumber);
        }
    }

    private static void AppendSourceFileName(this StringBuilder stringBuilder, string fileName)
        => stringBuilder.Append(Constants.InSourceWithLeadingAndEndingSpace).Append(fileName);

    private static void AppendLineNumber(this StringBuilder stringBuilder, int lineNumber)
        => stringBuilder.Append(Constants.LineWithEndingSpace).Append(lineNumber);

    private static void AppendColumNumber(this StringBuilder stringBuilder, int columnNumber)
        => stringBuilder.Append(Constants.Colon).Append(columnNumber);

    public static void AppendDynamic(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Dynamic);
}