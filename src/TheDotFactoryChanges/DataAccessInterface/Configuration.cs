using System;

namespace DataAccessInterface
{
    public class Configuration : ICloneable
    {
        public static readonly string[] RotationDisplayString = new string[]
        {
            "0°",
            "90°",
            "180°",
            "270°"
        };

        public static readonly string[] DescriptorFormatDisplayString = new string[]
        {
            "Don't display",
            "In bits",
            "In bytes"
        };

        public Configuration()
        {

        }
        public Configuration(Configuration cfg)
        {
            Id = cfg.Id;

            commentVariableName = cfg.commentVariableName;
            commentCharVisualizer = cfg.commentCharVisualizer;
            commentCharDescriptor = cfg.commentCharDescriptor;
            commentStyle = cfg.commentStyle;
            bmpVisualizerChar = cfg.bmpVisualizerChar;

            rotation = cfg.rotation;

            flipHorizontal = cfg.flipHorizontal;
            flipVertical = cfg.flipVertical;

            paddingRemovalHorizontal = cfg.paddingRemovalHorizontal;
            paddingRemovalVertical = cfg.paddingRemovalVertical;

            lineWrap = cfg.lineWrap;

            bitLayout = cfg.bitLayout;
            byteOrder = cfg.byteOrder;
            byteFormat = cfg.byteFormat;
            byteLeadingString = cfg.byteLeadingString;

            generateLookupArray = cfg.generateLookupArray;
            descCharWidth = cfg.descCharWidth;
            descCharHeight = cfg.descCharHeight;
            descFontHeight = cfg.descFontHeight;
            generateLookupBlocks = cfg.generateLookupBlocks;
            lookupBlocksNewAfterCharCount = cfg.lookupBlocksNewAfterCharCount;
            descImgHeight = cfg.descImgHeight;
            descImgWidth = cfg.descImgWidth;

            generateSpaceCharacterBitmap = cfg.generateSpaceCharacterBitmap;
            spaceGenerationPixels = cfg.spaceGenerationPixels;
            minHeight = cfg.minHeight;

            varNfBitmaps = cfg.varNfBitmaps;
            varNfCharInfo = cfg.varNfCharInfo;
            varNfFontInfo = cfg.varNfFontInfo;
            varNfHeight = cfg.varNfHeight;
            varNfWidth = cfg.varNfWidth;

            displayName = cfg.displayName;

            commentStartString = cfg.commentStartString;
            commentBlockEndString = cfg.commentBlockEndString;
            commentBlockMiddleString = cfg.commentBlockMiddleString;
            commentEndString = cfg.commentEndString;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        // leading strings
        public const string ByteLeadingStringBinary = "0b";
        public const string ByteLeadingStringHex = "0x";
        public const string ApplicationVersion = "v0.1";

        // comments
        public bool commentVariableName = true;
        public bool commentCharVisualizer = true;
        public bool commentCharDescriptor = true;
        public CommentStyle commentStyle = CommentStyle.Cpp;
        public String bmpVisualizerChar = "#";

        // rotation
        public Rotation rotation = Rotation.RotateZero;

        // flip
        public bool flipHorizontal = false;
        public bool flipVertical = false;

        // padding removal
        public PaddingRemoval paddingRemovalHorizontal = PaddingRemoval.Fixed;
        public PaddingRemoval paddingRemovalVertical = PaddingRemoval.Tighest;

        // line wrap
        public LineWrap lineWrap = LineWrap.AtColumn;

        // byte
        public BitLayout bitLayout = BitLayout.RowMajor;
        public ByteOrder byteOrder = ByteOrder.MsbFirst;
        public ByteFormat byteFormat = ByteFormat.Hex;
        public string byteLeadingString = ByteLeadingStringHex;

        // descriptors
        public bool generateLookupArray = true;
        public DescriptorFormat descCharWidth = DescriptorFormat.DisplayInBits;
        public DescriptorFormat descCharHeight = DescriptorFormat.DontDisplay;
        public DescriptorFormat descFontHeight = DescriptorFormat.DisplayInBytes;
        public bool generateLookupBlocks = false;
        public int lookupBlocksNewAfterCharCount = 80;
        public DescriptorFormat descImgWidth = DescriptorFormat.DisplayInBytes;
        public DescriptorFormat descImgHeight = DescriptorFormat.DisplayInBits;

        // space generation
        public bool generateSpaceCharacterBitmap = false;
        public int spaceGenerationPixels = 2;
        public int minHeight = 2;

        // variable formats
        public string varNfBitmaps = "const uint_8 {0}Bitmaps";
        public string varNfCharInfo = "const FONT_CHAR_INFO {0}Descriptors";
        public string varNfFontInfo = "const FONT_INFO {0}FontInfo";
        public string varNfWidth = "const uint_8 {0}Width";
        public string varNfHeight = "const uint_8 {0}Height";

        // display name
        public string displayName = "default";

        public string commentStartString = "//";
        public string commentBlockEndString = "//";
        public string commentBlockMiddleString = "//";
        public string commentEndString = "//";
    }
}
