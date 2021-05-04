﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LangRepl.Roslyn;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Completion;
using PrettyPrompt.Highlighting;
using PromptCompletionItem = PrettyPrompt.Completion.CompletionItem;

namespace LangRepl.PromptConfiguration
{
    /// <summary>
    /// Maps the roslyn datatypes into prompt configuration datatypes
    /// </summary>
    class PromptAdapter
    {
        public IReadOnlyCollection<FormatSpan> AdaptSyntaxClassification(IReadOnlyCollection<ClassifiedSpan> classifications)
        {
            return classifications
                .Select(r => new FormatSpan(r.TextSpan.Start, r.TextSpan.Length, ToColor(r.ClassificationType)))
                .Where(f => f.Formatting is not null)
                .ToArray();
        }

        public IReadOnlyList<PromptCompletionItem> AdaptCompletions(IReadOnlyCollection<CompletionItemWithDescription> completions)
        {
            return completions
                .OrderByDescending(i => i.Item.Rules.MatchPriority)
                .Select(r => new PromptCompletionItem
                {
                    StartIndex = r.Item.Span.Start,
                    ReplacementText = r.Item.DisplayTextPrefix + r.Item.DisplayText + r.Item.DisplayTextSuffix,
                    ExtendedDescription = r.DescriptionProvider
                })
                .ToArray()
                ??
                Array.Empty<PromptCompletionItem>();
        }

        internal static ConsoleFormat ToColor(string classificationType) =>
            classificationType switch
            {
                "string" => new ConsoleFormat(AnsiColor.BrightYellow),
                "number" => new ConsoleFormat(AnsiColor.BrightBlue),
                "operator" => new ConsoleFormat(AnsiColor.Magenta),
                "preprocessor keyword" => new ConsoleFormat(AnsiColor.Magenta),
                "keyword" => new ConsoleFormat(AnsiColor.Magenta),
                "keyword - control" => new ConsoleFormat(AnsiColor.Magenta),

                "record class name" => new ConsoleFormat(AnsiColor.BrightCyan),
                "class name" => new ConsoleFormat(AnsiColor.BrightCyan),
                "struct name" => new ConsoleFormat(AnsiColor.BrightCyan),

                "comment" => new ConsoleFormat(AnsiColor.Cyan),
                _ => null
            };
    }
}
