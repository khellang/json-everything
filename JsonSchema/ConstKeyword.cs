﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;

namespace Json.Schema
{
	[SchemaKeyword(Name)]
	[SchemaDraft(Draft.Draft6)]
	[SchemaDraft(Draft.Draft7)]
	[SchemaDraft(Draft.Draft201909)]
	[Vocabulary(Vocabularies.Validation201909Id)]
	[JsonConverter(typeof(ConstKeywordJsonConverter))]
	public class ConstKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "const";

		public JsonElement Value { get; }

		public ConstKeyword(JsonElement value)
		{
			Value = value.Clone();
		}

		public void Validate(ValidationContext context)
		{
			context.IsValid = Value.IsEquivalentTo(context.LocalInstance);
			if (!context.IsValid)
				context.Message = "Expected value to match given value";
		}
	}

	public class ConstKeywordJsonConverter : JsonConverter<ConstKeyword>
	{
		public override ConstKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var element = JsonDocument.ParseValue(ref reader).RootElement;

			return new ConstKeyword(element);
		}
		public override void Write(Utf8JsonWriter writer, ConstKeyword value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(ConstKeyword.Name);
			value.Value.WriteTo(writer);
		}
	}
}