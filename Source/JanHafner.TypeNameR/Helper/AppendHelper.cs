using JanHafner.TypeNameR.StackTrace;
using System.Runtime.CompilerServices;
using System.Text;

namespace JanHafner.TypeNameR.Helper;

internal static class AppendHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendArrayRank(this StringBuilder stringBuilder, int arrayRank) 
        => stringBuilder
            .Append(Constants.LeftSquareBracket)
            .Append(Constants.Comma, arrayRank - 1)
            .Append(Constants.RightSquareBracket);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendPointerMarker(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Asterisk);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendNullableMarker(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.QuestionMark);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendPlus(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Plus);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendNamespace(this StringBuilder stringBuilder, string @namespace)
        => stringBuilder.Append(@namespace).AppendFullStop();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendCommaWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.CommaWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendGreaterThanSign(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.GreaterThanSign);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendLessThanSign(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.LessThanSign);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendLeftParenthesis(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.LeftParenthesis);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendThisWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.ThisWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendRightParenthesis(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.RightParenthesis);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendAsyncWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.AsyncWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Space);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendFullStop(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.FullStop);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendConstructor(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Constructor);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendStaticConstructor(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.StaticConstructor);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendPrivateWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.PrivateWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendPublicWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.PublicWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendStaticWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.StaticWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendParameterName(this StringBuilder stringBuilder, string parameterName)
        => stringBuilder.AppendSpace().Append(parameterName);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendOutWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.OutWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendInWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.InWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendRefWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.RefWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendParamsWithEndingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.ParamsWithEndingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendQuotedParameterValue(this StringBuilder stringBuilder, string value)
        => stringBuilder
            .Append(Constants.EqualsSignWithEndingSpace + Constants.QuotationMark)
            .Append(value)
            .Append(Constants.QuotationMark);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendEqualsDefaultWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsSign + Constants.DefaultWithLeadingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendEqualsNullWithLeadingSpace(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.EqualsSign + Constants.NullWithLeadingSpace);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendEqualsValue(this StringBuilder stringBuilder, object value)
        => stringBuilder.Append(Constants.EqualsSignWithEndingSpace).Append(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendNewLine(this StringBuilder stringBuilder)
        => stringBuilder.AppendLine();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendUnknownStackFrameName(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.UnknownStackFrameName);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendMoveNextCallSuffix(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.MoveNextCallSuffix);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendCallDepth(this StringBuilder stringBuilder, uint callDepth)
        => stringBuilder.Append(Constants.RecursionMarkWithLeadingAndEndingSpace).Append(callDepth);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendExceptionMessage(this StringBuilder stringBuilder, string exceptionMessage)
        => stringBuilder.Append(Constants.ColonWithEndingSpace).Append(exceptionMessage);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendStackFramePreamble(this StringBuilder stringBuilder)
        => stringBuilder.Append(Constants.Indent + Constants.AtWithEndingSpace);

    public static StringBuilder AppendStackFrameMetadata(this StringBuilder stringBuilder, StackFrameMetadata stackFrameMetadata)
    {
        stringBuilder.AppendSourceFileName(stackFrameMetadata.FileName);

        if (stackFrameMetadata.LineNumber == 0)
        {
            return stringBuilder;
        }

        stringBuilder.AppendLineNumber(stackFrameMetadata.LineNumber);

        if (stackFrameMetadata.ColumnNumber > 0)
        {
            stringBuilder.AppendColumNumber(stackFrameMetadata.ColumnNumber);
        }

        return stringBuilder;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringBuilder AppendSourceFileName(this StringBuilder stringBuilder, string fileName)
        => stringBuilder.Append(Constants.InSourceWithLeadingAndEndingSpace).Append(fileName);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringBuilder AppendLineNumber(this StringBuilder stringBuilder, int lineNumber)
        => stringBuilder.Append(Constants.LineWithEndingSpace).Append(lineNumber);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringBuilder AppendColumNumber(this StringBuilder stringBuilder, int columnNumber)
        => stringBuilder.Append(Constants.Colon).Append(columnNumber);
}