﻿#region License Header
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
#endregion

using System.Text.Json.Serialization;

namespace CSharpRepl.Services.Completion.OpenAI.CompletionApi;

/// <summary>
/// OpenAI Completion request - data for HTTP POST.
/// Creates a completion for the provided prompt and parameters.
/// https://platform.openai.com/docs/api-reference/completions/create
/// </summary>
internal sealed class CompletionRequest
{
    /// <summary>
    /// ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    /// <summary>
    /// The prompt(s) to generate completions for, encoded as a string, array of strings, array of tokens, or array of token arrays.
    /// Note that <|endoftext|> is the document separator that the model sees during training, so if a prompt is not specified the
    /// model will generate as if from the beginning of a new document.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    /// <summary>
    /// The suffix that comes after a completion of inserted text.
    /// </summary>
    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; }

    /// <summary>
    /// The maximum number of tokens to generate in the completion.
    /// The token count of your prompt plus max_tokens cannot exceed the model's context length.
    /// Most models have a context length of 2048 tokens (except for the newest models, which support 4096).
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more
    /// random, while lower values like 0.2 will make it more focused and deterministic.
    /// We generally recommend altering this or top_p but not both.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the
    /// results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10%
    /// probability mass are considered. We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopProbability { get; set; }

    /// <summary>
    /// How many completions to generate for each prompt.
    /// Note: Because this parameter generates many completions, it can quickly consume your token quota.
    /// Use carefully and ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    [JsonPropertyName("n")]
    public int? NCompletions { get; set; }

    /// <summary>
    /// Whether to stream back partial progress. If set, tokens will be sent as data-only server-sent
    /// events as they become available, with the stream terminated by a data: [DONE] message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    /// Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens.
    /// For example, if logprobs is 5, the API will return a list of the 5 most likely tokens. The API
    /// will always return the logprob of the sampled token, so there may be up to logprobs+1 elements
    /// in the response.
    ///
    /// The maximum value for logprobs is 5. If you need more than this, please contact us through our
    /// Help center and describe your use case.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public int? LogProbabilities { get; set; }

    /// <summary>
    /// Echo back the prompt in addition to the completion
    /// </summary>
    [JsonPropertyName("echo")]
    public bool? Echo { get; set; }

    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not
    /// contain the stop sequence.
    /// </summary>
    [JsonPropertyName("stop")]
    public string[]? Stop { get; set; }

    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in
    /// the text so far, increasing the model's likelihood to talk about new topics.
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public int? PresencePenalty { get; set; }

    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency
    /// in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public int? FrequencyPenalty { get; set; }

    /// <summary>
    /// Generates best_of completions server-side and returns the "best" (the one with the highest log
    /// probability per token). Results cannot be streamed. When used with n, best_of controls the number
    /// of candidate completions and n specifies how many to return – best_of must be greater than n.
    ///
    /// Note: Because this parameter generates many completions, it can quickly consume your token quota.
    /// Use carefully and ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    [JsonPropertyName("best_of")]
    public int? BestOf { get; set; }

    /// <summary>
    /// Modify the likelihood of specified tokens appearing in the completion.
    ///
    /// Accepts a json object that maps tokens (specified by their token ID in the GPT tokenizer) to an
    /// associated bias value from -100 to 100. You can use this tokenizer tool (which works for both
    /// GPT-2 and GPT-3) to convert text to token IDs. Mathematically, the bias is added to the logits
    /// generated by the model prior to sampling. The exact effect will vary per model, but values
    /// between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100
    /// should result in a ban or exclusive selection of the relevant token.
    ///
    /// As an example, you can pass {"50256": -100} to prevent the <|endoftext|> token from being generated.
    /// </summary>
    [JsonPropertyName("logit_bias")]
    public int? LogitBias { get; set; }

    /// <summary>
    /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }
}
