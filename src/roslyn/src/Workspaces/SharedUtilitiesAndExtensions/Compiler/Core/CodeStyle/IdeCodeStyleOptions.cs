﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;

#if !CODE_STYLE
using Microsoft.CodeAnalysis.Host;
#endif

namespace Microsoft.CodeAnalysis.CodeStyle;

internal abstract class IdeCodeStyleOptions
{
    protected static readonly CodeStyleOption2<bool> s_trueWithSuggestionEnforcement =
        new(value: true, notification: NotificationOption2.Suggestion);

    protected static readonly CodeStyleOption2<bool> s_trueWithSilentEnforcement =
        new(value: true, notification: NotificationOption2.Silent);

    private static readonly CodeStyleOption2<UnusedParametersPreference> s_preferAllMethodsUnusedParametersPreference =
        new(UnusedParametersPreference.AllMethods, NotificationOption2.Suggestion);

    private static readonly CodeStyleOption2<AccessibilityModifiersRequired> s_accessibilityModifiersRequiredDefault =
        new(SyntaxFormattingOptions.CommonOptions.Default.AccessibilityModifiersRequired, NotificationOption2.Silent);

    private static readonly CodeStyleOption2<ParenthesesPreference> s_alwaysForClarityPreference =
        new(ParenthesesPreference.AlwaysForClarity, NotificationOption2.Silent);

    private static readonly CodeStyleOption2<ParenthesesPreference> s_neverIfUnnecessaryPreference =
        new(ParenthesesPreference.NeverIfUnnecessary, NotificationOption2.Silent);

    private static readonly CodeStyleOption2<ForEachExplicitCastInSourcePreference> s_forEachExplicitCastInSourceNonLegacyPreference =
        new(ForEachExplicitCastInSourcePreference.WhenStronglyTyped, NotificationOption2.Suggestion);

    [DataContract]
    internal sealed record class CommonOptions
    {
        public static readonly CommonOptions Default = new();

        [DataMember] public CodeStyleOption2<bool> PreferObjectInitializer { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferCollectionInitializer { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferSimplifiedBooleanExpressions { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public OperatorPlacementWhenWrappingPreference OperatorPlacementWhenWrapping { get; init; } = OperatorPlacementWhenWrappingPreference.BeginningOfLine;
        [DataMember] public CodeStyleOption2<bool> PreferCoalesceExpression { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferNullPropagation { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferExplicitTupleNames { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferAutoProperties { get; init; } = s_trueWithSilentEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferInferredTupleNames { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferInferredAnonymousTypeMemberNames { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferIsNullCheckOverReferenceEqualityMethod { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferConditionalExpressionOverAssignment { get; init; } = s_trueWithSilentEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferConditionalExpressionOverReturn { get; init; } = s_trueWithSilentEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferCompoundAssignment { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> PreferSimplifiedInterpolation { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<UnusedParametersPreference> UnusedParameters { get; init; } = s_preferAllMethodsUnusedParametersPreference;
        [DataMember] public CodeStyleOption2<AccessibilityModifiersRequired> AccessibilityModifiersRequired { get; init; } = s_accessibilityModifiersRequiredDefault;
        [DataMember] public CodeStyleOption2<bool> PreferReadonly { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<ParenthesesPreference> ArithmeticBinaryParentheses { get; init; } = s_alwaysForClarityPreference;
        [DataMember] public CodeStyleOption2<ParenthesesPreference> OtherBinaryParentheses { get; init; } = s_alwaysForClarityPreference;
        [DataMember] public CodeStyleOption2<ParenthesesPreference> RelationalBinaryParentheses { get; init; } = s_alwaysForClarityPreference;
        [DataMember] public CodeStyleOption2<ParenthesesPreference> OtherParentheses { get; init; } = s_neverIfUnnecessaryPreference;
        [DataMember] public CodeStyleOption2<ForEachExplicitCastInSourcePreference> ForEachExplicitCastInSource { get; init; } = s_forEachExplicitCastInSourceNonLegacyPreference;
        [DataMember] public CodeStyleOption2<bool> PreferNamespaceAndFolderMatchStructure { get; init; } = s_trueWithSuggestionEnforcement;
        [DataMember] public CodeStyleOption2<bool> AllowMultipleBlankLines { get; init; } = s_trueWithSilentEnforcement;
        [DataMember] public CodeStyleOption2<bool> AllowStatementImmediatelyAfterBlock { get; init; } = s_trueWithSilentEnforcement;
        [DataMember] public string RemoveUnnecessarySuppressionExclusions { get; init; } = "";
    }

    [DataMember]
    public CommonOptions Common { get; init; } = CommonOptions.Default;

#if !CODE_STYLE
    public static IdeCodeStyleOptions GetDefault(LanguageServices languageServices)
        => languageServices.GetRequiredService<ICodeStyleService>().DefaultOptions;
#endif
}

internal static class IdeCodeStyleOptionsProviders
{
    public static IdeCodeStyleOptions.CommonOptions GetCommonCodeStyleOptions(this IOptionsReader options, string language, IdeCodeStyleOptions.CommonOptions? fallbackOptions)
    {
        fallbackOptions ??= IdeCodeStyleOptions.CommonOptions.Default;

        return new()
        {
            PreferObjectInitializer = options.GetOption(CodeStyleOptions2.PreferObjectInitializer, language, fallbackOptions.PreferObjectInitializer),
            PreferCollectionInitializer = options.GetOption(CodeStyleOptions2.PreferCollectionInitializer, language, fallbackOptions.PreferCollectionInitializer),
            PreferSimplifiedBooleanExpressions = options.GetOption(CodeStyleOptions2.PreferSimplifiedBooleanExpressions, language, fallbackOptions.PreferSimplifiedBooleanExpressions),
            OperatorPlacementWhenWrapping = options.GetOption(CodeStyleOptions2.OperatorPlacementWhenWrapping, fallbackOptions.OperatorPlacementWhenWrapping),
            PreferCoalesceExpression = options.GetOption(CodeStyleOptions2.PreferCoalesceExpression, language, fallbackOptions.PreferCoalesceExpression),
            PreferNullPropagation = options.GetOption(CodeStyleOptions2.PreferNullPropagation, language, fallbackOptions.PreferNullPropagation),
            PreferExplicitTupleNames = options.GetOption(CodeStyleOptions2.PreferExplicitTupleNames, language, fallbackOptions.PreferExplicitTupleNames),
            PreferAutoProperties = options.GetOption(CodeStyleOptions2.PreferAutoProperties, language, fallbackOptions.PreferAutoProperties),
            PreferInferredTupleNames = options.GetOption(CodeStyleOptions2.PreferInferredTupleNames, language, fallbackOptions.PreferInferredTupleNames),
            PreferInferredAnonymousTypeMemberNames = options.GetOption(CodeStyleOptions2.PreferInferredAnonymousTypeMemberNames, language, fallbackOptions.PreferInferredAnonymousTypeMemberNames),
            PreferIsNullCheckOverReferenceEqualityMethod = options.GetOption(CodeStyleOptions2.PreferIsNullCheckOverReferenceEqualityMethod, language, fallbackOptions.PreferIsNullCheckOverReferenceEqualityMethod),
            PreferConditionalExpressionOverAssignment = options.GetOption(CodeStyleOptions2.PreferConditionalExpressionOverAssignment, language, fallbackOptions.PreferConditionalExpressionOverAssignment),
            PreferConditionalExpressionOverReturn = options.GetOption(CodeStyleOptions2.PreferConditionalExpressionOverReturn, language, fallbackOptions.PreferConditionalExpressionOverReturn),
            PreferCompoundAssignment = options.GetOption(CodeStyleOptions2.PreferCompoundAssignment, language, fallbackOptions.PreferCompoundAssignment),
            PreferSimplifiedInterpolation = options.GetOption(CodeStyleOptions2.PreferSimplifiedInterpolation, language, fallbackOptions.PreferSimplifiedInterpolation),
            UnusedParameters = options.GetOption(CodeStyleOptions2.UnusedParameters, language, fallbackOptions.UnusedParameters),
            AccessibilityModifiersRequired = options.GetOption(CodeStyleOptions2.AccessibilityModifiersRequired, language, fallbackOptions.AccessibilityModifiersRequired),
            PreferReadonly = options.GetOption(CodeStyleOptions2.PreferReadonly, language, fallbackOptions.PreferReadonly),
            ArithmeticBinaryParentheses = options.GetOption(CodeStyleOptions2.ArithmeticBinaryParentheses, language, fallbackOptions.ArithmeticBinaryParentheses),
            OtherBinaryParentheses = options.GetOption(CodeStyleOptions2.OtherBinaryParentheses, language, fallbackOptions.OtherBinaryParentheses),
            RelationalBinaryParentheses = options.GetOption(CodeStyleOptions2.RelationalBinaryParentheses, language, fallbackOptions.RelationalBinaryParentheses),
            OtherParentheses = options.GetOption(CodeStyleOptions2.OtherParentheses, language, fallbackOptions.OtherParentheses),
            ForEachExplicitCastInSource = options.GetOption(CodeStyleOptions2.ForEachExplicitCastInSource, fallbackOptions.ForEachExplicitCastInSource),
            PreferNamespaceAndFolderMatchStructure = options.GetOption(CodeStyleOptions2.PreferNamespaceAndFolderMatchStructure, language, fallbackOptions.PreferNamespaceAndFolderMatchStructure),
            AllowMultipleBlankLines = options.GetOption(CodeStyleOptions2.AllowMultipleBlankLines, language, fallbackOptions.AllowMultipleBlankLines),
            AllowStatementImmediatelyAfterBlock = options.GetOption(CodeStyleOptions2.AllowStatementImmediatelyAfterBlock, language, fallbackOptions.AllowStatementImmediatelyAfterBlock),
            RemoveUnnecessarySuppressionExclusions = options.GetOption(CodeStyleOptions2.RemoveUnnecessarySuppressionExclusions, fallbackOptions.RemoveUnnecessarySuppressionExclusions)
        };
    }
}
