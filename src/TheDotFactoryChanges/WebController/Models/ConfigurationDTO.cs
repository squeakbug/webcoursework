namespace WebControllers.Models
{
    public class ConfigurationDTO
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public long Id { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Always)]
        public long UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("commentVariableName", Required = Newtonsoft.Json.Required.Always)]
        public bool CommentVariableName { get; set; }

        [Newtonsoft.Json.JsonProperty("commentCharVisualizer", Required = Newtonsoft.Json.Required.Always)]
        public bool CommentCharVisualizer { get; set; }

        [Newtonsoft.Json.JsonProperty("commentCharDescriptor", Required = Newtonsoft.Json.Required.Always)]
        public bool CommentCharDescriptor { get; set; }

        [Newtonsoft.Json.JsonProperty("commentStyle", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationCommentStyle CommentStyle { get; set; }

        [Newtonsoft.Json.JsonProperty("bmpVisualizerChar", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string BmpVisualizerChar { get; set; }

        [Newtonsoft.Json.JsonProperty("rotation", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationRotation Rotation { get; set; }

        [Newtonsoft.Json.JsonProperty("flipHorizontal", Required = Newtonsoft.Json.Required.Always)]
        public bool FlipHorizontal { get; set; }

        [Newtonsoft.Json.JsonProperty("flipVertical", Required = Newtonsoft.Json.Required.Always)]
        public bool FlipVertical { get; set; }

        [Newtonsoft.Json.JsonProperty("paddingRemovalHorizontal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationPaddingRemovalHorizontal PaddingRemovalHorizontal { get; set; }

        [Newtonsoft.Json.JsonProperty("paddingRemovalVertical", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationPaddingRemovalVertical PaddingRemovalVertical { get; set; }

        [Newtonsoft.Json.JsonProperty("bitLayout", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationBitLayout BitLayout { get; set; }

        [Newtonsoft.Json.JsonProperty("byteOrder", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationByteOrder ByteOrder { get; set; }

        [Newtonsoft.Json.JsonProperty("byteFormat", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ConfigurationByteFormat ByteFormat { get; set; }

        [Newtonsoft.Json.JsonProperty("generateLookupArray", Required = Newtonsoft.Json.Required.Always)]
        public bool GenerateLookupArray { get; set; }

        [Newtonsoft.Json.JsonProperty("generateSpaceCharacterBitmap", Required = Newtonsoft.Json.Required.Always)]
        public bool GenerateSpaceCharacterBitmap { get; set; }

        [Newtonsoft.Json.JsonProperty("spaceGenerationPixels", Required = Newtonsoft.Json.Required.Always)]
        public long SpaceGenerationPixels { get; set; }

        [Newtonsoft.Json.JsonProperty("minHeight", Required = Newtonsoft.Json.Required.Always)]
        public long MinHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("varNfBitmaps", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string VarNfBitmaps { get; set; }

        [Newtonsoft.Json.JsonProperty("varNfCharInfo", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string VarNfCharInfo { get; set; }

        [Newtonsoft.Json.JsonProperty("varNfFontInfo", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string VarNfFontInfo { get; set; }

        [Newtonsoft.Json.JsonProperty("varNfWidth", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string VarNfWidth { get; set; }

        [Newtonsoft.Json.JsonProperty("varNfHeight", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string VarNfHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("displayName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string DisplayName { get; set; }

        [Newtonsoft.Json.JsonProperty("commentStartString", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CommentStartString { get; set; }

        [Newtonsoft.Json.JsonProperty("commentBlockEndString", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CommentBlockEndString { get; set; }

        [Newtonsoft.Json.JsonProperty("commentBlockMiddleString", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CommentBlockMiddleString { get; set; }

        [Newtonsoft.Json.JsonProperty("commentEndString", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CommentEndString { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }
}
